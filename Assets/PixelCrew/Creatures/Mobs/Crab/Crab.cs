using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Creatures;
namespace PixelCrew.Creatures.Mobs.Crab
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Crab : Creature
    {
        [SerializeField] private float _x = 1;
        [SerializeField] private float _coefX = 1.3f;

        [SerializeField] private float _coefY = 2.2f;
        [SerializeField] private float _minY = 10f;
        
        [Space]
        [SerializeField] private LayerMask _defaultLayer;
        [SerializeField] private LayerMask _dieLayer;

        private HeroAll.Hero _hero;
        private Rigidbody2D _rb;
        protected override void Awake()
        {
            base.Awake();
            _hero = FindObjectOfType<HeroAll.Hero>();
            _rb = GetComponent<Rigidbody2D>(); ;
        }
        protected override void FixedUpdate()
        {
            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
        }

        [ContextMenu("AddForce")]
        private void AddForce()
        {
            float y = 0;
            if (_x * _coefY < _minY)
                y = 7;
            else
                y = _x * _coefY;

            _rb.AddForce(new Vector2(_x * _coefX, y), ForceMode2D.Impulse);
        }
        public void AddForce(int x)
        {
            float y = 0;
            if (x * _coefY < _minY)
                y = 7;
            else
                y = x * _coefY;

            _rb.AddForce(new Vector2(x * _coefX, y), ForceMode2D.Impulse);
            _particles.Spawn("Jump");
        }
        public override void TakeDamage()
        {
            base.TakeDamage();
        }
        public void JumpUp()
        {
            _rb.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
        }

        public void ChangeLayerToDieLayer()
        {
            gameObject.layer = (int)Mathf.Log(_dieLayer.value, 2);;
        }
    }
}