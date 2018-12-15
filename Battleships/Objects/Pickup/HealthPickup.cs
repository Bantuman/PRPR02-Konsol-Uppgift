using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    public class HealthPickup : Pickup
    {
        public HealthPickup(Vector2 position, float lifetime, IGame1 game) : base(position, lifetime, game, TextureLibrary.GetTexture("HealthPickup"))
        {
        }

        public override void PickUp(ref Ship obj)
        {
            obj.GiveHealth(5);
            Destroy();
        }
    }
}