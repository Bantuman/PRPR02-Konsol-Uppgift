using Microsoft.Xna.Framework;

namespace Battleships.Objects.Projectile
{
    /// <summary>
    /// Missile interface.
    /// </summary>
    public interface IMissile
    {
        Vector2 Position { get; }

        /// <summary>
        /// Rotates missile to target rotation.
        /// </summary>
        /// <param name="rotation">Target rotation.</param>
        void RotateTo(float rotation);
    }
}
