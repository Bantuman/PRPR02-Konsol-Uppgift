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

            Rectangle = new Rectangle(position.ToPoint(), size);
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }
    }
}