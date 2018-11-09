using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Battleships.Objects.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    public abstract class Ship : Object, ICollidable, IAnimated
    {
        public new Collider Collider            { get; set; }
        public Animator Animator                { get; set; }
        public Animation.Animation[] Animations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Turret[] turrets;
        private float energy;
        private float health;
        private bool initialized;
        private const int turretCount = 6;

        internal float Damage { get => 10; }

        public Ship(IGame1 game, Vector2 position) : base(game, TextureLibrary.GetTexture("Ship"))
        {
            Point size  = new Point(64, 32);
            Rectangle   = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider    = new Collider(this, ColliderType.Static);
            Position    = position;
            Animator    = new Animator(new Animation.Animation(Texture, new Point(64, 32), new Point(3, 1), 5f));
            initialized = false;
            health      = 1;
            OnDestroy += OnDeath;
        }

        private void OnDeath(object sender, EventArgs e)
        {
            Game.Instantiate(new Explosion(Game, Position, 4, 3));
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
            foreach(Turret turret in turrets)
            {
                turret.Update(gameTime);
            }
            if (health <= 0)
            {
                Destroy();
            }
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for (int i = 0; i < turrets.Length; ++i)
            {
                Vector2 originalPosition = turrets[i].RelativePosition;
                turrets[i].FacingLeft = (i > (turrets.Length - 2) / 2);

                float rotation = Rotation + (turrets[i].FacingLeft ? (float)Math.PI : 0);
                float cosTetha = (float)Math.Cos(rotation);
                float sinTetha = (float)Math.Sin(rotation);

                turrets[i].RotatedPosition = new Vector2(originalPosition.X * cosTetha - originalPosition.Y * sinTetha, originalPosition.X * sinTetha + originalPosition.Y * cosTetha);

                Vector2 offset = turrets[i].Texture.Bounds.Size.ToVector2() / 2;
                
                spriteBatch.Draw(turrets[i].Texture, new Rectangle(Position.ToPoint() + turrets[i].RotatedPosition.ToPoint(), turrets[i].Size.ToPoint()), null, Color.White, rotation, offset, SpriteEffects.None, 0);
            }
        }

        public void Initialize()
        {
            if (initialized)
            {
                throw new Exception("Ship has already been initialized.");
            }
            if (Game == null)
            {
                throw new Exception("Value of game has not been set.");
            }

            turrets = new Turret[turretCount];
            for(int i = 0; i < turrets.Length; ++i)
            {
                Vector2 position = new Vector2(-Rectangle.CollisionRectangle.Size.X / 2 + 28 * (i % (turrets.Length / 2)), 20);
                if (i > (turrets.Length - 1) / 2)
                {
                    position.X *= -1;
                }
                turrets[i] = new Turret(Game, this, position, new Vector2(10, 25));
            }

            initialized = true;
        }

        protected void Shoot(float duration)
        {
            foreach(Turret turret in turrets)
            {
                turret.Fire(duration);
            }
        }

        public void TakeDamage(float damage)
        {
            health -= Math.Abs(damage);
        }
    }
}