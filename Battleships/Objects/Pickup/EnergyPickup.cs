﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    class EnergyPickup : Pickup
    {
        public EnergyPickup(IGame1 game, Texture2D texture) : base(game, texture)
        {
        }

        public override void PickUp(ref IObject obj)
        {

        }
    }
}