using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    public class Missile : Projectile, IMissile
    {
        // In radians.
        private float targetRotation;
        private float rotation;

        private float speed;
        private Action<GameTime, Missile> guideMissile;
        private Random random;
        private float timeAlive;

        private const float LIFETIME = 2f;
        private const float DAMAGE = 200f;

        public override float Rotation => rotation;

        public Missile(IGame1 game, float rotation, float damage, Vector2 position, Ship owner, Action<GameTime, Missile> guideMissile) : base(game, 0, TextureLibrary.GetTexture("Missile"), owner)
        {
            Rectangle                  = new RotatedRectangle(new Rectangle(0, 0, 16, 8), rotation);
            Position                   = position;
            targetRotation             = this.rotation = rotation;
            Damage                     = damage;
            speed                      = 200;
            this.guideMissile          = guideMissile;
            Collider.OnCollisionEnter += OnCollisionEnter;
            random                     = new Random();
        }

        private void OnCollisionEnter(object sender, Collider.CollisionHitInfo collisionHitInfo)
        {
            if (collisionHitInfo.Object == BulletOwner as Object)
            {
                return;
            }

            if (collisionHitInfo.Object is Ship ship)
            {
                ++BulletOwner.MissilesHit;
                ship.TakeDamage(DAMAGE);
            }

            Game.Instantiate(new Explosion(Game, Position, (float)random.NextDouble() * 3 + 5, (float)random.NextDouble() * 2 + 1));
            Destroy();
        }

        public void RotateTo(float rotation)
        {
            targetRotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            timeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            guideMissile?.Invoke(gameTime, this);
            rotation = MathLibrary.LerpAngle(rotation, targetRotation, 1 - (float)Math.Pow(0.07f, gameTime.ElapsedGameTime.TotalSeconds));
            Velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * speed;
            base.Update(gameTime);
            if (timeAlive >= LIFETIME)
            {
                Destroy();
            }
        }
    }
}