using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class Dart
    {
        #region Fields and Properties
        public int Segment = 0;
        public int Multiplier = 1;

        public int ScoredMarks = 0; // Used for cricket
        public Player Owner;
        #endregion

        public Dart(Player owner, int segment, int multiplier)
        {
            Owner = owner;
            Segment = segment;
            Multiplier = multiplier;
        }

        public override string ToString()
        {
            return "Segment: " + Segment + ", Multiplier: " + Multiplier;
        }
    }
}
