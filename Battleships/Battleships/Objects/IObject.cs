using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    interface IObject
    {
        event EventHandler OnDestroy;

        Rectangle Rectangle { get; set; }

        void Update();
        void Draw();
    }
}