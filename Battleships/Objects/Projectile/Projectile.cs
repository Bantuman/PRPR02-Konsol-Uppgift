﻿using System;
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

        public Projectile(IGame1 game, Texture2D texture) : base(game, texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}