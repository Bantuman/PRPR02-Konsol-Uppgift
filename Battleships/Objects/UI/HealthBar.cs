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
    public class HealthBar : Object
    {
        private Ship Ship { get; }
        private Texture2D healthTexture;
        private string shipName;

        public HealthBar(IGame1 game, Ship ship, Point size, Point position) : base(game, TextureLibrary.GetTexture("HealthbarBorder"), new RotatedRectangle(new Rectangle(position, new Point(10, 10)), 0))
        {
            Ship = ship;
            shipName = MathLibrary.ClampString(ship.Name, 12);
            Rectangle = new RotatedRectangle(new Rectangle(position, size), 0);
            healthTexture = TextureLibrary.GetTexture("HealthbarFill");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle.CollisionRectangle, null, Color.White, Rotation, Offset, SpriteEffects.None, Layer);

            Vector2 offset = Vector2.Zero;
            Rectangle rectangle = Rectangle.CollisionRectangle;
            rectangle.Width     = (int)(rectangle.Width * (Ship.Health / Ship.MaxHealth));

            spriteBatch.Draw(healthTexture, rectangle, null, Color.White, Rotation, offset, SpriteEffects.None, Layer + 0.01f);

            SpriteFont font = FontLibrary.GetFont("fixedsys");
            spriteBatch.DrawString(font, $"HEALTH: ({Math.Round(Ship.Health, MidpointRounding.AwayFromZero)}/{Math.Round(Ship.MaxHealth, MidpointRounding.AwayFromZero)})", rectangle.Location.ToVector2() + new Vector2(1f, 10f), Ship.NameColor, 0, Vector2.Zero, 0.11f, SpriteEffects.None, 1f);
        }
    }
}
