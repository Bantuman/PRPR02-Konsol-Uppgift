using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Animation
{
    /// <summary>
    /// Animation class using sprite sheets.
    /// </summary>
    public class Animation
    {
        public Texture2D       SpriteSheet     { get; }
        public Rectangle       SourceRectangle { get; set; }
        public Point           SpriteCount     { get; private set; }
                               
        private Point          spriteSize;
        private float          elapsedTime;
        private readonly float timeBetweenFrames;

        public Animation(Texture2D spriteSheet, Point spriteSize, Point spriteCount, float timeBetweenFrames)
        {
            SpriteSheet            = spriteSheet;
            this.spriteSize        = spriteSize;
            SpriteCount            = spriteCount;
            this.timeBetweenFrames = timeBetweenFrames;
        }

        /// <summary>
        /// Updates animation.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public void Update(GameTime gameTime)
        {
            if (SpriteCount.X * SpriteCount.Y == 1)
            {
                SourceRectangle = new Rectangle(0, 0, spriteSize.X, spriteSize.Y);
                return;
            }

            int targetSpriteIndex     = GetTargetSpriteIndex(gameTime);

            Point targetImage         = GetTargetSprite(targetSpriteIndex);
            Point targetImagePosition = new Point(targetImage.X * spriteSize.X, targetImage.Y * spriteSize.Y);

            Rectangle targetRectangle = new Rectangle(targetImagePosition.X, targetImagePosition.Y, spriteSize.X, spriteSize.Y);
            SourceRectangle           = targetRectangle;
        }

        /// <summary>
        /// Gets in index for the target sprite in the sprite sheet.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        /// <returns>Target sprite index.</returns>
        private int GetTargetSpriteIndex(GameTime gameTime)
        {
            int targetSpriteIndex = 0;
            do
            {
                targetSpriteIndex = (int)(elapsedTime / (timeBetweenFrames <= 0 ? 1 : timeBetweenFrames));
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (targetSpriteIndex >= SpriteCount.X * SpriteCount.Y)
                {
                    elapsedTime = 0;
                }
            } while (targetSpriteIndex >= SpriteCount.X * SpriteCount.Y);

            return targetSpriteIndex;
        }

        /// <summary>
        /// Gets the target sprite position using the target index.
        /// </summary>
        /// <param name="targetSpriteIndex">Target sprite index.</param>
        /// <returns>Position in sprite sheet where the target sprite is located.</returns>
        private Point GetTargetSprite(int targetSpriteIndex)
        {
	        return new Point(targetSpriteIndex % SpriteCount.X, (int)(targetSpriteIndex / SpriteCount.X));
        }
    }
}