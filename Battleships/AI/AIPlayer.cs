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

        public AIPlayer(Vector2 position) : base(null, position)
        {}

        private void MoveToPoint(Vector2 targetPosition)
        {
            Acceleration = Vector2.Normalize(targetPosition - Position) * 40f;
        }
        
        private void AimTowards(Vector2 targetPosition, int shipSide = 1)
        {
            Acceleration = MathLibrary.ConstructVector(MathLibrary.Direction(targetPosition - Position) + (90 * shipSide));
        }

        public override void Act()
        {
            Shoot(1);
            MoveToPoint(new Vector2(-100, -100));
            //AimTowards(Vector2.One, 1);
        }
    }
}