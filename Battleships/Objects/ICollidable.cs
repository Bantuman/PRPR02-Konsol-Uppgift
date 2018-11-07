using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    public interface ICollidable
    {
        Object.Collider Collider { get; set; }
    }
}