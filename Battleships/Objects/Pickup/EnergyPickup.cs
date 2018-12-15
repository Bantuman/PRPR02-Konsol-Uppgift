using Battleships.Libraries;
using Microsoft.Xna.Framework;

namespace Battleships.Objects.Pickup
{
    /// <summary>
    /// Pickup that gives ship energy upon picking it up.
    /// </summary>
    public class EnergyPickup : Pickup
    {
        public EnergyPickup(Vector2 position, float lifetime, IGame1 game) :
            base(position, lifetime, game, TextureLibrary.GetTexture("EnergyPickup"))
        {}

        /// <summary>
        /// Picks up pickup.
        /// </summary>
        /// <param name="obj">Ship to pick up pickup.</param>
        public override void PickUp(ref Ship obj)
        {
            obj.GiveEnergy(100);
            Destroy();
        }
    }
}