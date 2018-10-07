using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects.Animation
{
    class Animation
    {
        public Texture2D SpriteSheet     { get; }
        public Rectangle SourceRectangle { get; set; }

        private Point spriteSize;
        private Point spriteCount;
        private float timeBetweenFrames;
        private float elapsedTime;

        public Animation(Texture2D spriteSheet, Point spriteSize, Point spriteCount, float timeBetweenFrames)
        {
            SpriteSheet            = spriteSheet;
            this.spriteSize        = spriteSize;
            this.spriteCount       = spriteCount;
            this.timeBetweenFrames = timeBetweenFrames;
            elapsedTime            = 0;
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (spriteCount.X * spriteCount.Y == 1)
            {
                SourceRectangle = new Rectangle(0, 0, spriteSize.X, spriteSize.Y);
                return;
            }

            int targetSpriteIndex = GetTargetSpriteIndex(gameTime);

            Point targetImage = GetTargetSprite(targetSpriteIndex);
            Point targetImagePosition = new Point(targetImage.X * spriteSize.X, targetImage.Y * spriteSize.Y);

            Rectangle targetRectangle = new Rectangle(targetImagePosition.X, targetImagePosition.Y, spriteSize.X, spriteSize.Y);
            SourceRectangle = targetRectangle;
        }

        private int GetTargetSpriteIndex(GameTime gameTime)
        {
            int targetSpriteIndex = 0;
            do
            {
                targetSpriteIndex = (int)(elapsedTime / (timeBetweenFrames <= 0 ? 1 : timeBetweenFrames));
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (targetSpriteIndex >= spriteCount.X * spriteCount.Y)
                {
                    elapsedTime = 0;
                }
            } while (targetSpriteIndex >= spriteCount.X * spriteCount.Y);

            return targetSpriteIndex;
        }

        private Point GetTargetSprite(int targetImageIndex)
        {
	        return new Point(targetImageIndex % spriteCount.X, (int)(targetImageIndex / (spriteCount.X + 1)));
        }
    }
}