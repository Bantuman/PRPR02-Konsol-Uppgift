﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battleships.Objects.Pickup
{
    abstract class Pickup : IObject
    {
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Rectangle Rectangle => throw new NotImplementedException();

        public event EventHandler OnDestroy;

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public abstract void PickUp(ref IObject obj);

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}