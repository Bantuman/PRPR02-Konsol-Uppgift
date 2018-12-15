using Microsoft.Xna.Framework;
using System;

namespace Battleships.Libraries
{
    /// <summary>
    /// Library of math helper functions.
    /// </summary>
    public static class MathLibrary
    {
        /// <summary>
        /// Converts a vector to a floating point representing rotation.
        /// </summary>
        /// <param name="vector">Vector to get angle from.</param>
        /// <returns>Angle of vector.</returns>
        public static float Direction(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// Constructs a vector from an angle with a specified magnitude.
        /// </summary>
        /// <param name="angle">Angle of vector.</param>
        /// <param name="length">Magnitude of vector.</param>
        /// <returns>Vector with specified angle and magnitude.</returns>
        public static Vector2 ConstructVector(float angle, float length = 1)
        {
            return (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * length);
        }

        /// <summary>
        /// Clamps a string.
        /// </summary>
        /// <param name="str">String to clamp.</param>
        /// <param name="length">Length to clamp for.</param>
        /// <returns>Clamped string.</returns>
        public static string ClampString(string str, int length)
        {
            return str.Substring(0, Math.Min(str.Length, length - 2)) + (str.Length > length ? ".." : "");
        }

        /// <summary>
        /// Linearaly interpolates an angle.
        /// </summary>
        /// <param name="a">Angle to interpolate from.</param>
        /// <param name="b">Angle to interpolate to.</param>
        /// <param name="t">Amount to interpolate with.</param>
        /// <returns>Interpolated angle.</returns>
        public static float LerpAngle(float a, float b, float t)
        {
            float deltaTheta = b - a;
            if (deltaTheta > (float)Math.PI)
            {
                a += 2 * (float)Math.PI;
            }
            else if (deltaTheta < -Math.PI) a -= 2 * (float)Math.PI;
            {
                a += (b - a) * t;
            }
            return a;
        }
    }
}