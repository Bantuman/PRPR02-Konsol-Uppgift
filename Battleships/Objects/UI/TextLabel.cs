using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Libraries;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Battleships.Objects;

namespace Objects.UI
{
    public class TextLabel : IObject
    {
        public string Text { get; private set; }
        public Vector2 Position { get; set; }
        public RotatedRectangle Rectangle { get; set; }
        public float Layer { get; set; }
        public event EventHandler OnDestroy;

        private SpriteFont font;
        private float fontSize;

        public TextLabel(string text, Vector2 position, float size, string font = "fixedsys")
        {
            this.font = FontLibrary.GetFont(font);
            Position = position;
            this.fontSize = size;
            Text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, position: Position, Color.White, 0, Vector2.Zero, fontSize, SpriteEffects.None, 1f);
        }

        public void Update(GameTime gameTime) { }
    }
}
