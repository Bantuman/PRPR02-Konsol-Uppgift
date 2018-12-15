using Battleships.Libraries;
using Battleships.Objects.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battleships.Objects
{
    public class Turret
    {
        private Ship Ship { get; }
        private IGame1 Game { get; }

        private float roundsPerMinute = 50;
        private float timeSinceLastShot;

        public Vector2 RotatedPosition { get; set; }
        public bool FacingLeft { get; set; }
        public Vector2 RelativePosition { get; set; }
        public Vector2 Size { get; set; }
        public Texture2D Texture { get; }
        public float FireInterval => 60 / roundsPerMinute;
        public bool IsFiring { get; set; }

        public Turret(IGame1 game, Ship ship, Vector2 relativePosition, Vector2 size)
        {
            Ship = ship;
            Game = game;
            RelativePosition = relativePosition;
            Size = size;
            Texture = TextureLibrary.GetTexture("Turret");
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        internal void Update(GameTime gameTime)
        {
            if (IsFiring && Ship.Energy > 0)
            {
                if (timeSinceLastShot > FireInterval)
                {
                    Shoot();
                    Ship.LoseEnergy(5);
                    timeSinceLastShot = 0;
                }
            }
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Shoots.
        /// </summary>
        private void Shoot()
        {
            ++Ship.ShotsFired;
            float rotation = Ship.Rotation + ((FacingLeft) ? (float)Math.PI : 0);

            Bullet bolt = (Bullet)Game.Instantiate(new Bullet(Game, Ship, MathLibrary.ConstructVector(rotation + (float)Math.PI / 2), RotatedPosition + Ship.Position));
            bolt.Collider.OnCollisionEnter += Bolt_OnCollisionEnter;
        }

        /// <summary>
        /// If the collided object is a ship, adds one to shots hit.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="collided">Hit info.</param>
        private void Bolt_OnCollisionEnter(object sender, Object.Collider.CollisionHitInfo collided)
        {
            if(collided.Object is Ship)
            {
                ++Ship.ShotsHit;
            }
        }
    }
}