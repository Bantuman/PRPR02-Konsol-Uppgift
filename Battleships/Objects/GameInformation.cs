namespace Battleships.Objects
{
    /// <summary>
    /// Game information container for relaying game information to ships.
    /// </summary>
    public class GameInformation
    {
        public Pickup.Pickup[] Pickups { get; }
        public float TimeLeft          { get; }
        public bool OutsideMap         { get; }

        public GameInformation(Pickup.Pickup[] pickups, float timeLeft, bool outsideMap)
        {
            Pickups    = pickups;
            TimeLeft   = timeLeft;
            OutsideMap = outsideMap;
        }
    }
}
