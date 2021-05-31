using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;

        [SerializeField] private LayerCheck _groundCheck;

        private Vector2 _direction = Vector2.zero;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _sprite;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning = Animator.StringToHash("is-running");

        private int _sumCoins = 0;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();

            if (isJumping)
            {
                if (isGrounded && _rigidbody.velocity.y <= 0.01)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
            _animator.SetBool(IsGroundKey, isGrounded);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            _animator.SetBool(IsRunning, _direction.x != 0);

            UpdateSpriteDirection();



            //_rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);



        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
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
    }
}