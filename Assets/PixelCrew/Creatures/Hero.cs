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

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;


        [SerializeField] private float _fallVelocity;
        [SerializeField] private float _secForDisablePlatform;


        [SerializeField] AnimatorController _armed;
        [SerializeField] AnimatorController _disarmed;



        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;



        private bool _allowDoubleJump;
        private bool _isOnWall;


        private bool _goDownWithPlatform;
        private float _timeForDisablePlatform = 0;
        private bool _triggerPlatform = true;
        private bool _isSPressed;


        private GameSession _session;
        private float _defaultGravityScale;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;

        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();

        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }


        protected override void Update()
        {
            base.Update();
            TimerForPlatform();

            if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

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
            if (!IsGrounded && _allowDoubleJump)
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
        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();

        }
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;

        }
        public void SetSPressed(bool isSPressed)
        {
            _isSPressed = isSPressed;
            //Debug.Log(_isSPressed);
        }
        public bool GetGoDownWithPlatform()
        {
            return _goDownWithPlatform;
        }

    }
}