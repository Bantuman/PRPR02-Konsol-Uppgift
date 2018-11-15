using Battleships;
using Battleships.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects.UI
{
    public class HealthBar : Object
    {
        private Ship ship;
        public Texture2D HealthTexture;

        public HealthBar(IGame1 game, Ship ship, Point position) : base(game, null, new RotatedRectangle(new Rectangle(position, new Point(10, 10)))
        {
            this.ship = ship;
            Rectangle = new RotatedRectangle(new Rectangle(0, 0, )
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Vector2 offset = HealthTexture.Bounds.Size.ToVector2() / 2;
            Rectangle rectangle = Rectangle.CollisionRectangle;
            rectangle.Width = (int)(rectangle.Width * (ship.Health / ship.MaxHealth));
            spriteBatch.Draw(HealthTexture, rectangle, null, Color.White, Rotation, Offset, SpriteEffects.None, Layer);
            
        }
    }
}
