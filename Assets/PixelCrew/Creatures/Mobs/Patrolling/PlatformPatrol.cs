using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private int _direction = -1;
        [SerializeField] private OnChangeDirection _onChangeDirection;
        
        private void Start()
        {
            ReloadDirection();
        }

        private void OnEnable()
        {
            ReloadDirection();
        }

        public override IEnumerator DoPatrol()
        {
            ReloadDirection();
            while (enabled)
            {
                if (!_groundCheck.IsTouchingLayer || _obstacleCheck.IsTouchingLayer)
                {
                    ChangeDirection();
                }
                yield return null;
                yield return null;
                yield return null;
            }
        }

        private void ChangeDirection()
        {
            _direction = -_direction;
            _onChangeDirection?.Invoke(new Vector2(_direction, 0));
        }
        private void ReloadDirection()
        {           
            _onChangeDirection?.Invoke(new Vector2(_direction, 0));
        }
    }

    [Serializable]
    public class OnChangeDirection : UnityEvent<Vector2>
    {
        
    }
}