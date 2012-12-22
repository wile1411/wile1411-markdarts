using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class Shootout : GameMode
    {
        public override string Name
        {
            get { return "Shootout"; }
        }

        public Shootout(int players)
            : base(players)
        {
        }

        public override int GetScore(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
