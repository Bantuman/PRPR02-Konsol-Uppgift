using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    abstract class Pickup : Object
    {
        public Pickup(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public abstract void PickUp(ref IObject obj);

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}