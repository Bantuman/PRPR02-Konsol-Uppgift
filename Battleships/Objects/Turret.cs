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
        private Ship Ship { get; }

        private float roundsPerMinute;
        private float timeSinceLastShot;
        private float currentDuration;

        public float FireInterval => 60 / roundsPerMinute;

        public Turret(Ship ship)
        {
            Ship = ship;
        }

        public void Update(GameTime gameTime)
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
            //Ship.Game
        }

        public void Fire(float duration)
        {
            
        }
    }
}