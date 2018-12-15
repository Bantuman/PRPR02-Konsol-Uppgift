using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battleships.Objects
{
    /// <summary>
    /// Base object interface.
    /// </summary>
    public interface IObject
    {
        Vector2            Position  { get; }
        RotatedRectangle   Rectangle { get; }
        float              Layer     { get; set; }

        event EventHandler OnDestroy;

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}