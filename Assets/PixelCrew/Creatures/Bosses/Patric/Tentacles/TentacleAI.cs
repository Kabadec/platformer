using System;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Creatures.Mobs.Patrolling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Creatures.Bosses.Patric.Tentacles
{
    public class TentacleAI : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Patrol _patrol;
        [SerializeField] private LayerCheck _groundCheck;

        private Vector2 _lastDirection;
        private Coroutine _coroutine;

        private void Start()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(_patrol.DoPatrol());
        }
        
        private void OnEnable()
        {
            var directionX = Random.value;
            if (directionX > 0.5f)
                directionX = 1f;
            else
                directionX = -1f;
            
            var direction = new Vector2(directionX, 0);
            _direction = direction.normalized;
            _lastDirection = _direction;
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(_patrol.DoPatrol());
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            var direction = _direction;
            if (!_groundCheck.IsTouchingLayer)
            {
                direction = _lastDirection;
            }
            else
            {
                _lastDirection = _direction;
            }
            var yVelocity = _rigidbody.velocity.y;
            _rigidbody.velocity = new Vector2(direction.x * _speed, yVelocity);
        }

        private void Update()
        {
            
            if (!_groundCheck.IsTouchingLayer)
            {
                if (_lastDirection.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else if(_lastDirection.x < 0)
                    transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                if (_direction.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else if(_direction.x < 0)
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
    }
}