using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    class Bolt : Projectile
    {
        public Bolt(float damage, Ship owner, Vector2 direction, float speed, Vector2 position) : base(damage, TextureLibrary.GetTexture("Bullet"))
        {
            Rectangle = new RotatedRectangle(new Rectangle(0, 0, 10, 10), 0);
            Position  = position;

            if (direction.Length() == 0)
            {
                direction = Vector2.One;
            }
            direction.Normalize();
            Velocity = direction * speed;
        }
    }
}