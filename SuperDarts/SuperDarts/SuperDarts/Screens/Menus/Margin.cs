using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class Margin
    {
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public Margin(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public static Margin Zero
        {
            get
            {
                return new Margin(0, 0, 0, 0);
            }
        }
    }
}
