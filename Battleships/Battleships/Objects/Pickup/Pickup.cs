using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects.Pickup
{
    abstract class Pickup : IObject
    {
        public abstract void PickUp(ref IObject obj);
    }
}