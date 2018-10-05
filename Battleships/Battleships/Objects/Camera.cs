using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battleships.Objects
{
    class Camera
    {
        public Vector2 Position { get; private set; }
        public float ShakeMagnitude { get; private set; } = 4;
        public float Zoom { get => zoom; set => zoom = MathHelper.Clamp(value, 0.1f, 2); }
        public float Rotation { get; private set; } = 0;
        public float ShakeIntensity { get; private set; } = 0;
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        private float zoom = 1;
        private Vector2 shakeOffset = new Vector2(0, 0);
        private Random randomNumberGenerator = new Random();

        public Camera(int viewportWidth, int viewportHeight)
        {
            ViewportHeight = viewportHeight;
            ViewportWidth = viewportWidth;
        }

        // Calculates the center of the screen
        public Vector2 ViewportCenter
        {
            get => new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
        }

        // Calculates the translation matrix
        public Matrix TranslationMatrix
        {
            get => Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
        }

        // Translates the camera by delta vector
        public void TranslateCamera(Vector2 delta) => Position += delta;

        // Increases or decreases zoom
        public void AdjustZoom(float amount) => Zoom += amount;

        // Converts from Word Coords to Screen Coord and vice versa
        public Vector2 WorldToScreen(Vector2 worldPosition) => Vector2.Transform(worldPosition, TranslationMatrix);
        public Vector2 ScreenToWorld(Vector2 screenPosition) => Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));

        // Updates camera shake
        public void Update(GameTime gameTime)
        {
            float randomX = (float)(randomNumberGenerator.NextDouble() * 2 - 1);
            float randomY = (float)(randomNumberGenerator.NextDouble() * 2 - 1);
            shakeOffset = new Vector2(randomX * ShakeMagnitude, randomY * ShakeMagnitude) * ShakeIntensity;
        }
    }
}