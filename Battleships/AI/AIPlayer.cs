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
        private float rotation = 0;

        public AIPlayer(Vector2 position, string name, Color nameColor) : base(null, position, name, nameColor)
        {}

        private void MoveToPoint(Vector2 targetPosition)
        {
            Acceleration = Vector2.Normalize(targetPosition - Position) * 40f;
        }

        public override void Act()
        {
            Shoot(10);
            rotation += 0.1f;
            Acceleration = MathLibrary.ConstructVector(rotation);
            //MoveToPoint(new Vector2(0, 100));
        }
    }
}