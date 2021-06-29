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

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;


        [SerializeField] private float _fallVelocity;
        [SerializeField] private float _secForDisablePlatform;
        [SerializeField] private int _numSwordsOnMultiThrow = -3;


        [SerializeField] private Cooldown _singleThrowCoolDown;
        [SerializeField] private float _multiThrowCoolDown = 0.1f;
        [SerializeField] private float _waitBeforeMultiThrow = 1f;

        [SerializeField] AnimatorController _armed;
        [SerializeField] AnimatorController _disarmed;



        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;


        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");



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
        private Coroutine _multiThrowCoroutine;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;

        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            _swordAmmo = GetComponent<SwordsAmmoComponent>();
            health.SetHealth(_session.Data.Hp);
            _swordAmmo.SetSwords(_session.Data.SwordsAmmo);
            UpdateHeroWeapon();

        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }
        public void OnSwordAmmoChanged(int currentSwordAmmo)
        {
            _session.Data.SwordsAmmo = currentSwordAmmo;
        }


        protected override void Update()
        {
            base.Update();
            TimerForPlatform();
            TimerForThrow();

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
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddCoins(int cost)
        {
            _session.Data.Coins += cost;
            //Debug.Log($"У вас {_session.Data.Coins} монет");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
            //Debug.Log($"У вас {_sumCoins} монет");

        }
        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
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
        private void TimerForThrow()
        {
            if (!_isShiftPressed && _triggerThrow)
            {
                var timeShiftPressed = Time.time - _timeWhereShiftPressed;

                if (timeShiftPressed < _waitBeforeMultiThrow)
                {
                    if (_singleThrowCoolDown.IsReady)
                    {
                        _swordAmmo.ModifySwordsAmmo(-1);
                        _singleThrowCoolDown.Reset();
                    }
                }
                else
                {
                    _swordAmmo.ModifySwordsAmmo(-_numSwordsOnMultiThrow);
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
            if (_session.Data.SwordsAmmo <= 0) return;
            base.Attack();
        }

        public void GoArmSword()
        {
            UpdateHeroWeapon();
        }
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = (_session.Data.SwordsAmmo > 0) ? _armed : _disarmed;
            //Animator.runtimeAnimatorController = _armed;
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
                yield return new WaitForSeconds(_multiThrowCoolDown);
            }
        }
        public void CallSingleThrow()
        {
            _swordAmmo.ModifySwordsAmmo(-1);
        }
        public void CallMultiThrow()
        {
            if (_multiThrowCoroutine != null)
                _swordAmmo.ModifySwordsAmmo(_numSwordsOnMultiThrow);
        }


    }
}