using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battleships.Objects.UI
{
    /// <summary>
    /// Text label for user interface.
    /// </summary>
    public class TextLabel : IObject
    {
        public Vector2              Position  { get; set; }
        public RotatedRectangle     Rectangle { get => null; }
        public float                Layer     { get; set; }
                                    
        public string               Text      { get; private set; }
        
        public event EventHandler   OnDestroy;

        private readonly SpriteFont font;
        private readonly float      fontSize;

        public TextLabel(string text, Vector2 position, float size, SpriteFont font = null)
        {
            this.font = font ?? FontLibrary.GetFont("fixedsys");
            Position  = position;
            fontSize  = size;
            Text      = text;
        }

        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, position: Position, Color.White, 0, Vector2.Zero, fontSize, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime) { }
    }
}
