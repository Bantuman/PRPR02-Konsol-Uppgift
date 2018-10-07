using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects.Animation
{
    class Animator
    {
        public Animation Animation { get; set; }
        public Rectangle SourceRectangle => Animation.SourceRectangle;
        public Texture2D Texture         => Animation.SpriteSheet;
        
        public Animator(Animation animation)
        {
            Animation = animation;
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }
    }
}