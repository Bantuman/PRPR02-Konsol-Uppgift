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

        private readonly float lifetime;
        private float spawnTick;

        public Explosion(IGame1 game, Vector2 position, float scale, float duration) : base(game, TextureLibrary.GetTexture("Explosion"))
        {
            Point size = new Point((int)(32 * scale), (int)(32 * scale));
            Rectangle = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Position = position;
            Animator = new Animator(new Animation.Animation(Texture, new Point(128, 128), new Point(4, 4), duration/16));
            spawnTick = 0;
            lifetime = duration;
            game.ShakeCamera(0.8f);
        }

        public sealed override void Update(GameTime gameTime)
        {
            spawnTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTick >= lifetime)
            {
                Destroy();
            }
            base.Update(gameTime);
        }
    }
}