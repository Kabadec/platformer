using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {

        [SerializeField] private bool _isNextGround = true;
        [SerializeField] private bool _isNextWall = false;


        private Creature _creature;
        private bool _isRightDirection = true;

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
                if (!_isNextGround || _isNextWall)
                {
                    ChangeDirection();
                }
                yield return null;
            }
        }

        public void SetIsNextGround(bool isNextGround)
        {
            _isNextGround = isNextGround;
        }
        public void SetIsNextWall(bool isNextWall)
        {
            _isNextWall = isNextWall;
        }

        private void ChangeDirection()
        {
            if (_isRightDirection)
            {
                _creature.SetDirection(Vector2.left);
                _isRightDirection = false;
            }
            else
            {
                _creature.SetDirection(Vector2.right);
                _isRightDirection = true;
            }
        }
        private void ReloadDirection()
        {
            if (_isRightDirection)
            {
                _creature.SetDirection(Vector2.right);
            }
            else
            {
                _creature.SetDirection(Vector2.left);
            }
        }
    }
}