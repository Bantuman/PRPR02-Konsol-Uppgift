using Microsoft.Xna.Framework;
using System;

namespace Battleships.Objects
{
    /// <summary>
    /// Camera class.
    /// </summary>
    public class Camera
    {
        public float    ShakeMagnitude { get; set; }
        public Vector2  Zoom           { get => zoom; set => zoom = new Vector2(MathHelper.Clamp(value.X, 0.1f, 2), MathHelper.Clamp(value.Y, 0.1f, 2)); }
        public float    Rotation       { get; private set; }
        public float    ShakeIntensity { get; set; }

        public Vector2  ShakeOffset    { get; set; }
        public Vector2  Position       { get; private set; }
        public int      ViewportWidth  { get; set; }
        public int      ViewportHeight { get; set; }

        public Vector2  ViewportCenter    => new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
        public Matrix   TranslationMatrix => Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                                             Matrix.CreateRotationZ(Rotation) *
                                             Matrix.CreateScale(new Vector3(Zoom, 1)) *
                                             Matrix.CreateTranslation(new Vector3(ViewportCenter, 0)) *
                                             Matrix.CreateTranslation(new Vector3(ShakeOffset.X, ShakeOffset.Y, 0));

        private Random  random;
        private Vector2 zoom;
        
        public Camera(int viewportWidth, int viewportHeight)
        {
            ShakeMagnitude = 4;
            ShakeOffset    = new Vector2(0, 0);
            random         = new Random();
            zoom           = Vector2.One;

            UpdateViewport(viewportWidth, viewportHeight);
        }

        /// <summary>
        /// Updates viewport.
        /// </summary>
        /// <param name="viewportWidth">New width.</param>
        /// <param name="viewportHeight">New height.</param>
        public void UpdateViewport(int viewportWidth, int viewportHeight)
        {
            ViewportHeight = viewportHeight;
            ViewportWidth  = viewportWidth;
        }

        /// <summary>
        /// Updates camera.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime)
        {
            float randomX = (float)(random.NextDouble() * 2 - 1);
            float randomY = (float)(random.NextDouble() * 2 - 1);
            ShakeOffset = new Vector2(randomX * ShakeMagnitude, randomY * ShakeMagnitude) * ShakeIntensity;
        }
    }
}