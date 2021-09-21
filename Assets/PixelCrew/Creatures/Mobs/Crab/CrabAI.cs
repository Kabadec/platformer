using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;

namespace PixelCrew.Creatures.Mobs.Crab
{
    public class CrabAI : MobAI
    {
        [SerializeField] private ColliderCheck _canAttack1;
        [SerializeField] private float _CDJump = 1f;

        private Crab _crab;

        protected override void Awake()
        {
            base.Awake();
            _crab = GetComponent<Crab>();
        }

        private int GetDistToTarget()
        {
            var dist = _target.transform.position - transform.position;
            if ((int)dist.x == 0)
                return 1;
            else
                return (int)dist.x;
        }
        protected override IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer || _canAttack1.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    yield return new WaitForSeconds(_CDJump);
                    LookAtHero();
                    _crab.AddForce(GetDistToTarget());
                    yield return new WaitForSeconds(0.5f);
                    while (!_creature.IsGrounded)
                    {
                        yield return null;
                    }
                    _particles.Spawn("Fall");
                }
                yield return null;
            }
            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);
            StartState(_patrol.DoPatrol());
        }
        protected override IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer || _canAttack1.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToHero());
        }

    }
}