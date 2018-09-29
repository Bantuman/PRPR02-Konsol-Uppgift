using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    internal interface IObject
    {
        event EventHandler OnDestroy;

        int Rectangle { get; set; }
    }
}