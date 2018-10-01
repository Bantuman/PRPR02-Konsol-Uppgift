using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships.Objects
{
    abstract class Ship : IObject
    {
        private Turret[] turrets;
        private float energy;
        private float health;

        public abstract void Act();
    }
}