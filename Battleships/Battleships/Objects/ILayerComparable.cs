using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects
{
    class LayerComparer : IComparer<IObject>
    {
        public int Compare(IObject x, IObject y)
        {
            return x.Layer > y.Layer ? 0 : 1;
        }
    }
}
