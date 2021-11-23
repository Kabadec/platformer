using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected bool _invertX;


        protected Rigidbody2D Rigidbody;
        protected int Direction;

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            ResetProjectile();
        }

        public virtual void ResetProjectile()
        {
            var mod = _invertX ? -1 : 1;
            Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
        }

    }
}