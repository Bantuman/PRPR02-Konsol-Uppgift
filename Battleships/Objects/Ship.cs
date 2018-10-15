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
        public Animation.Animation[] Animations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Turret[] turrets;
        private float energy;
        private float health;

        public Ship(Vector2 position) : base(TextureLibrary.GetTexture("Ship"))
        {
            Point size = new Point(64, 32);
            Rectangle = new RotatedRectangle(new Rectangle(position.ToPoint(), size), 0);
            Collider = new Collider(this);
            Position = position;
            Animator = new Animator(new Animation.Animation(Texture, new Point(64, 32), new Point(3, 1), 5));
        }

        public abstract void Act();

        public sealed override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }
    }
}