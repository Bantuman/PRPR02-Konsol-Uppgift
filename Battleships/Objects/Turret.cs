using Battleships.Libraries;
using Battleships.Objects.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    class Turret
    {
        private Ship Ship   { get; }
        private IGame1 Game { get; }

        private float roundsPerMinute = 100;
        private float timeSinceLastShot;
        private float currentDuration;

        public Vector2 RotatedPosition  { get; set; }
        public bool FacingLeft          { get; set; }
        public Vector2 RelativePosition { get; set; }
        public Vector2 Size             { get; set; }
        public Texture2D Texture        { get; }
        public float FireInterval => 60 / roundsPerMinute;

        public Turret(IGame1 game, Ship ship, Vector2 relativePosition, Vector2 size)
        {
            Ship = ship;
            Game = game;
            RelativePosition = relativePosition;
            Size = size;
            Texture = TextureLibrary.GetTexture("Turret");
        }

        internal void Update(GameTime gameTime)
        {
            if (currentDuration > 0)
            {
                if (timeSinceLastShot > FireInterval)
                {
                    Shoot();
                    timeSinceLastShot = 0;
                }
            }
            
            if (currentDuration > 0)
            {
                currentDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (currentDuration < 0)
            {
                currentDuration = 0;
            }
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Shoot()
        {
            float rotation = Ship.Rotation + ((FacingLeft) ? (float)Math.PI : 0);

            Game.Instantiate(new Bolt(Game, Ship.Damage, Ship, MathLibrary.ConstructVector(rotation + (float)Math.PI / 2), 800, RotatedPosition + Ship.Position));
        }

        public void Fire(float duration)
        {
            currentDuration = duration;
        }
    }
}