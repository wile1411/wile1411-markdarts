using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class Player
    {
        public string Name { get; set; }

        public List<Round> Rounds = new List<Round>();

        public Color Color
        {
            get
            {
                int index = SuperDarts.Players.FindIndex(p => p == this);
                return SuperDarts.Options.PlayerColors[index % SuperDarts.Options.PlayerColors.Length];
            }
        }

        public Player(string name)
        {
            Name = name;
        }
    }
}
