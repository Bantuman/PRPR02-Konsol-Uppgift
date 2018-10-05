using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Ship : Object
    {
        private Turret[] turrets;
        private float energy;
        private float health;
        private Texture2D texture;

        public Ship(Vector2 position)
        {
            Position = position;
            Point size = new Point(70, 70);

            Rectangle = new Rectangle(position.ToPoint(), size);
            texture   = TextureLibrary.GetTexture("Ship");
        }

        protected sealed override void SetAcceleration(Vector2 value)
        {
            base.SetAcceleration(value);
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, null, Color.White, 0, texture.Bounds.Size.ToVector2() / 2, SpriteEffects.None, Layer);
            //throw new NotImplementedException();
        }
    }
}