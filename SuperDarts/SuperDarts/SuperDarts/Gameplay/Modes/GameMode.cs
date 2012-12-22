using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SuperDarts
{
    public enum Direction
    {
        ASC,
        DESC
    }

    /// <summary>
    /// This class serves as the base for all GameModes
    /// </summary>
    public abstract class GameMode
    {
        #region Fields and Properties
        public int CurrentPlayerIndex = 0;
        public int CurrentRoundIndex = 0;
        public int DartsPerRound = 3;
        public int MaxRounds;

        public bool IsGameOver = false;
        public bool Paused = false;

        public abstract string Name { get; }

        public BaseView BaseScreen;

        public List<Player> Players
        {
            get
            {
                return SuperDarts.Players;
            }
        }

        public Round CurrentPlayerRound
        {
            get
            {
                return CurrentPlayer.Rounds[CurrentRoundIndex];
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return SuperDarts.Players[CurrentPlayerIndex];
            }
        }

        AwardScreen awardScreen;

        public GameplayScreen GameplayScreen;
        PlayerChangeScreen playerChangeScreen;
        TimeoutScreen throwDartsScreen;
        TimeoutScreen newRoundTimeoutScreen;
        public Direction ScoringDirection = Direction.DESC;
        #endregion

        #region Constructor
        public GameMode(int players)
        {
            SuperDarts.Players.Clear();

            MaxRounds = SuperDarts.Options.MaxRounds;

            addPlayers(players);

            awardScreen = new AwardScreen();

            playerChangeScreen = new PlayerChangeScreen("Player Change", TimeSpan.FromSeconds(SuperDarts.Options.PlayerChangeTimeout));
            playerChangeScreen.OnTimeout = new TimeoutScreen.TimeoutDelegate(playerChange);
            throwDartsScreen = new TimeoutScreen(CurrentPlayer.Name + " throw darts!", TimeSpan.FromSeconds(3));

            newRoundTimeoutScreen = new TimeoutScreen("Round " + (CurrentRoundIndex + 1).ToString(), TimeSpan.FromSeconds(3));
            newRoundTimeoutScreen.OnTimeout = new TimeoutScreen.TimeoutDelegate(showThrowDartsMessage);
        }

        private void addPlayers(int players)
        {
            for (int i = 0; i < players; i++)
            {
                Player p = new Player("Player " + (i + 1).ToString());
                SuperDarts.Players.Add(p);
                for (int j = 0; j < MaxRounds; j++)
                {
                    p.Rounds.Add(new Round());
                }
            }
        }
        #endregion

        #region Show Message Screen Helpers
        public void showPlayerChangeScreen()
        {
            playerChangeScreen.Timeout = TimeSpan.FromSeconds(SuperDarts.Options.PlayerChangeTimeout);
            SuperDarts.ScreenManager.AddScreen(playerChangeScreen);
        }

        private void showNewRoundMessageScreen()
        {
            newRoundTimeoutScreen.Text = "Round " + (CurrentRoundIndex + 1).ToString();
            newRoundTimeoutScreen.ElapsedTime = 0;
            SuperDarts.ScreenManager.AddScreen(newRoundTimeoutScreen);
        }

        private void showThrowDartsMessage()
        {
            throwDartsScreen.Text = CurrentPlayer.Name + " throw darts!";
            throwDartsScreen.ElapsedTime = 0;
            SuperDarts.ScreenManager.AddScreen(throwDartsScreen);
        }
        #endregion


        /// <summary>
        /// This method is run at the beginning of a new round
        /// </summary>
        private void startRound()
        {
            //Reset current player index and increment round
            CurrentPlayerIndex = 0;
            CurrentRoundIndex++;

            //Check if its the last round
            if (CurrentRoundIndex == MaxRounds - 1)
            {
                // Play last round sound
                SuperDarts.SoundManager.PlaySound(SoundCue.LastRound);
            }
            else
            {
                // Play new round sound
                SuperDarts.SoundManager.PlaySound(SoundCue.NewRound);
            }

            showNewRoundMessageScreen();
        }

        /// <summary>
        /// This method is called by pressing the skip button, for example when a player missed a dart
        /// </summary>
        public virtual void ForcePlayerChange()
        {
            if (!IsGameOver)
            {
                for (; CurrentPlayerRound.Darts.Count < DartsPerRound; )
                {
                    RegisterDart(0, 0);
                }
            }
        }

        /// <summary>
        /// This method is triggered when the player change message has timed out or has been closed
        /// </summary>
        private void playerChange()
        {
            //Stop any award videos that are currently playing
            awardScreen.Stop();

            //Increment player index
            CurrentPlayerIndex++;

            //Check if it's a new round
            if (CurrentPlayerIndex > SuperDarts.Players.Count - 1)
            {
                startRound();
            }
            else
            {
                SuperDarts.SoundManager.PlaySound(SoundCue.ThrowStart);
                showThrowDartsMessage();
            }
        }


        /// <summary>
        /// This method is called at the end of a round (before the start of a new round)
        /// </summary>
        /// <param name="isGameOver"></param>
        public virtual void OnNextPlayer(out bool isGameOver)
        {
            isGameOver = false;
        }

        /// <summary>
        /// This method is called when the round is over and we should switch players
        /// </summary>
        public void NextPlayer()
        {
            bool isGameOver;
            OnNextPlayer(out isGameOver);

            //Check if it was the last player and the last round
            bool last = (CurrentPlayerIndex == SuperDarts.Players.Count - 1 && CurrentRoundIndex == MaxRounds - 1);
            if (isGameOver || last)
            {
                GameOver();
            }
            else
            {
                showPlayerChangeScreen();

                if(SuperDarts.Options.PlayAwards)
                    PlayAwards();
            }
        }

        /// <summary>
        /// Checks if any conditions for the awards are met and plays the award video
        /// </summary>
        public virtual void PlayAwards()
        {
            if(CurrentPlayerRound.Darts.Count != 3)
                return;

            if (GetScore(CurrentPlayerRound) == 180)
            {
                awardScreen.Play(AwardCue.TonEighty);
            }
            else if (CurrentPlayerRound.Darts.TrueForAll(delegate(Dart dart)
            {
                return dart.Segment == 25 && dart.Multiplier == 2;
            }))
            {
                //Play award three in the black
                awardScreen.Play(AwardCue.ThreeInTheBlack);
            }
            else if (CurrentPlayerRound.Darts.TrueForAll(delegate(Dart dart)
            {
                return dart.Segment == 25;
            }))
            {
                //Play award hattrick!
                awardScreen.Play(AwardCue.HatTrick);
            }
            else if (GetScore(CurrentPlayerRound) > 150)
            {
                awardScreen.Play(AwardCue.HighTon);
            }
            else if (GetScore(CurrentPlayerRound) >= 100)
            {
                //Play award low ton!
                awardScreen.Play(AwardCue.LowTon);
            }
            else if (CurrentPlayerRound.Darts.All(x =>
                x.Segment != 0 &&
                x.Multiplier != 0 &&
                x.Segment == CurrentPlayerRound.Darts[0].Segment &&
                x.Multiplier == CurrentPlayerRound.Darts[0].Multiplier))
            {
                awardScreen.Play(AwardCue.ThreeInABed);
            }
        }

        /// <summary>
        /// This method is fired by the serial manager when a dart hits the dartboard
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="multiplier"></param>
        public void RegisterDart(int segment, int multiplier)
        {
            if (IsGameOver || Paused)
                return;

            // Check if we have already thrown all darts for this round
            if (CurrentPlayerRound.Darts.Count >= DartsPerRound)
                return;

            //Else register the dart

            //Exit screens in case they are still in view
            newRoundTimeoutScreen.ExitScreen(this, null);
            throwDartsScreen.ExitScreen(this, null);

            //Add the dart and play sound
            Dart dart = new Dart(CurrentPlayer, segment, multiplier);
            CurrentPlayerRound.Darts.Add(dart);
            PlaySound(dart);

            //Run this method to allow the game modes to run their logic whenever a dart is registered
            OnDartHit(dart);
        }

        /// <summary>
        /// This method is called just as a dart hits the board
        /// </summary>
        /// <param name="dart"></param>
        public virtual void OnDartHit(Dart dart)
        {
            bool lastThrow = CurrentPlayerRound.Darts.Count == DartsPerRound;
            //Check if its the last throw for the currentplayer
            if (lastThrow)
            {
                NextPlayer();
            }
        }

        public abstract int GetScore(Player player);

        public virtual int GetScore(Round r)
        {
            return r.Darts.Sum(x => GetScore(x));
        }

        public virtual int GetScore(Dart d)
        {
            if (d.Segment == 25)
                return d.Segment * 2; // Regardless of multiplier (single/double bull)
            else
                return d.Segment * d.Multiplier;
        }

        #region Update and Draw
        public virtual void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            BaseScreen.Update(gameTime, isCoveredByOtherScreen);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            BaseScreen.Draw(spriteBatch);
            awardScreen.Draw(spriteBatch);
        }
        #endregion

        #region LoadContent
        public virtual void LoadContent(ContentManager content)
        {
            BaseScreen.LoadContent(content);
            awardScreen.LoadContent(content);
            playerChangeScreen.LoadContent(content);
        }
        #endregion

        public virtual void GameOver()
        {
            if (IsGameOver)
                return;

            IsGameOver = true;
            SuperDarts.SoundManager.PlaySound(SoundCue.Won);

            List<Player> leaders = GetLeaders();

            string text = "Winner";

            if (leaders.Count > 1)
                text = "Draw";

            leaders.ForEach(p => text += " " + p.Name);

            MessageBoxScreen gameOverScreen = new MessageBoxScreen("Game Over", text, MessageBoxButtons.Ok);
            gameOverScreen.OnOk += new EventHandler(delegate(object sender, EventArgs e) { GameplayScreen.Pause(); });
            SuperDarts.ScreenManager.AddScreen(gameOverScreen);
        }

        public virtual List<Player> GetLeaders()
        {
            if (ScoringDirection == Direction.ASC)
                return SuperDarts.Players.GroupBy(p => GetScore(p)).OrderBy(g => g.Key).First().ToList();
            else
                return SuperDarts.Players.GroupBy(p => GetScore(p)).OrderBy(g => g.Key).Last().ToList();
        }

        void removeDart()
        {
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                CurrentPlayerRound.Darts.RemoveAt(CurrentPlayerRound.Darts.Count - 1);
                IsGameOver = false;
            }
        }

        public virtual void Unthrow()
        {
            //Remove last dart of the current players throws
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                removeDart();
            }
            //Back up one player and remove dart
            else if (CurrentPlayerIndex > 0)
            {
                CurrentPlayerIndex--;
                removeDart();
            }
            //Back up one round
            else if (CurrentRoundIndex > 0)
            {
                CurrentRoundIndex--;
                CurrentPlayerIndex = SuperDarts.Players.Count - 1;
                removeDart();
            }
            else
            {
                //No dart has been thrown?
            }
        }

        #region SoundHelper
        private static void PlaySound(Dart dart)
        {
            if (dart.Segment == 25)
            {
                if (dart.Multiplier == 2)
                    SuperDarts.SoundManager.PlaySound(SoundCue.DoubleBull);
                else
                    SuperDarts.SoundManager.PlaySound(SoundCue.SingleBull);
            }
            else
            {
                if (dart.Multiplier == 2)
                    SuperDarts.SoundManager.PlaySound(SoundCue.Double);
                else if (dart.Multiplier == 3)
                    SuperDarts.SoundManager.PlaySound(SoundCue.Triple);
                else
                    SuperDarts.SoundManager.PlaySound(SoundCue.Single);
            }
        }
        #endregion
    }
}
