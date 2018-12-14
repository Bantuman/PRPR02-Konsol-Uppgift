using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    class Missile : Projectile
    {
        // In radians.
        private float targetRotation;
        private float rotation;

        private float speed;

        public override float Rotation => rotation;

        public Missile(IGame1 game, Texture2D texture, float rotation) : base(game, 0, texture)
        {
            Rectangle = new RotatedRectangle(new Rectangle(0, 0, 100, 200), rotation);
            this.rotation = rotation;
        }

        public void RotateTo(float rotation)
        {
            targetRotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            rotation = MathLibrary.LerpAngle(rotation, targetRotation, 1 - (float)Math.Pow(0.2f, gameTime.ElapsedGameTime.TotalSeconds));
            Velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * speed;
            base.Update(gameTime);
        }
    }
}