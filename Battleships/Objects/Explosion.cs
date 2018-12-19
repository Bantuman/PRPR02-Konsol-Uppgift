using Battleships.Libraries;
using Battleships.Objects.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battleships.Objects
{
    /// <summary>
    /// Explosion class.
    /// </summary>
    public class Explosion : IObject, IAnimated
    {
        public Vector2             Position  { get => Rectangle.CollisionRectangle.Location.ToVector2(); }
        public RotatedRectangle    Rectangle { get; private set; }
        public float               Layer     { get; set; }
        public Animator            Animator  { get; set; }

        public event EventHandler  OnDestroy;
                                   
        private Texture2D          texture;
        private Rectangle          explosionRectangle;
        private Texture2D          glowTexture;
        private float              spawnTick;

        private float              glowTransparency;
        private IGame1             game;
        private readonly float     lifetime;

        public Explosion(IGame1 game, Vector2 position, float scale, float duration)
        {
            Point size         = new Point((int)(32 * scale), (int)(32 * scale));
                               
            Rectangle          = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            explosionRectangle = new Rectangle(position.ToPoint(), new Point((int)(size.X * 1.5f), (int)(size.Y * 1.5f)));
            texture            = TextureLibrary.GetTexture("Explosion");
            glowTexture        = TextureLibrary.GetTexture("glow");

            Animator           = new Animator(new Animation.Animation(texture, new Point(128, 128), new Point(4, 4), duration / 8));
            glowTransparency   = 0.7f;
            lifetime           = duration;
            this.game          = game;

            game.ShakeCamera(scale / 4);
        }

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime)
        {
            Animator.Update(gameTime);

            glowTransparency -= 0.05f;
            spawnTick        += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spawnTick >= lifetime)
            {
                game.Destroy(this);
            }
        }

        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(glowTexture, explosionRectangle, null, new Color(255, 140, 77) * glowTransparency, 0, new Vector2(glowTexture.Width * 0.5f, glowTexture.Height * 0.5f), SpriteEffects.None, 1);

            texture = Animator.Texture;
            Vector2 offset = (texture.Bounds.Size.ToVector2() / Animator.Animation.SpriteCount.ToVector2()) / 2;
            
            spriteBatch.Draw(texture, Rectangle.CollisionRectangle, (this as IAnimated)?.Animator.SourceRectangle, Color.White, 0, offset, SpriteEffects.None, Layer);
        }
    }
}