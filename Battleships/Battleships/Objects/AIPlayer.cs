﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
            
        }
    }
}