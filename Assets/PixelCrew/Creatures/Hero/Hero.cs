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
using PixelCrew.Components.SwordAmmo;
using PixelCrew.Components.GoBased;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ColliderCheck _wallCheck;


        [SerializeField] private float _fallVelocity;
        [SerializeField] private float _secForDisablePlatform;
        [SerializeField] private int _numSwordsOnMultiThrow = -3;


        [SerializeField] private Cooldown _singleThrowCoolDown;
        [SerializeField] private float _multiThrowCoolDown = 0.1f;
        [SerializeField] private float _waitBeforeMultiThrow = 1f;

        [SerializeField] AnimatorController _armed;
        [SerializeField] AnimatorController _disarmed;


        [SerializeField] private int _healthValue = 5;



        [Space]
        [Header("Particles")]
        [SerializeField] private RandomSpawner _hitDrop;


        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");


        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");



        private bool _allowDoubleJump;
        private bool _isOnWall;


        private bool _goDownWithPlatform;
        private float _timeForDisablePlatform = 0;
        private bool _triggerPlatform = true;
        private bool _isSPressed;


        private bool _isShiftPressed = false;
        private bool _triggerThrow;
        private float _timeWhereShiftPressed;



        private GameSession _session;
        private float _defaultGravityScale;
        private SwordsAmmoComponent _swordAmmo;
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
            _swordAmmo = GetComponent<SwordsAmmoComponent>();

            _health.SetHealth(_session.Data.Hp);
            _swordAmmo.SetSwords(SwordCount);

            _session.Data.Inventory.OnChanged += OnInventoryChanged;

            UpdateHeroWeapon();

        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }
        public void OnSwordAmmoChanged(int swordAmmoDelta)
        {
            if (swordAmmoDelta >= 0)
                _session.Data.Inventory.Add("Sword", swordAmmoDelta);
            else
                _session.Data.Inventory.Remove("Sword", -1 * swordAmmoDelta);

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
                        CallSingleThrow();
                        _singleThrowCoolDown.Reset();
                    }
                }
                else
                {
                    CallMultiThrow();
                }
                _triggerThrow = false;
            }
            if (_isShiftPressed && !_triggerThrow)
            {
                _timeWhereShiftPressed = Time.time;
                _triggerThrow = true;
            }
        }
        public override void Attack()
        {
            if (SwordCount <= 0) return;
            base.Attack();
        }
        public void UsePotionHealth()
        {
            //Debug.Log("U press H");
            if (_session.Data.Inventory.Count("PotionHealth") > 0)
            {
                _health.ModifyHealth(_healthValue);
                _session.Data.Inventory.Remove("PotionHealth", 1);
                //Debug.Log("U used Potion Health");
            }
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
        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }
        public void SingleThrow()
        {
            if (_singleThrowCoolDown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                Sounds.Play("Range");
                _singleThrowCoolDown.Reset();
            }
        }
        public void MultiThrow(int numSwordsOnMultiThrow)
        {
            _multiThrowCoroutine = StartCoroutine(MultiThrowCorroutine(numSwordsOnMultiThrow));
        }
        public IEnumerator MultiThrowCorroutine(int numSwordsOnMultiThrow)
        {
            for (int i = numSwordsOnMultiThrow - 1; i >= 0; i--)
            {
                Animator.SetTrigger(ThrowKey);
                Sounds.Play("Range");
                yield return new WaitForSeconds(_multiThrowCoolDown);
            }
        }
        public void CallSingleThrow()
        {
            _swordAmmo.ModifySwordsAmmo(-1);
        }
        public void CallMultiThrow()
        {
            _swordAmmo.ModifySwordsAmmo(-1 * _numSwordsOnMultiThrow);
        }


    }
}