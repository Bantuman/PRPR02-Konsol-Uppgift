using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    public class AIPlayer : Ship
    {
        float rotation = 0;

        public AIPlayer(Vector2 position) : base(null, position)
        {
            Acceleration = new Vector2(0, 1f) * 40f * ((position.Y > 1) ? -1 : 1);
        }

        public override void Act()
        {
            Shoot(10);
        }
    }
}