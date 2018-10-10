using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    class AIPlayer : Ship
    {
        public AIPlayer(Vector2 position) : base(position)
        {
           Acceleration = new Vector2(1.8f, 1f) * 100f * ((position.X > 1) ? -1 : 1);
        }

        public override void Act()
        {
            //Vector2 dv = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - Position);
           // Acceleration = new Vector2(dv.X, dv.Y) * 100f;//new Vector2(1.8f, 1f) * 100f * ((position.X > 1) ? -1 : 1);
        }
    }
}