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

        public static string ClampString(string str, int length)
        {
            return str.Substring(0, Math.Min(str.Length, length - 2)) + (str.Length > length ? ".." : "");
        }

        public static float LerpAngle(float a, float b, float t)
        {
            float dtheta = b - a;
            if (dtheta > (float)Math.PI)
                a += 2 * (float)Math.PI;
            else if (dtheta < -Math.PI) a -= 2 * (float)Math.PI;
                a += (b - a) * t;
            return a;
        }
    }
}