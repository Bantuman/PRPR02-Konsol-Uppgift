using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    class Missile : Projectile
    {
        public Missile(IGame1 game, Texture2D texture) : base(game, 0, texture)
        {
        }
    }
}