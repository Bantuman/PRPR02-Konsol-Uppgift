using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Animation
{
    /// <summary>
    /// Animator class that animates object using animations.
    /// </summary>
    public class Animator
    {
        public Animation Animation { get; set; }

        public Rectangle SourceRectangle => Animation.SourceRectangle;
        public Texture2D Texture         => Animation.SpriteSheet;
        
        public Animator(Animation animation)
        {
            Animation = animation;
        }

        /// <summary>
        /// Updates the current animation.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }
    }
}