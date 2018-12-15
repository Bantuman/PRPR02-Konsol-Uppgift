using Battleships.Objects.Projectile;
using Microsoft.Xna.Framework;

namespace Battleships.Objects
{
    /// <summary>
    /// Base class for projectile information.
    /// </summary>
    public abstract class ProjectileInformation
    {
        public Vector2 Position      { get; }
        public Vector2 Velocity      { get; }

        public abstract float Damage { get; }

        public ProjectileInformation(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }

    /// <summary>
    /// Missile information.
    /// </summary>
    public class MissileInformation : ProjectileInformation
    {
        public float TimeLeft { get; }

        public override float Damage => Missile.DAMAGE;

        public MissileInformation(Vector2 position, Vector2 velocity, float timeAlive) :
            base(position, velocity)
        {
            TimeLeft = Missile.LIFETIME - timeAlive;
        }
    }

    /// <summary>
    /// Bullet information.
    /// </summary>
    public class BulletInformation : ProjectileInformation
    {
        public override float Damage => Bullet.DAMAGE;

        public BulletInformation(Vector2 position, Vector2 velocity) :
            base(position, velocity)
        {}
    }

    /// <summary>
    /// Ship inteface.
    /// </summary>
    public interface IShip
    {
        Vector2              Position      { get; }
        Vector2              Velocity      { get; }
        float                MaxHealth     { get; }
        float                Health        { get; }
                             
        float                MaxEnergy     { get; }
        float                Energy        { get; }
        float                Rotation      { get; }
        int                  ShotsFired    { get; }
                             
        int                  ShotsHit      { get; }
        int                  MissilesFired { get; }
        int                  MissilesHit   { get; }
        int                  MissileCount  { get; }

        MissileInformation[] Missiles      { get; }
        BulletInformation[]  Bullets       { get; }
    }
}
