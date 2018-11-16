using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    public enum ColliderType
    {
        Trigger,
        Static
    }

    public abstract partial class Object : IObject
    {
        public class Collider
        {
            public ColliderType ColliderType { get; set; }
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
            private Vector2 previousPosition;
            private List<Object> previousCollidingObjects;
            private List<ICollidable> ignoreList;

            public delegate void CollisionHandler(object sender, CollisionHitInfo collided);
            public event CollisionHandler OnCollisionEnter;

            public Collider(Object holder, ColliderType type, List<ICollidable> ignore = null)
            {
                if (type == ColliderType.Static)
                {
                    colliders.Add(this);
                }
                ColliderType = type;
                Holder = holder;
                holder.OnDestroy += Holder_OnDestroy;
                previousCollidingObjects = new List<Object>();
                ignoreList = ignore ?? new List<ICollidable> { };
            }

            private void Holder_OnDestroy(object sender, EventArgs e)
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
                List<Object> collidingObjects = GetCollidingObjects(ColliderType.Static);
                if (collidingObjects.Count > 0)
                {
                    Holder.Position = previousPosition;
                    foreach (Object obj in collidingObjects)
                    {
                         if (previousCollidingObjects.Contains(obj))
                        {
                            continue;
                        }
                        OnCollisionEnter?.Invoke(this, new CollisionHitInfo(obj));
                    }
                    if (ColliderType == ColliderType.Static)
                    {
                        for (int i = 0; i < collidingObjects.Count; i++)
                        {
                            Object collision = collidingObjects[i];
                            if ((collision as ICollidable).Collider.ColliderType == ColliderType.Static)
                            {
                                Vector2 nVector = -Vector2.UnitX * collision.Rectangle.Width; // Left Collision

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
                                    // Top Collision
                                    nVector = Vector2.UnitY * collision.Rectangle.Height;

                                }
                                if (aCollision < bCollision && aCollision < cCollision && aCollision < dCollision)
                                {
                                    // Bottom Collision
                                    nVector = -Vector2.UnitY * collision.Rectangle.Height;
                                }
                                if (dCollision < cCollision && dCollision < bCollision && dCollision < aCollision)
                                {
                                    // Right Collision
                                    nVector = Vector2.UnitX * collision.Rectangle.Width;
                                }

                                float nLength = nVector.Length();
                                float hLength = Holder.Rectangle.Height;
                                Vector2 collisionDirection = Vector2.Normalize(collision.Position - Holder.Position);

                                Holder.Velocity = (collisionDirection * (collision.Velocity.Length() - Holder.Velocity.Length())) / 2;
                                collision.Velocity = (collisionDirection * (Holder.Velocity.Length() - collision.Velocity.Length()));
                            }
                        }
                    }
                }
                previousPosition = Holder.Position;
                previousCollidingObjects = collidingObjects;
            }
            
            public List<Object> GetCollidingObjects(ColliderType type)
            {
                List<Object> collidingObjects = new List<Object>();
                foreach (Collider collider in colliders)
                {
                    if (collider == this || ignoreList.Contains(collider as ICollidable))
                    {
                        continue;
                    }
                    if (collider.Holder.Rectangle.Intersects(Holder.Rectangle) && collider.ColliderType == type)
                    {
                        collidingObjects.Add(collider.Holder);
                    }
                }
                return collidingObjects;
            }
        }
    }
}