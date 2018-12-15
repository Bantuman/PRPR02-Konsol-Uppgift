﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    public class MissilePickup : Pickup
    {
        public MissilePickup(Vector2 position, float lifetime, IGame1 game) : base(position, lifetime, game, TextureLibrary.GetTexture("Missile"))
        {}

        public override void PickUp(ref Ship obj)
        {
            obj.GiveMissiles(1);
            Destroy();
        }
    }
}