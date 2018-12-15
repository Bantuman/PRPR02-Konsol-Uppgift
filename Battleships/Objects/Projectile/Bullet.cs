using Battleships.Libraries;
using Microsoft.Xna.Framework;
using System;

namespace Battleships.Objects.Projectile
{
    /// <summary>
    /// Regular bullet class.
    /// </summary>
    public class Bullet : Projectile
    {
        public override float Rotation => MathLibrary.Direction(Velocity);

        public const float    DAMAGE   = 10;

        private Random        random;

        private const float SPEED = 200;

        public Bullet(IGame1 game, Ship owner, Vector2 direction, Vector2 position) :
            base(game, DAMAGE, TextureLibrary.GetTexture("Bullet"), owner)
        {
            random = new Random();
            Rectangle = new RotatedRectangle(new Rectangle(0, 0, 4, 2), MathLibrary.Direction(direction));
            Position  = position;
            if (direction.Length() == 0)
            {
                direction = Vector2.One;
            }
            direction.Normalize();
            Velocity = direction * SPEED;
            Collider.OnCollisionEnter += OnHit;
            Layer = 0.01f;
        }

        /// <summary>
        /// Handles hits.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="collisionHitInfo">Hit info.</param>
        private void OnHit(object sender, Collider.CollisionHitInfo collisionHitInfo)
        {
            if (collisionHitInfo.Object == BulletOwner as Object)
            {
                return;
            }

            if (collisionHitInfo.Object is Ship ship)
            {
                ship.TakeDamage(Damage);
            }

            Game.Instantiate(new Explosion(Game, Position, (float)random.NextDouble() * 2 + 1, (float)random.NextDouble() * 2 + 1));
            Destroy();
        }
    }
}