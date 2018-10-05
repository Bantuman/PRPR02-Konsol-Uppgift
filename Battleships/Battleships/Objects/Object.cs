using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Object : IObject
    {
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
        
        private Vector2 position;
        private Rectangle rectangle;
        private Vector2 acceleration;
        private Vector2 velocity;

        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void Update(GameTime gameTime)
        {
            ApplyAcceleration(gameTime);
            ApplyVelocity(gameTime);
        }

        protected virtual void SetAcceleration(Vector2 value)
        {
            acceleration = value;
        }

        protected void ApplyAcceleration(GameTime gameTime)
        {
            velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void ApplyVelocity(GameTime gameTime)
        {
            Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}