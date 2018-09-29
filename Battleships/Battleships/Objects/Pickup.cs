using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    abstract class Pickup : IObject, ICollidable
    {
        public event EventHandler OnPickedUp;
    }
}