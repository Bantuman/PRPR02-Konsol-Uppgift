using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    enum ColliderType
    {
        Trigger,
        Static
    }

    class Collider
    {
        private ColliderType ColliderType;

        public event EventHandler OnCollisionEnter;
        public event EventHandler OnCollisionExit;
        public event EventHandler OnCollisionStay;
    }
}