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
        public Animator Animator { get; set; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public string Name { get; private set; }
        public float Energy { get; private set; }
        public float MaxEnergy { get; private set; }
        public Turret[] Turrets { get; private set; }
        public Ship EnemyShip { get; protected set; }
        public List<IObject> Objects { get; private set; }

        private bool initialized;
        private const int turretCount = 6;

        internal float Damage { get => 10; }

        public Ship(IGame1 game, Vector2 position) : base(game, TextureLibrary.GetTexture("Ship"))
        {
            Point size                 = new Point(64, 32);
            Rectangle                  = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider                   = new Collider(this, ColliderType.Static);
            Animator                   = new Animator(new Animation.Animation(Texture, new Point(64, 32), new Point(3, 1), 5f));
            initialized                = false;
            Position                   = position;
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
            foreach(Turret turret in Turrets)
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
            for (int i = 0; i < Turrets.Length; ++i)
            {
                Vector2 originalPosition = Turrets[i].RelativePosition;
                Turrets[i].FacingLeft = (i > (Turrets.Length - 2) / 2);

                float rotation = Rotation + (Turrets[i].FacingLeft ? (float)Math.PI : 0);
                float cosTetha = (float) Math.Cos(rotation);
                float sinTetha = (float) Math.Sin(rotation);

                Turrets[i].RotatedPosition = new Vector2(originalPosition.X * cosTetha - originalPosition.Y * sinTetha, originalPosition.X * sinTetha + originalPosition.Y * cosTetha);

                Vector2 offset = Turrets[i].Texture.Bounds.Size.ToVector2() / 2;

                spriteBatch.Draw(Turrets[i].Texture, new Rectangle(Position.ToPoint() + Turrets[i].RotatedPosition.ToPoint(), Turrets[i].Size.ToPoint()), null, Color.White, rotation, offset, SpriteEffects.None, Layer + 0.01f);
            }
        }

        public void Initialize(Ship OtherPlayer)
        {
            if (initialized)
            {
                throw new Exception("Ship has already been initialized.");
            }
            if (Game == null)
            {
                throw new Exception("Value of game has not been set.");
            }

            EnemyShip = OtherPlayer;
            Turrets = new Turret[turretCount];
            for(int i = 0; i < Turrets.Length; ++i)
            {
                Vector2 position = new Vector2(-Rectangle.CollisionRectangle.Size.X / 2 + 28 * (i % (Turrets.Length / 2)), 20);
                if (i > (Turrets.Length - 1) / 2)
                {
                    position.X *= -1;
                }
                Turrets[i] = new Turret(Game, this, position, new Vector2(10, 25));
            }

            initialized = true;
        }
        public void GiveHealth(float health)
        {
            Health += Math.Abs(health);
        }
        public void GiveEnergy(float energy)
        {
            Energy += Math.Abs(energy);
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