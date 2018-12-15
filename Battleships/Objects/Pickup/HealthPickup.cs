using Battleships.Libraries;
using Microsoft.Xna.Framework;

namespace Battleships.Objects.Pickup
{
    /// <summary>
    /// Pickup that gives ship health upon picking it up.
    /// </summary>
    public class HealthPickup : Pickup
    {
        public HealthPickup(Vector2 position, float lifetime, IGame1 game) :
            base(position, lifetime, game, TextureLibrary.GetTexture("HealthPickup"))
        {}

        /// <summary>
        /// Picks up pickup.
        /// </summary>
        /// <param name="obj">Ship to pick up pickup.</param>
        public override void PickUp(ref Ship obj)
        {
            obj.GiveHealth(50);
            Destroy();
        }
    }
}