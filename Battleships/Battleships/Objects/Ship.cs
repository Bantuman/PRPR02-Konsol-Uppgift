using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Ship : IObject
    {
        private Turret[] turrets;
        private float energy;
        private float health;

        public Rectangle Rectangle { get; private set; }
        public Vector2 Position { get; set; }
        public float Layer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler OnDestroy;

        public Ship(Vector2 position)
        {
            Position = position;
            Point size = new Point(100, 100); // Arbitrary.
            Rectangle = new Rectangle(position.ToPoint(), size);
        }

        public abstract void Act();

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}