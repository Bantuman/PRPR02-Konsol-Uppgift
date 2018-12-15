using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects.Projectile
{
    public interface IMissile
    {
        Vector2 Position { get; }

        void RotateTo(float rotation);
    }
}
