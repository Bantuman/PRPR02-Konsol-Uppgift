using Battleships.Libraries;
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

        private float roundsPerMinute;
        private float timeSinceLastShot;
        private float currentDuration;

        public Vector2 RelativePosition { get; set; }
        public Vector2 Size             { get; set; }
        public float FireInterval => 60 / roundsPerMinute;
        public Texture2D Texture { get; }

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
            
        }

        public void Fire(float duration)
        {
            
        }
    }
}