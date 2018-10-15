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

    public abstract partial class Object : IObject
    {
        public class Collider
        {
            public struct CollisionHitInfo
            {
                public Object Object { get; set; }

                public CollisionHitInfo(Object obj)
                {
                    Object = obj;
                }
            }

            private static List<Collider> colliders = new List<Collider>();

            private Object Holder { get; }
            private ColliderType ColliderType { get; set; }
            private Vector2 previousPosition;
            private List<Object> previousCollidingObjects;

            public delegate void CollisionHandler(CollisionHitInfo collided);
            public event CollisionHandler OnCollisionEnter;

            public Collider(Object holder)
            {
                colliders.Add(this);
                Holder = holder;
                previousCollidingObjects = new List<Object>();
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
                    foreach (Object obj in collidingObjects)
                    {
                        if (previousCollidingObjects.Contains(obj))
                        {
                            continue;
                        }
                        OnCollisionEnter?.Invoke(new CollisionHitInfo(obj));
                    }
                    Holder.Position = previousPosition;
                    Holder.Velocity *= -1f;
                }
                previousPosition = Holder.Position;
                previousCollidingObjects = collidingObjects;
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