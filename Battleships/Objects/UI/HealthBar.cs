using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battleships.Objects.UI
{
    /// <summary>
    /// Health bar class.
    /// </summary>
    public class HealthBar : IObject
    {
        public Vector2             Position  { get => Rectangle.CollisionRectangle.Location.ToVector2(); }
        public RotatedRectangle    Rectangle { get; private set; }
        public float               Layer     { get; set; }

        public event EventHandler  OnDestroy;

        private Ship               Ship      { get; }

        private readonly Texture2D texture;
        private readonly Texture2D healthTexture;
        private readonly string    shipName;

        public HealthBar(IGame1 game, Ship ship, Point size, Point position)
        {
            Ship          = ship;
            shipName      = MathLibrary.ClampString(ship.Name, 12);
            Rectangle     = new RotatedRectangle(new Rectangle(position, size), 0);
            texture       = TextureLibrary.GetTexture("HealthbarBorder");
            healthTexture = TextureLibrary.GetTexture("HealthbarFill");
        }

        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset      = Vector2.Zero;
            spriteBatch.Draw(texture, Rectangle.CollisionRectangle, null, Color.White, 0, offset, SpriteEffects.None, Layer);
            
            Rectangle rectangle = Rectangle.CollisionRectangle;
            rectangle.Width     = (int)(rectangle.Width * (Ship.Health / Ship.MaxHealth));

            spriteBatch.Draw(healthTexture, rectangle, null, Color.White, 0, offset, SpriteEffects.None, Layer + 0.01f);

            SpriteFont font     = FontLibrary.GetFont("fixedsys");
            spriteBatch.DrawString(font, $"HEALTH: ({Math.Round(Ship.Health, MidpointRounding.AwayFromZero)}/{Math.Round(Ship.MaxHealth, MidpointRounding.AwayFromZero)})", rectangle.Location.ToVector2() + new Vector2(1f, 10f), Color.White, 0, Vector2.Zero, 0.11f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime){}
    }
}
