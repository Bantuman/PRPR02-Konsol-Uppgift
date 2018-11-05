using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Libraries
{
    public static class MathLibrary
    {
        public static float Direction(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 ConstructVector(float angle, float length = 1)
        {
            return (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length);
        }
    }
}