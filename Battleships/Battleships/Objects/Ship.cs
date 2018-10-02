using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battleships.Objects
{
    abstract class Ship : IObject
    {
        private Turret[] turrets;
        private float energy;
        private float health;

        public Rectangle Rectangle { get; private set; }
        public Vector2 Position { get; set; }

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
    }
}