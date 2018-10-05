using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Ship : Object
    {
        private Turret[] turrets;
        private float energy;
        private float health;

        public Ship(Vector2 position) : base(TextureLibrary.GetTexture("Ship"))
        {
            Position = position;
            Point size = new Point(70, 70);

            Rectangle    = new Rectangle(position.ToPoint(), size);
            Acceleration = new Vector2(100, 0);
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            if(Math.Abs(Velocity.X) > 100)
            {
                Acceleration = -Acceleration;
            }

            base.Update(gameTime);
        }
    }
}