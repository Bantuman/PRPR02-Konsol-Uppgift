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
using Microsoft.Xna.Framework.Input;

namespace Battleships.Objects.UI
{
    public class Slider : Object
    {
        private Action<float> setValue;
        private Vector2 sliderBounds;
        private float value;
        private string title;
        private Texture2D buttonTexture;
        private Func<Vector2> getCameraScale;
        private bool touching;

        public Slider(IGame1 game, Vector2 position, Point size, Action<float> setValue, Vector2 sliderBounds, float startValue, string title, Func<Vector2> getCameraScale) : base(game, TextureLibrary.GetTexture("HealthbarBorder"), new RotatedRectangle(new Rectangle(Point.Zero, size), 0))
        {
            Position = position;
            this.setValue = setValue;
            this.sliderBounds = sliderBounds;
            value = startValue;
            this.title = title;
            buttonTexture = TextureLibrary.GetTexture("SliderButton");
            this.getCameraScale = getCameraScale;
            touching = false;
        }

        public override float Rotation => base.Rotation;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            float scale = 0.1f;
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            string str = $"{title}: {value.ToString()}";
            Vector2 offset = Vector2.UnitY * -20f;

            spriteBatch.DrawString(font, str, Position + offset, Color.White, 0, font.MeasureString(str) * 0.5f, scale, SpriteEffects.None, 0);

            Color color = Color.White;
            MouseState mouseState = Mouse.GetState();
            Vector2 buttonTextureBoundSize = buttonTexture.Bounds.Size.ToVector2();

            if (touching)
            {
                color = Color.Gray;
            }

            spriteBatch.Draw(buttonTexture, GetButtonPosition(), null, color, 0, buttonTextureBoundSize * 0.5f, (Vector2.One * Rectangle.CollisionRectangle.Size.Y) / buttonTextureBoundSize, SpriteEffects.None, 0);
        }

        private Vector2 GetButtonPosition()
        {
            float part = (value - sliderBounds.X) * 2 / (sliderBounds.Y - sliderBounds.X);
            part -= 1.0f;

            Vector2 buttonPosition = Position + Vector2.UnitX * Texture.Bounds.Size.X * part;

            return buttonPosition;
        }

        private bool started = false;
        private float startPositionX = 0;

        public override void Update(GameTime gameTime)
        {
            Vector2 cameraScale = getCameraScale();
            Vector2 buttonTextureBoundSize = buttonTexture.Bounds.Size.ToVector2();
            Vector2 buttonPosition = GetButtonPosition();
            MouseState mouseState = Mouse.GetState();
            touching = (mouseState.Position.X < (buttonPosition.X + buttonTextureBoundSize.X * 0.5f) * cameraScale.X &&
                        mouseState.Position.X > (buttonPosition.X - buttonTextureBoundSize.X * 0.5f) * cameraScale.X &&
                        mouseState.Position.Y < (buttonPosition.Y + buttonTextureBoundSize.Y * 0.5f) * cameraScale.Y &&
                        mouseState.Position.Y > (buttonPosition.Y - buttonTextureBoundSize.Y * 0.5f) * cameraScale.Y);
            
            if (touching && !started && mouseState.LeftButton == ButtonState.Pressed)
            {
                started = true;
                startPositionX = mouseState.Position.X * cameraScale.X;
            }
            if (started && mouseState.LeftButton == ButtonState.Pressed)
            {
                float mousePositionX = mouseState.Position.X;
                float leftBound  = (Position.X - Rectangle.CollisionRectangle.Size.X * 0.5f) * cameraScale.X;
                float rightBound = (Position.X + Rectangle.CollisionRectangle.Size.X * 0.5f) * cameraScale.X;

                float part = (mousePositionX - leftBound) / (rightBound - leftBound);
                if (part > 1)
                {
                    part = 1;
                }
                if (part < 0)
                {
                    part = 0;
                }

                value = part * (sliderBounds.Y - sliderBounds.X) + sliderBounds.X;
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                started = false;
            }

            base.Update(gameTime);
            setValue(value);
        }
    }
}
