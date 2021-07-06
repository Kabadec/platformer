using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components.ColliderBased;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;


        private Creature _creature;
        private int _direction = -1;

        private void Start()
        {
            _creature = GetComponent<Creature>();
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
            }
        }

        private void ChangeDirection()
        {
            _direction = -_direction;
            _creature.SetDirection(new Vector2(_direction, 0));
        }
        private void ReloadDirection()
        {
            _creature.SetDirection(new Vector2(_direction, 0));
        }
    }
}