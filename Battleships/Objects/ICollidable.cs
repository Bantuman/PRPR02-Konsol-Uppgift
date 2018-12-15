namespace Battleships.Objects
{
    /// <summary>
    /// Interface for collidable object.
    /// </summary>
    public interface ICollidable
    {
        Object.Collider Collider { get; }
    }
}