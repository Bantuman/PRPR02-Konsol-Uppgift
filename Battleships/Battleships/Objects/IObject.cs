using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    interface IObject
    {
        Vector2 Position { get; set; }
        Rectangle Rectangle { get; }

        event EventHandler OnDestroy;

        void Update();
        void Draw();
    }
}