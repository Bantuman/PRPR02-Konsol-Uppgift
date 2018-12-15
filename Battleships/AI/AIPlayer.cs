using Microsoft.Xna.Framework;

namespace Battleships.Objects
{
    public class AIPlayer : Ship
    {
        public AIPlayer(Vector2 position, Color color) :
            base(null, position)
        {}

        public override void Act() { }
    }
}