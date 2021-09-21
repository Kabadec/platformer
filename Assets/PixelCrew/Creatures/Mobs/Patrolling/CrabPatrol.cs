using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Creatures.Mobs.Crab;
using PixelCrew.Components.GoBased;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class CrabPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private int _distJump = 1;
        [SerializeField] float _treshold = 1f;

        private Creature _creature;
        private PixelCrew.Creatures.Mobs.Crab.Crab _crab;
        private SpawnListComponent _particles;
        private int _destinationPointIndex;


        private void Awake()
        {
            _creature = GetComponent<Creature>();
            _crab = GetComponent<PixelCrew.Creatures.Mobs.Crab.Crab>();
            _particles = GetComponent<SpawnListComponent>();
        }
        public override IEnumerator DoPatrol()
        {
            yield return new WaitForSeconds(0.4f);
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }
                var direction = _points[_destinationPointIndex].position - transform.position;
                direction = direction.normalized;
                if (direction.x >= 0 && direction.x < 1)
                    direction.x = 1;
                if (direction.x < 0 && direction.x > -1)
                    direction.x = -1;

                _creature.SetDirection(direction);
                _creature.UpdateSpriteDirection(direction);
                _crab.AddForce((int)direction.x * _distJump);

                yield return new WaitForSeconds(0.5f);
                while (!_creature.IsGrounded)
                {
                    yield return null;
                }
                _particles.Spawn("Fall");
                yield return new WaitForSeconds(1f);
                yield return null;
            }
        }
        private bool IsOnPoint()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
        }
    }
}