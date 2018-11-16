using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Objects;
using Battleships.Libraries;
using Battleships.Objects.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Battleships.Objects
{
    public class Explosion : Battleships.Objects.Object, Battleships.Objects.Animation.IAnimated
    {
        public Animator Animator { get; set; }
        public Animation.Animation[] Animations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Texture2D glowTexture;
        private readonly float lifetime;
        private float spawnTick;
        private float glowTransparency;

        public Explosion(IGame1 game, Vector2 position, float scale, float duration) : base(game, TextureLibrary.GetTexture("Explosion"))
        {
            Point size = new Point((int)(32 * scale), (int)(32 * scale));
            Rectangle = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            glowTexture = TextureLibrary.GetTexture("glow");
            Position = position;
            Animator = new Animator(new Animation.Animation(Texture, new Point(128, 128), new Point(4, 4), duration / 8));
            spawnTick = 0;
            glowTransparency = 0.8f;
            lifetime = duration;
            game.ShakeCamera(scale / 4);
        }

        public sealed override void Update(GameTime gameTime)
        {
            glowTransparency -= 0.05f;
            spawnTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTick >= lifetime)
            {
                Destroy();
            }
            base.Update(gameTime);
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(glowTexture, Rectangle.CollisionRectangle, null, new Color(244, 130, 66) * glowTransparency, 0, new Vector2(glowTexture.Width * 0.5f, glowTexture.Height * 0.5f), SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }
    }
}