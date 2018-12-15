using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    /// <summary>
    /// Base class for pickup to be picked up by ship.
    /// </summary>
    public abstract class Pickup : Object, ICollidable
    {
        public new Collider Collider { get; set; }

        private float lifetime;
        private float elapsedLifetime = 0;
        public Pickup(Vector2 position, float lifetime, IGame1 game, Texture2D texture) : base(game, texture)
        {
            this.lifetime = lifetime;
            Point size = new Point(16, 16);
            Rectangle = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Position = position;
            Collider = new Collider(this, ColliderType.Trigger);
            Collider.OnCollisionEnter += OnCollision;
        }

        /// <summary>
        /// Handles collisions with this.
        /// If the colliding object is a ship it picks this up.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="collisionHitInfo">Hit info.</param>
        private void OnCollision(object sender, Collider.CollisionHitInfo collisionHitInfo)
        {
            if (collisionHitInfo.Object is Ship ship)
            {
                PickUp(ref ship);
            }
        }

        /// <summary>
        /// Picks up pickup.
        /// </summary>
        /// <param name="obj">Ship to pick up pickup.</param>
        public abstract void PickUp(ref Ship obj);

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            elapsedLifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedLifetime > lifetime)
            {
                Destroy();
            }
        }
    }
}