using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public class BastardHit
    {
        public Dart Dart;
        public int Round;

        public Player ThrownBy;
        public Player SegmentOwner;

        public BastardHit(Dart dart, Player thrownBy, Player segmentOwner, int round)
        {
            Dart = dart;
            ThrownBy = thrownBy;
            SegmentOwner = segmentOwner;
            Round = round;
        }
    }

    public class Bastard : GameMode
    {
        #region Fields and Properties
        BastardSummaryScreen summaryScreen;
        public int StartScore = 10;
        public override string Name
        {
            get { return "Bastard"; }
        }
        public Dictionary<int, Player> PlayerSegments = new Dictionary<int, Player>();
        #endregion

        #region Constructor and Setup
        public Bastard(int players)
            : base(players)
        {
            setupSegments(players);
            BaseScreen = new BastardView(this);
            summaryScreen = new BastardSummaryScreen(this);

            ScoringDirection = Direction.ASC;
        }

        private void setupSegments(int players)
        {
            int segmentsPerPlayer = 20 / players;

            for (int i = 0; i < segmentsPerPlayer; i++)
            {
                for (int j = i * players; j < players * (i + 1); j++)
                {
                    PlayerSegments.Add(Dartboard.SegmentOrder[j], SuperDarts.Players[j - i * players]);
                }
            }
        }
        #endregion

        public override int GetScore(Player player)
        {
            return CalculatePlayerScores()[player];
        }

        public override int GetScore(Dart d)
        {
            return d.Multiplier;
        }

        private Dictionary<Player, int> CalculatePlayerScores()
        {
            Dictionary<Player, int> playerScores = new Dictionary<Player, int>();
            Players.ForEach(x => playerScores.Add(x, StartScore));

            for (int i = 0; i <= CurrentRoundIndex; i++)
            {
                for (int j = 0; j < SuperDarts.Players.Count; j++)
                {
                    Player player = SuperDarts.Players[j];
                    Round round = player.Rounds[i];

                    for (int k = 0; k < round.Darts.Count; k++)
                    {
                        Dart dart = round.Darts[k];
                        //If the owner of the segment is the one we are calculating score for and the owner of the dart is the player
                        Player segementOwner = null;
                        if (PlayerSegments.Keys.Contains(dart.Segment))
                            segementOwner = PlayerSegments[dart.Segment];
                        else
                            continue;

                        if (segementOwner == player)
                            playerScores[player] -= dart.Multiplier;
                        else
                            playerScores[segementOwner] += dart.Multiplier;
                    }
                }
            }
            return playerScores;
        }
    }
}
