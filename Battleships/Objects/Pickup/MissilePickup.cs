using Battleships.Libraries;
using Microsoft.Xna.Framework;

namespace Battleships.Objects.Pickup
{
    /// <summary>
    /// Pickup that gives ship a missile upon picking it up.
    /// </summary>
    public class MissilePickup : Pickup
    {
        public MissilePickup(Vector2 position, float lifetime, IGame1 game) :
            base(position, lifetime, game, TextureLibrary.GetTexture("Missile"))
        {}

        /// <summary>
        /// Picks up pickup.
        /// </summary>
        /// <param name="obj">Ship to pick up pickup.</param>
        public override void PickUp(ref Ship obj)
        {
            obj.GiveMissiles(1);
            Destroy();
        }
    }
}