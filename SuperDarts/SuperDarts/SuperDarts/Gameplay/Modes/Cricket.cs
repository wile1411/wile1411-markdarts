using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public class CricketSegment
    {
        public int Owner = -1;
        public bool IsAlive = true;

        /// <summary>
        /// Key = PlayerIndex, Value = Mark Count
        /// </summary>
        public Dictionary<int, int> Marks = new Dictionary<int, int>();

        public CricketSegment()
        {
            //add keys for each player
            for (int i = 0; i < 2; i++)
            {
                Marks.Add(i, 0);
            }
        }
    }

    public class Cricket : GameMode
    {
        /// <summary>
        /// Key = SegmentIndex, Value = Segment
        /// </summary>
        public Dictionary<int, CricketSegment> Segments = new Dictionary<int, CricketSegment>();
        public Dictionary<Dart, int> ScoredMarks = new Dictionary<Dart, int>();

        int Price = 3; // The number of hits a segment costs in order to own it

        public Cricket(int players)
            : base(players)
        {
            MaxRounds = 20;

            Segments.Add(25, new CricketSegment());

            for (int i = 20; i >= 15; i--)
                Segments.Add(i, new CricketSegment());

            BaseScreen = new CricketView(this);
        }

        public override void PlayAwards()
        {
            //base.PlayAwards();
        }

        public override int GetScore(Player player)
        {
            int score = 0;

            int playerIndex = -1;

            for (int i = 0; i < SuperDarts.Players.Count; i++)
            {
                if (SuperDarts.Players[i] == player)
                {
                    playerIndex = i;
                    break;
                }
            }

            foreach (KeyValuePair<int, CricketSegment> pair in Segments)
            {
                if (pair.Value.Owner == playerIndex)
                {
                    score += pair.Key * (pair.Value.Marks[playerIndex] - Price);
                }
            }

            return score;
        }

        public override void OnDartHit(Dart dart)
        {
            int scoredMarks = 0;

            //if the player hit a segment that matters
            if (Segments.Keys.Contains(dart.Segment))
            {
                CricketSegment s = Segments[dart.Segment];

                //if its still in the game
                if (s.IsAlive)
                {
                    s.Marks[CurrentPlayerIndex] += dart.Multiplier;
                    scoredMarks = dart.Multiplier; //test

                    if (s.Owner == -1)
                    {
                        if (s.Marks[CurrentPlayerIndex] >= Price)
                        {
                            s.Owner = CurrentPlayerIndex;
                        }
                    }
                    else
                    {
                        if (s.Owner != CurrentPlayerIndex)
                        {
                            if (s.Marks[CurrentPlayerIndex] >= Price)
                            {
                                s.IsAlive = false;
                                SuperDarts.SoundManager.PlaySound(SoundCue.CricketClosed);
                            }
                        }
                    }
                }
            }

            ScoredMarks.Add(dart, scoredMarks);

            //Check if someone closed all segments
            if (Segments.All(s => s.Value.IsAlive == false))
            {
                GameOver();
            }
            //Check if one player has all open segments AND is in the lead
            else if (GetLeaders().Any(p => Segments.Where(s => s.Value.IsAlive == true).All(s => SuperDarts.Players.ElementAtOrDefault(s.Value.Owner) == p) == true))
            {
                GameOver();
            }

            base.OnDartHit(dart);
        }

        public override string Name
        {
            get { return "Standard\nCricket"; }
        }
    }
}
