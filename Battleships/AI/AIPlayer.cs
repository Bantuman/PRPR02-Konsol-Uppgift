﻿using Battleships.Libraries;
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
            //Acceleration = new Vector2(1.8f, 1f) * 100f * ((position.Y > 1) ? -1 : 1);
        }

        public override void Act()
        {
            rotation += .01f;
            Acceleration = MathLibrary.ConstructVector(rotation, 0.01f);

            Shoot(10);
        }
    }
}