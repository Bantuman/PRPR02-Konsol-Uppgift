﻿using Battleships;
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
    public class EnergyBar : Object
    {
        private Ship Ship { get; }
        private Texture2D healthTexture;

        public EnergyBar(IGame1 game, Ship ship, Point size, Point position) : base(game, TextureLibrary.GetTexture("HealthBar"), new RotatedRectangle(new Rectangle(position, new Point(10, 10)), 0))
        {
            Ship = ship;
            Rectangle = new RotatedRectangle(new Rectangle(position, size), 0);
            healthTexture = TextureLibrary.GetTexture("Bullet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle.CollisionRectangle, null, Color.White, Rotation, Offset, SpriteEffects.None, Layer);

            Vector2 offset = Vector2.Zero;
            Rectangle rectangle = Rectangle.CollisionRectangle;
            rectangle.Width = (int)(rectangle.Width * (Ship.Energy / Ship.MaxEnergy));

            spriteBatch.Draw(healthTexture, rectangle, null, Color.LightBlue, Rotation, offset, SpriteEffects.None, Layer + 0.01f);

            SpriteFont font = FontLibrary.GetFont("Pixel");
            spriteBatch.DrawString(font, $"ENERGY: ({Math.Round(Ship.Energy, MidpointRounding.AwayFromZero)}/{Math.Round(Ship.MaxEnergy, MidpointRounding.AwayFromZero)})", rectangle.Location.ToVector2() + Vector2.UnitY * 4, Ship.NameColor, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 1f);
        }
    }
}
