using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    abstract class Ship : IObject
    {
        private Turret[] turrets;
        private float energy;
        private float health;
        private Texture2D texture;

        public Rectangle Rectangle { get; private set; }
        public Vector2 Position { get; set; }
        public float Layer { get => 1; set => throw new NotImplementedException(); }

        public event EventHandler OnDestroy;

        public Ship(Vector2 position)
        {
            Position = position;
            Point size = new Point(100, 100); // Arbitrary.
            Rectangle = new Rectangle(position.ToPoint(), size);
            texture = TextureLibrary.GetTexture("Ship");
        }

        public abstract void Act();

        public void Update(GameTime gameTime)
        {
            rotation += 0.1f;
            //throw new NotImplementedException();
        }

        float rotation = 0;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, null, Color.White, rotation, texture.Bounds.Size.ToVector2() / 2, SpriteEffects.None, Layer);
            //throw new NotImplementedException();
        }
    }
}