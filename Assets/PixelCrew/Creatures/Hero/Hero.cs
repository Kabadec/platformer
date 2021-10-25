using System.Diagnostics.Contracts;
using System.ComponentModel.Design;
using System;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Model;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Components.GoBased;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : Creature, ICanAddInInventory
    {
        [Space]
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ColliderCheck _wallCheck;


        [SerializeField] private float _fallVelocity;
        [SerializeField] private float _secForDisablePlatform;
        
        [Header("Throws")]
        [SerializeField] private int _numAmmoOnMultiThrow = 3;
        [SerializeField] private Cooldown _singleThrowCoolDown;
        [SerializeField] private float _multiThrowCoolDown = 0.1f;
        [SerializeField] private float _waitBeforeMultiThrow = 1f;

        [Header("Potions")]
        [SerializeField] private int _bigPotionHeal = 5;
        [SerializeField] private int _potionHeal = 1;
        [SerializeField] private float _durationSpeedBuff = 5f;
        [SerializeField] private float _powerSpeedBuff = 4;

        [Space]
        [SerializeField] AnimatorController _armed;
        [SerializeField] AnimatorController _disarmed;
        
        [SerializeField] private SpawnComponent _throwSpawner;
        
        [Header("Particles")]
        [SerializeField] private RandomSpawner _hitDrop;


        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private const string SwordId = "Sword";

        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private string SelectedId => _session.QuickInventory.SelectedItem.Id;



        private bool _allowDoubleJump;
        private bool _isOnWall;


        private bool _goDownWithPlatform;
        private float _timeForDisablePlatform = 0;
        private bool _triggerPlatform = true;
        private bool _isSPressed;


        private bool _isShiftPressed = false;
        private bool _triggerThrow;
        private float _timeWhereShiftPressed;

        private bool _isBuffSpeed = false;
        private Coroutine _speedBuffCoroutine;



        private GameSession _session;
        private float _defaultGravityScale;
        private HealthComponent _health;
        private Coroutine _multiThrowCoroutine;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;

        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();

            _health.SetHealth(_session.Data.Hp.Value);

            _session.Data.Inventory.OnChanged += OnInventoryChanged;

            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }
       


        protected override void Update()
        {
            base.Update();
            TimerForPlatform();
            ProcessThrowTimer();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
                Rigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (isJumpPressing && _isSPressed)
            {
                _goDownWithPlatform = true;
                //DropFromPlatform();
                return Rigidbody.velocity.y;

            }
            else if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                DoJumpVfx();
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public bool AddInInventory(string id, int value)
        {
            return _session.Data.Inventory.Add(id, value);
        }
        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
            //Debug.Log($"У вас {_sumCoins} монет");

        }
        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.Restart();
        }
        public void Interact()
        {
            _interactionCheck.Check();
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _fallVelocity)
                {
                    _particles.Spawn("Fall");
                }
            }
        }
        private void TimerForPlatform()
        {
            if (_timeForDisablePlatform > Time.time) return;
            if (_goDownWithPlatform && _triggerPlatform)
            {
                _timeForDisablePlatform = Time.time + _secForDisablePlatform;
                _triggerPlatform = false;
            }
            else if (!_triggerPlatform)
            {
                _goDownWithPlatform = false;
                _triggerPlatform = true;
            }
        }
        private void ProcessThrowTimer()
        {
            if (!_isShiftPressed && _triggerThrow)
            {
                var timeShiftPressed = Time.time - _timeWhereShiftPressed;

                if (timeShiftPressed < _waitBeforeMultiThrow)
                {
                    if (_singleThrowCoolDown.IsReady)
                    {
                        UseSomething();
                        _singleThrowCoolDown.Reset();
                    }
                }
                else
                {
                    MultiThrow();
                }
                _triggerThrow = false;
            }
            if (_isShiftPressed && !_triggerThrow)
            {
                _timeWhereShiftPressed = Time.time;
                _triggerThrow = true;
            }
        }

        private void UseSomething()
        {
            if (DefsFacade.I.Items.Get(SelectedId).HasTag(ItemTag.Throwable))
            {
                SingleThrow();
                return;
            }
            if(SelectedId == "PotionHealth") UsePotionHealth("PotionHealth");
            if(SelectedId == "BigPotionHealth") UsePotionHealth("BigPotionHealth");
            if (SelectedId == "SpeedPotion") UseSpeedPotion();
        }

        

        public override void Attack()
        {
            if (SwordCount <= 0) return;
            base.Attack();
        }
        

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = (SwordCount > 0) ? _armed : _disarmed;
        }
        public void SetSPressed(bool isSPressed)
        {
            _isSPressed = isSPressed;
        }
        public void SetShiftPressed(bool isShiftPressed)
        {
            _isShiftPressed = isShiftPressed;
        }
        public bool GetGoDownWithPlatform()
        {
            return _goDownWithPlatform;
        }
        

        private bool CanThrow()
        {
            if (!DefsFacade.I.Items.Get(SelectedId).HasTag(ItemTag.Throwable)) return false;
            if (SelectedId == SwordId)
            {
                if (SwordCount > 1) return true;
            }
            else
            {
                if (_session.Data.Inventory.Count(SelectedId) > 0) return true;
            }

            return false;
        }
        public void SingleThrow()
        {
            if(!CanThrow()) return;
            if (_singleThrowCoolDown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                Sounds.Play("Range");
                DoThrow();
                _session.Data.Inventory.Remove(SelectedId, 1);
                _singleThrowCoolDown.Reset();
            }
        }
        public void MultiThrow()
        {
            if(!CanThrow()) return;
            var throwableCount = _session.Data.Inventory.Count(SelectedId);
            var possibleCount = SelectedId == SwordId ? throwableCount - 1 : throwableCount;
            var numAmmo = Mathf.Min(_numAmmoOnMultiThrow, possibleCount);;

            _multiThrowCoroutine = StartCoroutine(MultiThrowCoroutine(numAmmo));
        }
        public IEnumerator MultiThrowCoroutine(int numAmmo)
        {
            for (int i = numAmmo - 1; i >= 0; i--)
            {
                Animator.SetTrigger(ThrowKey);
                Sounds.Play("Range");
                DoThrow();
                _session.Data.Inventory.Remove(SelectedId, 1);
                yield return new WaitForSeconds(_multiThrowCoolDown);
            }
        }
        private void DoThrow()
        {
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            if (throwableDef.Projectile == null) return;
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
        }
        

        public void DropFromPlatform()
        {
            var position = transform.position;
            var endPosition = position + new Vector3(0, -1);
            var hit = Physics2D.Linecast(position, endPosition, _groundLayer);

            if (hit.collider == null) return;

            var component = hit.collider.GetComponent<TmpDisableColliderComponent>();
            if (component == null) return;

            component.DisableCollider();

        }
        public void UsePotionHealth(string potionId)
        {
            var deltaHeal = (potionId == "PotionHealth") ? _potionHeal : _bigPotionHeal;
            if (_session.Data.Inventory.Count(potionId) > 0)
            {
                if (_health.ModifyHealth(deltaHeal))
                    _session.Data.Inventory.Remove(potionId, 1);
            }
        }
        private void UseSpeedPotion()
        {
            if (!_isBuffSpeed && _session.Data.Inventory.Count("SpeedPotion") > 0)
            {
                _speedBuffCoroutine = StartCoroutine(SpeedBuffCoroutine());
                _session.Data.Inventory.Remove("SpeedPotion", 1);
            }
        }

        public IEnumerator SpeedBuffCoroutine()
        {
            _isBuffSpeed = true;
            _speed += _powerSpeedBuff;
            yield return new WaitForSeconds(_durationSpeedBuff);
            _isBuffSpeed = false;
            _speed -= _powerSpeedBuff;
        }
        
        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
    }
}