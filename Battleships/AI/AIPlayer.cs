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

        public AIPlayer(Vector2 position, string name, Color nameColor) : base(null, position, name, nameColor)
        {
            //Acceleration = new Vector2(0, 1f) * 40f * ((position.Y > 1) ? -1 : 1);
        }
        
        public override void Act()
        {
            rotation = MathHelper.PiOver2;
            Acceleration = MathLibrary.ConstructVector(rotation, 0.0000001f);
            if(Name == "Nemo")
                Shoot(10);
        }
    }
}