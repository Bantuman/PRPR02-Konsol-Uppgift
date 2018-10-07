using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    enum ColliderType
    {
        Trigger,
        Static
    }

    abstract partial class Object : IObject
    {
        public class Collider
        {
            private static List<Collider> colliders = new List<Collider>();

            private Object Holder { get; }
            private ColliderType ColliderType { get; set; }
            private Vector2 previousPosition;

            public event EventHandler OnCollisionEnter;
            public event EventHandler OnCollisionExit;
            public event EventHandler OnCollisionStay;

            public Collider(Object holder)
            {
                colliders.Add(this);
                Holder = holder;
            }

            ~Collider()
            {
                colliders.Remove(this);
            }

            public void Update()
            {
                List<Object> collidingObjects = GetCollidingObjects();
                if (collidingObjects.Count > 0)
                {
                    Holder.Position = previousPosition;
                    Holder.Velocity *= -0.8f;
                }

                previousPosition = Holder.Position;
            }
            
            public List<Object> GetCollidingObjects()
            {
                List<Object> collidingObjects = new List<Object>();
                foreach (Collider collider in colliders)
                {
                    if (collider == this)
                    {
                        continue;
                    }
                    if (collider.Holder.Rectangle.Intersects(Holder.Rectangle))
                    {
                        collidingObjects.Add(collider.Holder);
                    }
                }
                return collidingObjects;
            }
        }
    }
}