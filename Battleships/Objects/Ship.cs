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
        public new Collider Collider { get; set; }
        public Animator Animator     { get; set; }
        public float Health          { get; private set; }
        public float MaxHealth       { get; private set; }
        public string Name           { get; private set; }
        public float MaxEnergy       { get; private set; }
        public int ShotsFired        { get; internal set; }
        public int ShotsHit          { get; internal set; }
        public float EnergySpent     { get; private set; }

        private float energy;
        public float Energy
        {
            get => energy;
            private set
            {
                if (value < energy)
                {
                    EnergySpent -= energy - value;
                }
                EnergySpent += value; energy = value;
            }
        }

        private Turret[] turrets;
        private bool initialized;
        private const int turretCount = 6;

        internal float Damage { get => 10; }

        public Ship(IGame1 game, Vector2 position) : base(game, TextureLibrary.GetTexture("Ship"))
        {
            Point size                 = new Point(32, 16);
            Rectangle                  = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider                   = new Collider(this, ColliderType.Static);
            Position                   = position;
            Animator                   = new Animator(new Animation.Animation(Texture, new Point(64, 32), new Point(3, 1), 5f));
            initialized                = false;
            MaxHealth                  = Health = 30;
            MaxEnergy                  = Energy = 666;
            OnDestroy                 += OnDeath;
            Name                       = GetType().Name;
            Collider.OnCollisionEnter += OnCollision;

            Energy = 100;
        }

        private void OnDeath(object sender, EventArgs e)
        {
            Game.Instantiate(new Explosion(Game, Position, 4, 1));
        }

        private void OnCollision(object sender, Collider.CollisionHitInfo e)
        {
            if (e.Object is Ship ship)
            {
                float collisionVelocity = ship.Velocity.Length();
                if (collisionVelocity > 70)
                {
                    TakeDamage(10 * (collisionVelocity / 70));
                    ship.TakeDamage(10 * (collisionVelocity / 70));
                    Game.Instantiate(new Explosion(Game, Vector2.Lerp(Position, e.Object.Position, 0.5f), 2 * (collisionVelocity / 70), 3));
                }
            }
        }
        
        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
            foreach(Turret turret in turrets)
            {
                turret.Update(gameTime);
            }
            if (Health <= 0)
            {
                Destroy();
            }
            RecoverEnergy(10f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void RecoverEnergy(float energy)
        {
            energy = Math.Abs(energy);
            Energy += energy;
            if (Energy > MaxEnergy)
            {
                Energy = MaxEnergy;
            }
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawTurrets(spriteBatch);
            DrawName(spriteBatch);
        }

        private void DrawName(SpriteBatch spriteBatch)
        {
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            Vector2 origin = font.MeasureString(Name) * 0.5f + Vector2.UnitY * Rectangle.Height * 8;
            spriteBatch.DrawString(font, Name, Position, Color.White, 0, origin, 0.18f, SpriteEffects.None, 1f);
        }

        private void DrawTurrets(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < turrets.Length; ++i)
            {
                Vector2 originalPosition = turrets[i].RelativePosition;
                turrets[i].FacingLeft = (i > (turrets.Length - 2) / 2);

                float rotation = Rotation + (turrets[i].FacingLeft ? (float)Math.PI : 0);
                float cosTetha = (float)Math.Cos(rotation);
                float sinTetha = (float)Math.Sin(rotation);

                turrets[i].RotatedPosition = new Vector2(originalPosition.X * cosTetha - originalPosition.Y * sinTetha, originalPosition.X * sinTetha + originalPosition.Y * cosTetha);

                Vector2 offset = turrets[i].Texture.Bounds.Size.ToVector2() / 2;

                spriteBatch.Draw(turrets[i].Texture, new Rectangle(Position.ToPoint() + turrets[i].RotatedPosition.ToPoint(), turrets[i].Size.ToPoint()), null, Color.White, rotation, offset, SpriteEffects.None, Layer + 0.01f);
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
            float shipScale = (Rectangle.CollisionRectangle.Size.X / 64f);
            for (int i = 0; i < turrets.Length; ++i)
            {
                Vector2 position = new Vector2(-Rectangle.CollisionRectangle.Size.X / 2 + 28 * shipScale * (i % (turrets.Length / 2)), 20 * shipScale);
                if (i > (turrets.Length - 1) / 2)
                {
                    position.X *= -1;
                }
                turrets[i] = new Turret(Game, this, position, new Vector2(10, 25) * shipScale);
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
            Health -= Math.Abs(damage);
            if (Health < 0)
            {
                Health = 0;
            }
        }
    }
}