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
        public Vector2 Position
        {
            get => position;
            private protected set
            {
                position = value;
                rectangle.CollisionRectangle.Location = position.ToPoint();
            }
        }
        public RotatedRectangle Rectangle { get => rectangle; protected set => rectangle = value; }
        public float Layer                { get; set; }
        public Vector2 Offset             { get; private set; }
        public float RotationOffset       { get; set; }
        public IGame1 Game                { private protected get; set; }
        public float DistanceTraveled     { get; private set; }
        public float HighestVelocity      { get; private set; }
        public Texture2D Texture          { get; private set; }
        public Vector2 Velocity           { get; private protected set; }

        public virtual float Rotation => MathLibrary.Direction(Acceleration);
        public event EventHandler OnDestroy;
        
        protected Vector2 Acceleration { get; set; }

        private Vector2 position;
        private RotatedRectangle rectangle;

        public Object(IGame1 game, Texture2D texture, RotatedRectangle rectangle = null)
        {
            Game = game;
            Texture = texture;
            Rectangle = rectangle;
            DistanceTraveled = 0;
            HighestVelocity = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 Offset = Texture.Bounds.Size.ToVector2() / 2;
            if (this is IAnimated animated)
            {
                Texture = animated.Animator.Texture;
                Offset = (Texture.Bounds.Size.ToVector2() / animated?.Animator.Animation.SpriteCount.ToVector2() ?? Vector2.One) / 2;
            }
            spriteBatch.Draw(Texture, Rectangle.CollisionRectangle, (this as IAnimated)?.Animator.SourceRectangle, Color.White, Rotation, Offset, SpriteEffects.None, Layer);
        }

        public void Destroy()
        {
            Game.Destroy(this);
            OnDestroy?.Invoke(this, new EventArgs());
        }

        public virtual void Update(GameTime gameTime)
        {
            if(Velocity.Length() > HighestVelocity)
            {
                HighestVelocity = Velocity.Length();
            }
            ApplyAcceleration(Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
            ApplyVelocity(gameTime);
            rectangle.Rotation = Rotation; 
            (this as ICollidable)?.Collider.Update(gameTime);
            (this as IAnimated)?.Animator.Update(gameTime);
        }

        private protected virtual void ApplyAcceleration(Vector2 accelerationAmount)
        {
            Velocity += accelerationAmount;
        }

        private protected void ApplyVelocity(GameTime gameTime)
        { 
            Vector2 movement = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            DistanceTraveled += movement.Length();
            Position += movement;
        }
    }
}