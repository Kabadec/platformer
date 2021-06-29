using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {

        protected override void Start()
        {
            base.Start();

            var force = new Vector2(Direction * _speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}