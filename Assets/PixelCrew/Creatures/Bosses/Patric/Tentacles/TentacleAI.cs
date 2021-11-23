using System;
using PixelCrew.Creatures.Mobs.Patrolling;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses.Patric.Tentacles
{
    public class TentacleAI : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Patrol _patrol;

        private void Start()
        {
            StartCoroutine(_patrol.DoPatrol());
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;
        }

        private void Update()
        {
            if (_direction.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if(_direction.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
}