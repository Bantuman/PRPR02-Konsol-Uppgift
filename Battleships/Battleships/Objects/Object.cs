using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Object : IObject
    {
        public Object(Texture2D texture)
        {
            this.texture = texture;
        }

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                rectangle.Location = position.ToPoint();
            }
        }
        public Rectangle Rectangle { get => rectangle; protected set => rectangle = value; }
        public float Layer         { get; set; }
        public event EventHandler OnDestroy;

        protected Vector2 Velocity     { get; private set; }
        protected Vector2 Acceleration { get; set; }

        private float Rotation => MathLibrary.Direction(Acceleration);
        private Vector2 position;
        private Rectangle rectangle;
        private Texture2D texture;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, null, Color.White, Rotation, texture.Bounds.Size.ToVector2() / 2, SpriteEffects.None, Layer);
        }

        public virtual void Update(GameTime gameTime)
        {
            ApplyAcceleration(gameTime);
            ApplyVelocity(gameTime);
        }

        protected void ApplyAcceleration(GameTime gameTime)
        {
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void ApplyVelocity(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}