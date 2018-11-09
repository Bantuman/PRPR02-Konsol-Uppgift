using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public interface IGame1
    {
        void ShakeCamera(float amount);
        void Destroy(Objects.IObject obj);
        Objects.IObject Instantiate(Objects.IObject obj);
    }
}
