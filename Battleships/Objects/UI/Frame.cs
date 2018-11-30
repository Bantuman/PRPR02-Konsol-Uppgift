using Battleships;
using Battleships.Libraries;
using Battleships.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects.UI
{
    public class Frame : Object
    {
        private Color color;
        public Frame(IGame1 game, Color color, Point size, Point position) : base(game, TextureLibrary.GetTexture("pixel"))
        {
            this.color = color;
            Rectangle = new RotatedRectangle(new Rectangle(position, size), 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle.CollisionRectangle, null, color, Rotation, Offset, SpriteEffects.None, Layer);
        }
    }
}
