using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            private float Cross(Vector2 v1, Vector2 v2)
            {
                return (v1.X * v2.Y - v1.Y * v2.X);
            }

            private Vector2 Cross(float v1, Vector2 v2)
            {
                return new Vector2(-v1 * v2.X, v1 * v2.X);
            }

            public void Update(GameTime gameTime)
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
                    for (int i = 0; i < collidingObjects.Count; i++)
                    {
                        Object collision = collidingObjects[i];
                        Vector2 nVector = -Vector2.UnitX * collision.Rectangle.Width; // Left collision

                        double collisionBottom = collision.Position.Y + collision.Rectangle.Height;
                        double holderBottom = Holder.Position.Y + Holder.Rectangle.Height;
                        double collisionRight = collision.Position.X + collision.Rectangle.Width;
                        double holderRight = Holder.Position.Y + Holder.Rectangle.Width;

                        double aCollision = holderBottom - collision.Position.Y;
                        double bCollision = collisionBottom - Holder.Position.Y;
                        double cCollision = holderRight - collision.Position.X;
                        double dCollision = collisionRight - Holder.Position.X;
                       
                        if (bCollision < aCollision && bCollision < cCollision && bCollision < dCollision)
                        {
                            // Top collision
                            nVector = Vector2.UnitY * collision.Rectangle.Height;
                          
                        }
                        if (aCollision < bCollision && aCollision < cCollision && aCollision < dCollision)
                        {
                            // Bottom collision
                            nVector = -Vector2.UnitY * collision.Rectangle.Height;
                        }
                        if (dCollision < cCollision && dCollision < bCollision && dCollision < aCollision)
                        {
                            // Right collision
                            nVector = Vector2.UnitX * collision.Rectangle.Width;
                        }

                        float nLength = nVector.Length();
                        float hLength = Holder.Rectangle.Height;
                        Vector2 collisionDirection = Vector2.Normalize(collision.Position - Holder.Position);

                        Holder.Velocity = (collisionDirection * (collision.Velocity.Length() - Holder.Velocity.Length())) / 2; // Dividing by 2 because mass and stuff
                        collision.Velocity = (collisionDirection * (Holder.Velocity.Length() - collision.Velocity.Length())); 
                    }
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