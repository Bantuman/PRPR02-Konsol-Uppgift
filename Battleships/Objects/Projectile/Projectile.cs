using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    abstract class Projectile : Object
    {
        private float damage;

        public Projectile(float damage, Texture2D texture) : base(null, texture)
        {
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}