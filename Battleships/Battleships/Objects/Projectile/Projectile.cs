using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battleships.Objects.Projectile
{
    abstract class Projectile : IObject
    {
        private float damage;

        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Rectangle Rectangle => throw new NotImplementedException();

        public event EventHandler OnDestroy;

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