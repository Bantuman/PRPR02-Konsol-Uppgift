using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    class Bolt : Projectile
    {
        public override float Rotation => MathLibrary.Direction(Velocity);
        private Ship bulletOwner;
        private Random random;
        public Bolt(IGame1 game, float damage, Ship owner, Vector2 direction, float speed, Vector2 position) : base(game, damage, TextureLibrary.GetTexture("Bullet"))
        {
            random = new Random();
            Rectangle = new RotatedRectangle(new Rectangle(0, 0, 8, 4), 0);
            Position  = position;
            bulletOwner = owner;
            if (direction.Length() == 0)
            {
                direction = Vector2.One;
            }
            direction.Normalize();
            Velocity = direction * speed;
            Collider.OnCollisionEnter += OnHit;
        }
    
        private void OnHit(object sender, Collider.CollisionHitInfo args)
        {
            if (args.Object == bulletOwner as Object)
            {
                return;
            }

            if (args.Object is Ship ship)
            {
                ship.TakeDamage(Damage);
            }

            Game.Instantiate(new Explosion(Game, Position, (float)random.NextDouble() * 2 + 1, (float)random.NextDouble() * 2 + 1));
            Destroy();
        }
    }
}