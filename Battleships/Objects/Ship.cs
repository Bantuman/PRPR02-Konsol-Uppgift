using Battleships.Libraries;
using Battleships.Objects.Animation;
using Battleships.Objects.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Battleships.Objects
{
    /// <summary>
    /// Base class for all ships.
    /// </summary>
    public abstract class Ship : Object, ICollidable, IAnimated, IShip
    {
        public new Collider                 Collider           { get; }
        public Animator                     Animator           { get; }
        public float                        Health             { get; private set; }
        public float                        MaxHealth          { get; private set; }

        public string                       Name               { get; private set; }
        public float                        MaxEnergy          { get; private set; }
        public int                          ShotsFired         { get; internal set; }
        public int                          ShotsHit           { get; internal set; }

        public int                          MissilesFired      { get; private set; }
        public int                          MissilesHit        { get; internal set; }
        public float                        EnergySpent        { get; private set; }
        public int                          MissileCount       { get; private set; }

        public MissileInformation[]         Missiles
        {
            get
            {
                MissileInformation[] missileInformation = new MissileInformation[missiles.Count];

                for(int i = 0; i < missiles.Count; ++i)
                {
                    missileInformation[i] = new MissileInformation(missiles[i].Position, missiles[i].Velocity, missiles[i].TimeAlive);
                }

                return missileInformation;
            }
        }
        public BulletInformation[]          Bullets
        {
            get
            {
                BulletInformation[] bulletInformation = new BulletInformation[missiles.Count];

                for (int i = 0; i < missiles.Count; ++i)
                {
                    bulletInformation[i] = new BulletInformation(Bullets[i].Position, Bullets[i].Velocity);
                }

                return bulletInformation;
            }
        }

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

        internal float Damage { get => 10; }

        protected IShip                     EnemyShip       { get; private set; }
        protected GameInformation           GameInformation { get => getGameInformation(this); }
        protected int                       TurretCount     { get => turrets.Length; }

        private float                       energy;
        private Turret[]                    turrets;
        private List<Missile>               missiles;
        private bool                        initialized;
        
        private Action<GameTime, IMissile>  guideMissile;
        private Func<Ship, GameInformation> getGameInformation;

        private const int TURRET_COUNT = 6;

        public Ship(IGame1 game, Vector2 position) :
            base(game, TextureLibrary.GetTexture("Ship"))
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

        /// <summary>
        /// Applies acceleration.
        /// </summary>
        /// <param name="accelerationAmount">Amount to accelerate with.</param>
        private protected sealed override void ApplyAcceleration(Vector2 accelerationAmount)
        {
            if(energy >= accelerationAmount.Length())
            {
                LoseEnergy(accelerationAmount.Length());
                base.ApplyAcceleration(accelerationAmount);
            }
        }

        /// <summary>
        /// Sets the AI to guide missiles.
        /// </summary>
        /// <param name="guideMissile">Function for guiding missiles.</param>
        protected void SetMissileGuideAI(Action<GameTime, IMissile> guideMissile)
        {
            this.guideMissile = guideMissile;
        }

        /// <summary>
        /// Instantiates an explosion on death.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnDeath(object sender, EventArgs e)
        {
            Game.Instantiate(new Explosion(Game, Position, 4, 1));
        }

        /// <summary>
        /// Handles collision.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="collisionHitInfo">Hit info.</param>
        private void OnCollision(object sender, Collider.CollisionHitInfo collisionHitInfo)
        {
            if (collisionHitInfo.Object is Ship ship)
            {
                float collisionVelocity = ship.Velocity.Length();
                if (collisionVelocity > 70)
                {
                    TakeDamage(10 * (collisionVelocity / 70));
                    ship.TakeDamage(10 * (collisionVelocity / 70));
                    Game.Instantiate(new Explosion(Game, Vector2.Lerp(Position, collisionHitInfo.Object.Position, 0.5f), 2 * (collisionVelocity / 70), 3));
                }
            }
        }

        /// <summary>
        /// Launches missile.
        /// </summary>
        /// <param name="rotation">Angle to fire in.</param>
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

        /// <summary>
        /// Removes missile when it is destroyed.
        /// </summary>
        /// <param name="sender">Missile.</param>
        /// <param name="e">Event arguments.</param>
        private void Missile_OnDestroy(object sender, EventArgs e)
        {
            missiles.Remove((Missile)sender);
        }

        /// <summary>
        /// Performs actions on a designated interval.
        /// </summary>
        public abstract void Act();

        /// <summary>
        /// Updates object.
        /// </summary>
        /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
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

        /// <summary>
        /// Recovers energy.
        /// </summary>
        /// <param name="energy">Amount to recover.</param>
        private void RecoverEnergy(float energy)
        {
            energy = Math.Abs(energy);
            Energy += energy;
            if (Energy > MaxEnergy)
            {
                Energy = MaxEnergy;
            }
        }


        /// <summary>
        /// Draws object.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawTurrets(spriteBatch);
            DrawName(spriteBatch);
        }

        /// <summary>
        /// Draws name.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
        private void DrawName(SpriteBatch spriteBatch)
        {
            SpriteFont font = FontLibrary.GetFont("fixedsys");
            Vector2 origin = font.MeasureString(Name) * 0.5f + Vector2.UnitY * Rectangle.Height * 8;
            spriteBatch.DrawString(font, Name, Position, Color.White, 0, origin, 0.18f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Draws turrets.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for drawing.</param>
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

        /// <summary>
        /// Initializes ship.
        /// </summary>
        /// <param name="enemyShip">Enemy ship.</param>
        /// <param name="getGameInformation">Function for getting game information.</param>
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

            turrets = new Turret[TURRET_COUNT];
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

        /// <summary>
        /// Gives missiles.
        /// </summary>
        /// <param name="amount">Amount to give.</param>
        internal void GiveMissiles(int amount)
        {
            MissileCount += amount;
        }

        /// <summary>
        /// Gives health.
        /// </summary>
        /// <param name="health">Amount to give.</param>
        internal void GiveHealth(float health)
        {
            Health = Math.Min(MaxHealth, Health + Math.Abs(health));
        }

        /// <summary>
        /// Gives energy.
        /// </summary>
        /// <param name="energy">Amount to give.</param>
        internal void GiveEnergy(float energy)
        {
            Energy = Math.Min(MaxEnergy, Energy + Math.Abs(energy));
        }

        /// <summary>
        /// Loses energy.
        /// </summary>
        /// <param name="energy">Amount to lose.</param>
        public void LoseEnergy(float energy)
        {
            Energy -= Math.Abs(energy);
            if (Energy < 0)
            {
                Energy = 0;
            }
        }

        /// <summary>
        /// Takes damage.
        /// </summary>
        /// <param name="damage">Amount to take.</param>
        public void TakeDamage(float damage)
        {
            Health -= Math.Abs(damage);
            if (Health < 0)
            {
                Health = 0;
            }
        }

        /// <summary>
        /// Sets the shooting value of all turrets.
        /// </summary>
        /// <param name="value">Value to set.</param>
        protected void SetShooting(bool value)
        {
            for(int i = 0; i < TurretCount; ++i)
            {
                turrets[i].IsFiring = value;
            }
        }
        /// <summary>
        /// Sets the shooting value of a specific turret.
        /// </summary>
        /// <param name="turretIndex">Index of turret to set value for.</param>
        /// <param name="value">Value to set.</param>
        protected void SetShooting(int turretIndex, bool value)
        {
            turrets[turretIndex].IsFiring = value;
        }
    }
}
