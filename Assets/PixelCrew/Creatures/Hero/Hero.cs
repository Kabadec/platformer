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
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils.Disposables;
using Random = UnityEngine.Random;

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
        

        [Space]
        [SerializeField] AnimatorController _armed;
        [SerializeField] AnimatorController _disarmed;
        
        [SerializeField] private SpawnComponent _throwSpawner;
        
        [Header("Particles")]
        [SerializeField] private RandomSpawner _hitDrop;
        [Header("ForceShield")]
        [SerializeField] private GameObject _forceShield;

        [SerializeField] private float _durationForceShield = 3;

        [Header("SwordShield")] 
        [SerializeField] private GameObject _swordShieldPrefab;
        
        [Header("Candle")] 
        [SerializeField] private GameObject _candle;



        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private const string SwordId = "Sword";
        private const string PotionSpeedId = "SpeedPotion";


        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private string SelectedId => _session.QuickInventory.SelectedItem.Id;
        private InventoryItemData SelectedItem => _session.QuickInventory.SelectedItem;
        
        

        //private float _timeHowPerkUsed = 0;

        //public float TimeHowPerkUsed => _timeHowPerkUsed;
        private float _coolDownPerk;

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
        private CameraShakeEffect _cameraShake;
        private Coroutine _multiThrowCoroutine;
        private Coroutine _forceShieldCoroutine;

        private GameObject _swordShieldGO;
        public bool IsPause { get; set; }
        
        public bool CanControlHero { get; set; }


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
            CanControlHero = true;
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;
            
            //_health.SetHealth(_session.Data.Hp.Value);
            _trash.Retain(_session.PerksModel.Subscribe(OnActivePerkChanged));
            OnActivePerkChanged();

            OnHeroUpgraded(StatId.Hp);
            UpdateHeroWeapon();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int) _session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    _health.SetHealth(health);
                    break;

            }
        }


        private void OnActivePerkChanged()
        {
            var def = DefsFacade.I.Perks.Get(_session.PerksModel.Used);
            _coolDownPerk = def.Cooldown;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.StatsModel.OnUpgraded -= OnHeroUpgraded;

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
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall
                && Time.time > _session.PerksModel.TimeHowPerkUsed + _coolDownPerk)
            {
                _session.PerksModel.SetTimeHowPerkUsed(Time.time);
                DoJumpVfx();
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override float CalculateSpeed()
        {
            if(_isBuffSpeed)
                return _session.StatsModel.GetValue(StatId.Speed) + DefsFacade.I.Potions.Get(PotionSpeedId).Value;
            else
                return _session.StatsModel.GetValue(StatId.Speed);
        }

        public bool AddInInventory(string id, int value)
        {
            return _session.Data.Inventory.Add(id, value);
        }
        public override void TakeDamage()
        {
            base.TakeDamage();
            _cameraShake?.Shake();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
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
                    if (_session.PerksModel.IsSuperThrowSupported 
                        && Time.time > _session.PerksModel.TimeHowPerkUsed + _coolDownPerk)
                    {
                        MultiThrow();
                    }
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
            if(SelectedItem == null) return;
            if (DefsFacade.I.Itemses.Get(SelectedId).HasTag(ItemTag.Throwable))
            {
                SingleThrow();
                return;
            }else if (DefsFacade.I.Itemses.Get(SelectedId).HasTag(ItemTag.Potion))
            {
                UsePotion();
            }
            
        }
        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    if (_session.Data.Inventory.Count(SelectedId) > 0)
                    {
                        if (_health.ModifyHealth((int)potion.Value))
                            _session.Data.Inventory.Remove(SelectedId, 1);
                    }
                    break;
                case Effect.SpeedUp:
                    if (!_isBuffSpeed && _session.Data.Inventory.Count(SelectedId) > 0)
                    {
                        _speedBuffCoroutine = StartCoroutine(SpeedBuffCoroutine(SelectedId));
                        _session.Data.Inventory.Remove(SelectedId, 1);
                    }
                    break;
            }
            
        }
        
        public void UseForceShield()
        {
            if (_session.PerksModel.IsForceShieldSupported
                && Time.time > _session.PerksModel.TimeHowPerkUsed + _coolDownPerk)
            {
                _session.PerksModel.SetTimeHowPerkUsed(Time.time);
                _forceShieldCoroutine = StartCoroutine(ForceShieldCoroutine());
            }
        }

        private IEnumerator ForceShieldCoroutine()
        {
            _health.Immune.Retain(this);
            _forceShield.SetActive(true);
            yield return new WaitForSeconds(_durationForceShield);
            _health.Immune.Release(this);
            _forceShield.SetActive(false);
        }

        public IEnumerator SpeedBuffCoroutine(string id)
        {
            var potion = DefsFacade.I.Potions.Get(PotionSpeedId);
            _isBuffSpeed = true;
            yield return new WaitForSeconds(potion.Time);
            _isBuffSpeed = false;
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
            if (SelectedId == null) return false;
            if (!DefsFacade.I.Itemses.Get(SelectedId).HasTag(ItemTag.Throwable)) return false;
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
            _session.PerksModel.SetTimeHowPerkUsed(Time.time);
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
            var projectile = _throwSpawner.SpawnInstance();

            ApplyRangeDamageStat(projectile);
            
            
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<ModifyHealthComponent>();
            var damageValue = _session.StatsModel.GetValue(StatId.RangeDamage);
            damageValue = ModifyDamageByCrit((int)damageValue);
            hpModify.SetHpDelta((int) (-1 * damageValue));
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critChange = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChange)
                return damage * 2;
            
            return damage;
        }
        
        public bool TryUseSwordShield()
        {
            if (_session.PerksModel.IsSwordShieldSupported 
                && Time.time > _session.PerksModel.TimeHowPerkUsed + _coolDownPerk)
            {
                _session.PerksModel.SetTimeHowPerkUsed(Time.time);
                SwordShield();
                return true;
            }

            return false;
        }
        private void SwordShield()
        {
            if(_swordShieldGO != null) Destroy(_swordShieldGO);
            _swordShieldGO = Instantiate(_swordShieldPrefab, gameObject.transform);
        }
#if UNITY_EDITOR
        public void CheatSwordShield()
        {
            SwordShield();
        }
#endif
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
        
        
        
        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void OnOffCandle()
        {
            _candle.SetActive(!_candle.activeSelf);
        }

        
        
    }
}