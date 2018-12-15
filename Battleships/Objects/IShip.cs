using Microsoft.Xna.Framework;

namespace Battleships.Objects
{
    /// <summary>
    /// Ship inteface.
    /// </summary>
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
