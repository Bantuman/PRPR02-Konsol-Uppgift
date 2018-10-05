using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    interface IObject
    {
        Vector2 Position     { get; set; }
        Rectangle Rectangle  { get; }
        float Layer          { get; set; }

        event EventHandler OnDestroy;

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}