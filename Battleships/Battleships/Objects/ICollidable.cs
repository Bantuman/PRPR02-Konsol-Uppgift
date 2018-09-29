using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    internal interface ICollidable
    {
        event EventHandler OnCollisionEnter;
        event EventHandler OnCollisionExit;
        event EventHandler OnCollisionStay;

        int ColliderType { get; set; }
    }
}