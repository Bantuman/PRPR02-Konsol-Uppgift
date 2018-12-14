using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Pickup
{
    public abstract class Pickup : Object, ICollidable
    {
        public new Collider Collider { get; set; }

        private float lifetime;
        private float elapsedLifetime = 0;
        public Pickup(Vector2 position, float lifetime, IGame1 game, Texture2D texture) : base(game, texture)
        {
            this.lifetime = lifetime;
            Point size = new Point(32, 32);
            Rectangle = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider = new Collider(this, ColliderType.Trigger);
            Collider.OnCollisionEnter += OnCollision;
        }

        private void OnCollision(object sender, Collider.CollisionHitInfo e)
        {
            if (e.Object is Ship ship)
            {
                PickUp(ref ship);
            }
        }

        public abstract void PickUp(ref Ship obj);

        public override void Update(GameTime gameTime)
        {
            elapsedLifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedLifetime > lifetime)
            {
                Destroy();
            }
        }
    }
}