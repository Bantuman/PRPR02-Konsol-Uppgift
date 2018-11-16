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
        public AIPlayer(Vector2 position, string name, Color nameColor) : base(null, position, name, nameColor)
        {}

        private void MoveToPoint(Vector2 targetPosition)
        {
            Acceleration = Vector2.Normalize(targetPosition - Position) * 40f;
        }

        public override void Act()
        {
            Shoot(1);
            MoveToPoint(new Vector2(0, 0));
        }
    }
}