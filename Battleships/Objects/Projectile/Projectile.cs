using System;
using Battleships.Libraries;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    abstract class Projectile : Object, ICollidable
    {
        protected float Damage { get; set; }
        public new Collider Collider { get; set; }

        public Projectile(IGame1 game, float damage, Texture2D texture) : base(game, texture)
        {
            Damage = damage;
            Collider = new Collider(this, ColliderType.Trigger);
        }
    }
}