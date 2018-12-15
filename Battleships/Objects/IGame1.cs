using Microsoft.Xna.Framework;

namespace Battleships
{
    /// <summary>
    /// Main game interface.
    /// </summary>
    public interface IGame1
    {
        Vector2 BaseDimensions { get; }

        /// <summary>
        /// Shakes camera.
        /// </summary>
        /// <param name="amount">Amount to shake with.</param>
        void ShakeCamera(float amount);

        /// <summary>
        /// Destroys object.
        /// </summary>
        /// <param name="obj">Object to destroy.</param>
        void Destroy(Objects.IObject obj);

        /// <summary>
        /// Instantiates object.
        /// </summary>
        /// <param name="obj">Object to instantiate.</param>
        /// <returns>The instantiated object.</returns>
        Objects.IObject Instantiate(Objects.IObject obj);
    }
}
