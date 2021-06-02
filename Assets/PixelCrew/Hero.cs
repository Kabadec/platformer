using System;
using UnityEngine;
using PixelCrew.Components;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _jumpDustParticles;
        [SerializeField] private SpawnComponent _fallDustParticles;


        private Collider2D[] _interactionResult = new Collider2D[1];
        private Vector2 _direction = Vector2.zero;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private int _sumCoins = 0;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int Hit = Animator.StringToHash("hit");


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {

            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            _animator.SetBool(IsRunning, _direction.x != 0);

            UpdateSpriteDirection();
            //_rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                if (_allowDoubleJump == false)
                {
                    SpawnFallDust();
                }
                _allowDoubleJump = true;
            }
            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);

            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                SpawnJumpDust();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                SpawnJumpDust();
            }
            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

        public void TakeCoin(int cost)
        {
            _sumCoins += cost;
            Debug.Log($"У вас {_sumCoins} монет");
        }
        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            if (_sumCoins > 0)
            {
                SpawnCoins();
            }
            Debug.Log($"У вас {_sumCoins} монет");

        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_sumCoins, 5);
            _sumCoins -= numCoinsToDispose;
            //Debug.Log($"У вас {_sumCoins} монет");

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();

        }
        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }
        public void SpawnJumpDust()
        {
            _jumpDustParticles.Spawn();
        }
        public void SpawnFallDust()
        {
            _fallDustParticles.Spawn();
        }

    }
}