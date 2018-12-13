using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships;
using Battleships.Libraries;
using Battleships.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.UI
{
    public class Slider : Object
    {
        private Action<float> setValue;
        private Vector2 sliderBounds;
        private float value;
        private string title;

        public Slider(IGame1 game, Vector2 position, Point size, Action<float> setValue, Vector2 sliderBounds, float startValue, string title) : base(game, TextureLibrary.GetTexture("HealthbarBorder"), new RotatedRectangle(new Rectangle(Point.Zero, size), 0))
        {
            Position = position;
            this.setValue = setValue;
            this.sliderBounds = sliderBounds;
            value = startValue;
            this.title = title;
        }

        public override float Rotation => base.Rotation;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            float scale = 0.1f;
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            string str = $"{title}: {value.ToString()}";
            spriteBatch.DrawString(font, str, Position, Color.White, 0, font.MeasureString(str) * scale * 0.5f, scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            setValue(value);
        }
    }
}
