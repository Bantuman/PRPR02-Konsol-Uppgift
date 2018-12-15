using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Objects
{
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
