using Battleships.Libraries;
using Microsoft.Xna.Framework;
using System;

namespace Battleships.Objects.Projectile
{
    /// <summary>
    /// Missile class.
    /// </summary>
    public class Missile : Projectile, IMissile
    {
        public override float             Rotation => rotation;

        public const float                DAMAGE   = 200f;
        public const float                LIFETIME = 2f;

        private float                     targetRotation;
        private float                     rotation;
        private float                     speed;
        private Action<GameTime, Missile> guideMissile;

        private Random                    random;
        internal float                    TimeAlive { get; set; }

        public Missile(IGame1 game, float rotation, float damage, Vector2 position, Ship owner, Action<GameTime, Missile> guideMissile) :
            base(game, 0, TextureLibrary.GetTexture("Missile"), owner)
        {
            Rectangle                  = new RotatedRectangle(new Rectangle(0, 0, 16, 8), rotation);
            Position                   = position;
            targetRotation             = this.rotation = rotation;
            Damage                     = damage;
            speed                      = 200;
            this.guideMissile          = guideMissile;
            Collider.OnCollisionEnter += OnHit;
            random                     = new Random();
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
                ++BulletOwner.MissilesHit;
                ship.TakeDamage(DAMAGE);
            }

            Game.Instantiate(new Explosion(Game, Position, (float)random.NextDouble() * 3 + 5, (float)random.NextDouble() * 2 + 1));
            Destroy();
        }

        /// <summary>
        /// Rotates missile to target rotation.
        /// </summary>
        /// <param name="rotation">Target rotation.</param>
        public void RotateTo(float rotation)
        {
            targetRotation = rotation;
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            TimeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            guideMissile?.Invoke(gameTime, this);

            rotation = MathLibrary.LerpAngle(rotation, targetRotation, 1 - (float)Math.Pow(0.07f, gameTime.ElapsedGameTime.TotalSeconds));
            Velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * speed;

            base.Update(gameTime);
            if (TimeAlive >= LIFETIME)
            {
                Destroy();
            }
        }
    }
}