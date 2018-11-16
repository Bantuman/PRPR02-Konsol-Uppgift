using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects.Animation
{
    interface IAnimated
    {
        Animator Animator      { get; set; }
    }
}