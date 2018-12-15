using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects
{
    public interface IShip
    {
        Vector2 Position      { get; }
        Vector2 Velocity      { get; }
        float   MaxHealth     { get; }
        float   Health        { get; }
        float   MaxEnergy     { get; }
        float   Energy        { get; }
        float   Rotation      { get; }
        int     ShotsFired    { get; }
        int     ShotsHit      { get; }
        int     MissilesFired { get; }
        int     MissilesHit   { get; }
        int     MissileCount  { get; }
    }
}
