using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class ZeroOne : GameMode
    {
        #region Fields and Properties
        public int StartScore;
        TimeoutScreen bustScreen;
        #endregion

        #region Constructor
        public ZeroOne(int players, int startScore)
            : base(players)
        {
            StartScore = startScore;
            BaseScreen = new ScoreView(this);
            bustScreen = new TimeoutScreen("Bust", TimeSpan.FromSeconds(3));
            bustScreen.OnTimeout = new TimeoutScreen.TimeoutDelegate(delegate()
            {
                CurrentPlayerRound.Darts.Clear(); // Remove darts for this round
                Paused = false;
                NextPlayer();
            });
            bustScreen.Color = Color.Red;
            ScoringDirection = Direction.ASC;
        }
        #endregion

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            bustScreen.LoadContent(content);
        }

        public override int GetScore(Player p)
        {
            int score = StartScore; // - Handicap
            score -= p.Rounds.Sum(r => GetScore(r));
            return score;
        }

        public override void OnDartHit(Dart dart)
        {
            // Check to see if we won or got bust?
            if (GetScore(CurrentPlayer) == 0)
            {
                NextPlayer();
            }
            else if (GetScore(CurrentPlayer) < 0)
            {
                bust();
            }
            else
            {
                base.OnDartHit(dart);
            }
        }

        public override void OnNextPlayer(out bool isGameOver)
        {
            isGameOver = false;
            var lastPlayer = CurrentPlayerIndex == SuperDarts.Players.Count - 1;
            if (lastPlayer && SuperDarts.Players.Any(delegate(Player p)
            {
                return GetScore(p) == 0;
            }))
            {
                isGameOver = true;
            }
        }

        private void bust()
        {
            SuperDarts.SoundManager.PlaySound(SoundCue.Bust);

            bustScreen.ElapsedTime = 0;
            SuperDarts.ScreenManager.AddScreen(bustScreen);
            Paused = true;
        }

        public override string Name
        {
            get { return "01 (" + StartScore.ToString() + ")"; }
        }
    }
}
