using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Battleships.Objects.UI
{
    /// <summary>
    /// Slider class for user interface.
    /// </summary>
    public class Slider : IObject
    {
        public Vector2                 Position  { get => Rectangle.CollisionRectangle.Location.ToVector2(); }
        public RotatedRectangle        Rectangle { get; private set; }
        public float                   Layer     { get; set; }
                                   
        public event EventHandler      OnDestroy;
        
        private Vector2                sliderBounds;
        private float                  value;
        private Texture2D              buttonTexture;
        private bool                   touching;

        private bool                   started;
        private float                  startPositionX;

        private readonly string        title;
        private readonly Texture2D     texture;
        private readonly Action<float> setValue;
        private readonly Func<Vector2> getCameraScale;

        public Slider(IGame1 game, Point position, Point size, Action<float> setValue, Vector2 sliderBounds, float startValue, string title, Func<Vector2> getCameraScale)
        {
            Rectangle           = new RotatedRectangle(new Rectangle(position, size), 0);
            this.setValue       = setValue;
            this.sliderBounds   = sliderBounds;
            value               = startValue;

            this.title          = title;
            texture             = TextureLibrary.GetTexture("HealthbarBorder");
            buttonTexture       = TextureLibrary.GetTexture("SliderButton");
            this.getCameraScale = getCameraScale;
        }

        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset                 = texture.Bounds.Size.ToVector2() / 2;
            spriteBatch.Draw(texture, Rectangle.CollisionRectangle, null, Color.White, 0, offset, SpriteEffects.None, Layer);

            float scale                    = 0.1f;
            SpriteFont font                = FontLibrary.GetFont("fixedsys");
            string str                     = $"{title}: {value.ToString()}";
            offset                         = Vector2.UnitY * -20f;

            spriteBatch.DrawString(font, str, Position + offset, Color.White, 0, font.MeasureString(str) * 0.5f, scale, SpriteEffects.None, 0);

            Color color                    = Color.White;
            Vector2 buttonTextureBoundSize = buttonTexture.Bounds.Size.ToVector2();

            if (touching)
            {
                color = Color.Gray;
            }

            spriteBatch.Draw(buttonTexture, GetButtonPosition(), null, color, 0, buttonTextureBoundSize * 0.5f, (Vector2.One * Rectangle.CollisionRectangle.Size.Y) / buttonTextureBoundSize, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Calculates the position of the button.
        /// </summary>
        /// <returns>The position of the button.</returns>
        private Vector2 GetButtonPosition()
        {
            float part = (value - sliderBounds.X) * 2 / (sliderBounds.Y - sliderBounds.X);
            part      -= 1.0f;

            Vector2 buttonPosition = Position + Vector2.UnitX * texture.Bounds.Size.X * part;

            return buttonPosition;
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime)
        {
            Vector2 buttonTextureBoundSize = buttonTexture.Bounds.Size.ToVector2();
            Vector2 buttonPosition         = GetButtonPosition();

            Vector2 cameraScale            = getCameraScale();
            MouseState mouseState          = Mouse.GetState();

            // Calculates if the mouse cursor is touching the button.
            touching                       = (mouseState.Position.X < (buttonPosition.X + buttonTextureBoundSize.X * 0.5f) * cameraScale.X &&
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

            setValue(value);
        }
    }
}
