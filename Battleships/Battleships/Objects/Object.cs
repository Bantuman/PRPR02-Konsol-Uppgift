using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Battleships.Libraries;
using Battleships.Objects.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract partial class Object : IObject
    {
        public Object(Texture2D texture)
        {
            Texture = texture;
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
        protected Texture2D Texture { get; private set; }
        protected float Rotation => MathLibrary.Direction(Acceleration);

        private Vector2 position;
        private Rectangle rectangle;
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this is IAnimated animated)
            {
                Texture = animated.Animator.Texture;
            }
            spriteBatch.Draw(Texture, Rectangle, (this as IAnimated)?.Animator.SourceRectangle, Color.White, Rotation, Texture.Bounds.Size.ToVector2() / 2, SpriteEffects.None, Layer);
        }

        public virtual void Update(GameTime gameTime)
        {
            (this as ICollidable)?.Collider.Update();
            (this as IAnimated)?.Animator.Update(gameTime);
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