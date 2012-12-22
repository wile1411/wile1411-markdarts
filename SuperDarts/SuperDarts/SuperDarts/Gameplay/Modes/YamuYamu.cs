using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class YamuYamu : GameMode
    {
        public override string Name
        {
            get { return "YamuYamu"; }
        }

        public YamuYamu(int players)
            : base(players)
        {
            string[] modes = new string[] { "Any Double", "Any Triple", "Bulls-Eye", "One Dart", "Random Segment" };
        }

        public override int GetScore(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
