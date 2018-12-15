using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects.Projectile
{
    /// <summary>
    /// Base class for all projectiles.
    /// </summary>
    public abstract class Projectile : Object, ICollidable
    {
        public new Collider Collider    { get; set; }

        protected float     Damage      { get; set; }
        protected Ship      BulletOwner { get; }

        public Projectile(IGame1 game, float damage, Texture2D texture, Ship owner) :
            base(game, texture)
        {
            Damage = damage;
            Collider = new Collider(this, ColliderType.Trigger);
            BulletOwner = owner;
        }
    }
}