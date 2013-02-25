using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public void GetVerbose(out string text, out Color color)
        {
            color = Color.White;
            text = "";

            if (Multiplier == 1)
            {
                text = "Single";
            }
            else if (Multiplier == 2)
            {
                text = "Double";
                color = Color.Yellow;
            }
            else if (Multiplier == 3)
            {
                text = "Triple";
                color = Color.Magenta;
            }

            if (Segment == 0 && Multiplier == 0)
                text = "MISS";
            else if (Segment != 25)
                text += " " + Segment.ToString();
            else
            {
                text += " BULL";
                color = Color.Red;
            }
        }

        public override string ToString()
        {
            return "Segment: " + Segment + ", Multiplier: " + Multiplier;
        }
    }
}
