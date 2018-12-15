using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Objects
{
    /// <summary>
    /// Rotated rectangle class.
    /// </summary>
    public class RotatedRectangle
    {
        public Rectangle CollisionRectangle { get => collisionRectangle; set => collisionRectangle = value; }
        public float     Rotation           { get; set; }
        public Vector2   Origin             { get; set; }

        public int       X                  { get => CollisionRectangle.X; }
        public int       Y                  { get => CollisionRectangle.Y; }
        public int       Width              { get => CollisionRectangle.Width; }
        public int       Height             { get => CollisionRectangle.Height; }

        private Rectangle collisionRectangle;

        public RotatedRectangle(Rectangle theRectangle, float theInitialRotation)
        {
            CollisionRectangle = theRectangle;
            Rotation           = theInitialRotation;

            Origin             = new Vector2(theRectangle.Width / 2, theRectangle.Height / 2);
        }

        /// <summary>
        /// Changes the position of the rectangle.
        /// </summary>
        /// <param name="xPositionAdjustment">Adjustment i x position.</param>
        /// <param name="yPositionAdjustment">Adjustment i y position.</param>
        public void ChangePosition(int xPositionAdjustment, int yPositionAdjustment)
        {
            collisionRectangle.X += xPositionAdjustment;
            collisionRectangle.Y += yPositionAdjustment;
        }

        /// <summary>
        /// Checks if the rectangle intersects another rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>If the rectangles intersect.</returns>
        public bool Intersects(Rectangle rectangle)
        {
            return Intersects(new RotatedRectangle(rectangle, 0.0f));
        }
        /// <summary>
        /// Checks if the rectangle intersects another rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>If the rectangles intersect.</returns>
        public bool Intersects(RotatedRectangle rectangle)
        {
            // Calculate the axis.
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(UpperRightCorner() - UpperLeftCorner());
            aRectangleAxis.Add(UpperRightCorner() - LowerRightCorner());
            aRectangleAxis.Add(rectangle.UpperLeftCorner() - rectangle.LowerLeftCorner());
            aRectangleAxis.Add(rectangle.UpperLeftCorner() - rectangle.UpperRightCorner());

            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisColliding(rectangle, aAxis))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks axis collision.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="axis">The axis.</param>
        /// <returns>If the axis is colliding.</returns>
        private bool IsAxisColliding(RotatedRectangle rectangle, Vector2 axis)
        {
            // Projects the corners of the rectangle on to the axis.
            List<int> rectangleAScalars = new List<int>();
            rectangleAScalars.Add(GenerateScalar(rectangle.UpperLeftCorner(), axis));
            rectangleAScalars.Add(GenerateScalar(rectangle.UpperRightCorner(), axis));
            rectangleAScalars.Add(GenerateScalar(rectangle.LowerLeftCorner(), axis));
            rectangleAScalars.Add(GenerateScalar(rectangle.LowerRightCorner(), axis));

            // Projects the corners of the current Rectangle on to the axis.
            List<int> rectangleBScalars = new List<int>();
            rectangleBScalars.Add(GenerateScalar(UpperLeftCorner(), axis));
            rectangleBScalars.Add(GenerateScalar(UpperRightCorner(), axis));
            rectangleBScalars.Add(GenerateScalar(LowerLeftCorner(), axis));
            rectangleBScalars.Add(GenerateScalar(LowerRightCorner(), axis));

            // Gets the maximum and minium scalar values for each of the rectangles.
            int rectangleAMinimum = rectangleAScalars.Min();
            int rectangleAMaximum = rectangleAScalars.Max();
            int rectangleBMinimum = rectangleBScalars.Min();
            int rectangleBMaximum = rectangleBScalars.Max();

            if (rectangleBMinimum <= rectangleAMaximum && rectangleBMaximum >= rectangleAMaximum)
            {
                return true;
            }
            else if (rectangleAMinimum <= rectangleBMaximum && rectangleAMaximum >= rectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates scalar.
        /// </summary>
        /// <param name="rectangleCorner">Rectangle corner.</param>
        /// <param name="axis">Axis.</param>
        /// <returns>Scalar.</returns>
        private int GenerateScalar(Vector2 rectangleCorner, Vector2 axis)
        {
            float numerator         = (rectangleCorner.X * axis.X) + (rectangleCorner.Y * axis.Y);
            float denominator       = (axis.X * axis.X) + (axis.Y * axis.Y);
            float divisionResult    = numerator / denominator;
            Vector2 cornerProjected = new Vector2(divisionResult * axis.X, divisionResult * axis.Y);

            float scalar = (axis.X * cornerProjected.X) + (axis.Y * cornerProjected.Y);
            return (int)scalar;
        }

        /// <summary>
        /// Rotates a point around an origin.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>Rotated point.</returns>
        private static Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(origin.X + (point.X - origin.X) * Math.Cos(rotation)
                - (point.Y - origin.Y) * Math.Sin(rotation));
            aTranslatedPoint.Y = (float)(origin.Y + (point.Y - origin.Y) * Math.Cos(rotation)
                + (point.X - origin.X) * Math.Sin(rotation));

            return aTranslatedPoint;
        }

        /// <summary>
        /// Gets the upper left corner.
        /// </summary>
        /// <returns>Upper left corner.</returns>
        public Vector2 UpperLeftCorner()
        {
            Vector2 aUpperLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Top);
            aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + Origin, Rotation);
            return aUpperLeft;
        }

        /// <summary>
        /// Gets the upper right corner.
        /// </summary>
        /// <returns>Upper right corner.</returns>
        public Vector2 UpperRightCorner()
        {
            Vector2 aUpperRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Top);
            aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-Origin.X, Origin.Y), Rotation);
            return aUpperRight;
        }

        /// <summary>
        /// Gets the lower left corner.
        /// </summary>
        /// <returns>Lower left corner.</returns>
        public Vector2 LowerLeftCorner()
        {
            Vector2 aLowerLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Bottom);
            aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(Origin.X, -Origin.Y), Rotation);
            return aLowerLeft;
        }

        /// <summary>
        /// Gets the lowr right corner.
        /// </summary>
        /// <returns>Lower right corner.</returns>
        public Vector2 LowerRightCorner()
        {
            Vector2 aLowerRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Bottom);
            aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-Origin.X, -Origin.Y), Rotation);
            return aLowerRight;
        }
    }
}