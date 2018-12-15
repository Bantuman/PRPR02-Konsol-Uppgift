using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships.Libraries;
using Battleships.Objects.Animation;
using Battleships.Objects.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships.Objects
{
    public abstract class Ship : Object, ICollidable, IAnimated, IShip
    {
        public new Collider Collider              { get; }
        public Animator Animator                  { get; }
        public float Health                       { get; private set; }
        public float MaxHealth                    { get; private set; }
        public string Name                        { get; private set; }
        public float MaxEnergy                    { get; private set; }
        public int ShotsFired                     { get; internal set; }
        public int ShotsHit                       { get; internal set; }
        public int MissilesFired                  { get; private set; }
        public int MissilesHit                    { get; internal set; }
        public float EnergySpent                  { get; private set; }
        public int MissileCount                   { get; private set; }

        protected IShip EnemyShip                 { get; private set; }
        protected GameInformation GameInformation { get => getGameInformation(this); }
        protected int TurretCount                 { get => turrets.Length; }

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
        private List<Missile> missiles;
        private bool initialized;
        private const int turretCount = 6;
        private Action<GameTime, IMissile> guideMissile;
        private Func<Ship, GameInformation> getGameInformation;
        
        internal float Damage { get => 10; }

        public Ship(IGame1 game, Vector2 position) : base(game, TextureLibrary.GetTexture("Ship"))
        {
            Point size                 = new Point(32, 16);
            Rectangle                  = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider                   = new Collider(this, ColliderType.Static);
            Position                   = position;
            Animator                   = new Animator(new Animation.Animation(Texture, new Point(64, 32), new Point(3, 1), 5f));
            initialized                = false;
            MaxHealth                  = Health = 500;
            MaxEnergy                  = Energy = 600;
            OnDestroy                 += OnDeath;
            Name                       = GetType().Name;
            Collider.OnCollisionEnter += OnCollision;
            guideMissile               = null;
            missiles                   = new List<Missile>();
        }

        private protected sealed override void ApplyAcceleration(Vector2 accelerationAmount)
        {
            if(energy >= accelerationAmount.Length())
            {
                TakeEnergy(accelerationAmount.Length());
                base.ApplyAcceleration(accelerationAmount);
            }
        }

        protected void SetMissileGuideAI(Action<GameTime, IMissile> guideMissile)
        {
            this.guideMissile = guideMissile;
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

        protected void LaunchMissile(float rotation)
        {
            if (MissileCount <= 0)
            {
                return;
            }
            ++MissilesFired;
            --MissileCount;
            Missile missile = (Missile)Game.Instantiate(new Missile(Game, rotation, 0, Position, this, guideMissile));
            missiles.Add(missile);
            missile.OnDestroy += Missile_OnDestroy;
        }

        private void Missile_OnDestroy(object sender, EventArgs e)
        {
            missiles.Remove((Missile)sender);
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (Turret turret in turrets)
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

        public void Initialize(Ship enemyShip, Func<Ship, GameInformation> getGameInformation)
        {
            if (initialized)
            {
                throw new Exception("Ship has already been initialized.");
            }
            if (Game == null)
            {
                throw new Exception("Value of game has not been set.");
            }

            this.getGameInformation = getGameInformation;
            EnemyShip = enemyShip;

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

        internal void GiveMissiles(int amount)
        {
            MissileCount += amount;
        }

        internal void GiveHealth(float health)
        {
            Health = Math.Min(MaxHealth, Health + Math.Abs(health));
        }

        internal void GiveEnergy(float energy)
        {
            Energy = Math.Min(MaxEnergy, Energy + Math.Abs(energy));
        }

        public void TakeEnergy(float energy)
        {
            Energy -= Math.Abs(energy);
            if (Energy < 0)
            {
                Energy = 0;
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

        protected void SetShooting(bool value)
        {
            for(int i = 0; i < TurretCount; ++i)
            {
                turrets[i].IsFiring = value;
            }
        }
        protected void SetShooting(int turretIndex, bool value)
        {
            turrets[turretIndex].IsFiring = value;
        }
    }
}
