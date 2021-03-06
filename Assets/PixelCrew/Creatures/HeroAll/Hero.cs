using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Creatures.HeroAll.Features;
using PixelCrew.Creatures.HeroAll.Features.HeroFlashLight;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine; //using UnityEditor.Animations;
using Random = UnityEngine.Random;

namespace PixelCrew.Creatures.HeroAll
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
        [SerializeField] RuntimeAnimatorController _armed;
        [SerializeField] RuntimeAnimatorController _disarmed;
        
        
        
        [SerializeField] private SpawnComponent _throwSpawner;
        
        [Header("Particles")]
        [SerializeField] private RandomSpawner _hitDrop;
        [Header("ForceShield")]
        [SerializeField] private HeroForceShieldComponent _heroForceShield;

        [Header("SwordShield")] 
        [SerializeField] private HeroSwordShieldComponent _heroSwordShield;
        
        [Header("FlashLight")] 
        [SerializeField] private GameObject _candle;



        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
        private static readonly int AttackAnimSelect = Animator.StringToHash("attack-anim-select");
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private const string SwordId = "Sword";
        private const string PotionSpeedId = "SpeedPotion";


        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private string SelectedId => _session.QuickInventory.SelectedItem.Id;
        private InventoryItemData SelectedItem => _session.QuickInventory.SelectedItem;
        
        
        //private float _coolDownPerk;

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

        private int _randomAnimAttack = 0;


        private GameSession _session;
        private float _defaultGravityScale;
        private HealthComponent _health;
        private CameraShakeEffect _cameraShake;
        private Coroutine _multiThrowCoroutine;
        private Coroutine _forceShieldCoroutine;

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
            _session = GameSession.Instance;
            _health = GetComponent<HealthComponent>();
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;
            

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

        private bool IsReadyPerk(string id)
        {
            return Time.time > _session.PerksModel.GetTimeHowPerkUsed(id) + DefsFacade.I.Perks.Get(id).Cooldown;
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
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall)
            {
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
                    if (_session.PerksModel.IsSuperThrowSupported && IsReadyPerk("super-throw"))
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
            if (_session.PerksModel.IsForceShieldSupported && IsReadyPerk("force-shield"))
            {
                _session.PerksModel.SetTimeHowPerkUsed("force-shield", Time.time);
                _heroForceShield.Use();
            }
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


            _randomAnimAttack = Random.Range(0, 3);
            Animator.SetInteger(AttackAnimSelect, _randomAnimAttack);
            
            Animator.SetTrigger(AttackKey);

            //base.Attack();
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
            _session.PerksModel.SetTimeHowPerkUsed("super-throw", Time.time);
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

            ApplyRangeStat(projectile);
        }

        private void ApplyRangeStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<ModifyHealthComponent>();
            var damageValue = _session.StatsModel.GetValue(StatId.Damage);
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
            if (_session.PerksModel.IsSwordShieldSupported && IsReadyPerk("sword-shield"))
            {
                _session.PerksModel.SetTimeHowPerkUsed("sword-shield", Time.time);
                _heroSwordShield.Use();
                return true;
            }

            return false;
        }
        
#if UNITY_EDITOR
        public void CheatSwordShield()
        {
            _heroSwordShield.Use();
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
        
        public override void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        public override void OnDoAttack()
        {
            ApplyRangeStat(_attackRange0.gameObject);
            
            _attackRange0.Check();
            if (_attackRange1 != null)
                _attackRange1.Check();
            
            Sounds.Play("Melee");
        }
        
        
        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void OnOffFlashLight()
        {
            var isFlashLightActive = _candle.activeSelf;
            if(isFlashLightActive)
                _candle.SetActive(false);
            else
                if(_session.Data.Oil.Value > 0f)
                    _candle.SetActive(true);
        }
    }
}