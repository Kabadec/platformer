using System.Collections;
using System;
using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;

namespace PixelCrew.Creatures.Mobs
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] protected ColliderCheck _vision;
        [SerializeField] protected ColliderCheck _canAttack;


        [SerializeField] protected float _alarmDelay = 0.5f;
        [SerializeField] protected float _attackCooldown = 1f;
        [SerializeField] protected float _missHeroCooldown = 0.5f;

        protected IEnumerator _current;
        protected GameObject _target;


        protected static readonly int IsDeadKey = Animator.StringToHash("is-dead");


        protected SpawnListComponent _particles;
        protected Creature _creature;
        protected Animator _animator;
        protected bool _isDead;
        protected Patrol _patrol;


        protected virtual void Awake()
        {
            _creature = GetComponent<Creature>();
            _particles = GetComponent<SpawnListComponent>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        protected void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;
            StartState(AgroToHero());
        }
        protected IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }
        protected void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }
        protected virtual IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    ///yield return null;
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }
            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);
            StartState(_patrol.DoPatrol());
        }
        protected virtual IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToHero());
        }
        protected void SetDirectionToTarget()
        {
            _creature.SetDirection(GetDirectionToTarget());
        }
        protected Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }
        protected IEnumerator Patrolling()
        {
            yield return null;
        }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null)
                StopCoroutine(_current);
            _current = coroutine;
            StartCoroutine(coroutine);
        }
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            if (_current != null)
                StopCoroutine(_current);
            _creature.SetDirection(Vector2.zero);
        }
    }
}