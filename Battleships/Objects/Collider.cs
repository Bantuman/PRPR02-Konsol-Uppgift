using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Battleships.Objects
{
    /// <summary>
    /// Collider types.
    /// </summary>
    public enum ColliderType
    {
        Trigger,
        Static
    }

    /// <summary>
    /// Base class for objects.
    /// </summary>
    public abstract partial class Object : IObject
    {
        /// <summary>
        /// Collider class for objects to use.
        /// </summary>
        public class Collider
        {
            /// <summary>
            /// Event argument class for hit info from collisions.
            /// </summary>
            public class CollisionHitInfo : EventArgs
            {
                public Object Object { get; set; }

                public CollisionHitInfo(Object obj)
                {
                    Object = obj;
                }
            }

            public delegate void CollisionHandler(object sender, CollisionHitInfo e);

            private static List<Collider> colliders = new List<Collider>();

            public ColliderType           ColliderType { get; set; }

            public event CollisionHandler OnCollisionEnter;

            private Object                Holder       { get; }

            private Vector2               previousPosition;
            private List<Object>          previousCollidingObjects;
            private List<ICollidable>     ignoreList;

            public Collider(Object holder, ColliderType type, List<ICollidable> ignore = null)
            {
                if (type == ColliderType.Static)
                {
                    colliders.Add(this);
                }

                ColliderType             = type;
                Holder                   = holder;
                holder.OnDestroy        += Holder_OnDestroy;
                previousCollidingObjects = new List<Object>();
                ignoreList               = ignore ?? new List<ICollidable>();
            }

            /// <summary>
            /// Resets the static collider container.
            /// </summary>
            public static void Reset()
            {
                colliders = new List<Collider>();
            }

            /// <summary>
            /// Removes this from static collider container on holder destruction.
            /// </summary>
            /// <param name="sender">Sender.</param>
            /// <param name="e">Event arguments.</param>
            private void Holder_OnDestroy(object sender, EventArgs e)
            {
                colliders.Remove(this);
            }

            /// <summary>
            /// Updates collider.
            /// </summary>
            /// <param name="gameTime">Container for time data such as elapsed time since last update.</param>
            public void Update(GameTime gameTime)
            {
                List<Object> collidingObjects = GetCollidingObjects(ColliderType.Static);
                if (collidingObjects.Count > 0)
                {
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
                        Holder.Position = previousPosition;
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
            
            /// <summary>
            /// Gets all colliding objects.
            /// </summary>
            /// <param name="type">Collider type to check for.</param>
            /// <returns>List of colliding objects.</returns>
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