using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public class BaseView : GameScreen
    {
        public GameMode Mode;

        public Texture2D DartTexture;
        Texture2D solidDart;
        Texture2D crown;

        Texture2D[] numberTextures = new Texture2D[3];

        const int PlayerPanelMaxWidth = 400;

        AnimatedSprite serialAnimation;
        Texture2D serialDisconnected;

        float elapsedTime = 0;
        float dartBlinkRate = 500.0f;
        private Texture2D glow;

        public int PlayerCount { get { return SuperDarts.Players.Count; } }
        public float glowAlpha = 0;

        Texture2D playerNameBackground;

        public BaseView(GameMode mode)
        {
            Mode = mode;
        }

        public virtual void LoadContent(ContentManager content)
        {
            DartTexture = content.Load<Texture2D>(@"Images\DartStroke");
            solidDart = content.Load<Texture2D>(@"Images\DartHighlight");
            crown = content.Load<Texture2D>(@"Images\Crown");
            glow = content.Load<Texture2D>(@"Images\Glow");
            playerNameBackground = content.Load<Texture2D>(@"Images\PlayerNameBackground");

            serialDisconnected = content.Load<Texture2D>(@"Images\SerialDisconnected");

            serialAnimation = new AnimatedSprite();
            serialAnimation.Frames = 18;
            serialAnimation.Texture = content.Load<Texture2D>(@"Images\Serial");
            serialAnimation.SourceRectangle = new Rectangle(0, 0, 148, 77);

            for (int i = 0; i < 3; i++)
            {
                numberTextures[i] = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + @"CricketNumbers\" + (i + 1).ToString());
            }
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            glowAlpha = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1.0f) / 2.0f;

            serialAnimation.Update(gameTime);

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            // Temporary variables
            string text = "";
            var position = Vector2.One * 20.0f;
            Vector2 center = Vector2.Zero;
            Vector2 offset;

            var smallFont = ScreenManager.Trebuchet22;
            var bigFont = ScreenManager.Trebuchet32;
            var scoreFont = ScreenManager.Trebuchet48;

            //Draw game mode name
            text = "Game Mode:";
            TextBlock.DrawShadowed(spriteBatch, smallFont, text, Color.LightBlue, position);
            position.Y += smallFont.LineSpacing;

            text = Mode.Name;
            TextBlock.DrawShadowed(spriteBatch, bigFont, text, Color.White, position);
            position.Y += bigFont.MeasureString(text).Y;

            //Draw serial port status
            DrawSerialPortStatus(spriteBatch);

            //Draw round x of y
            text = "Round:";
            TextBlock.DrawShadowed(spriteBatch, smallFont, text, Color.LightBlue, position);
            position.Y += smallFont.LineSpacing;

            text = (Mode.CurrentRoundIndex + 1).ToString() + "/" + Mode.MaxRounds.ToString();
            TextBlock.DrawShadowed(spriteBatch, bigFont, text, Color.White, position);
            position.Y += bigFont.LineSpacing;

            //Draw current player name
            TextBlock.DrawShadowed(spriteBatch, bigFont, Mode.CurrentPlayer.Name, Mode.CurrentPlayer.Color, position);
            position.Y += bigFont.LineSpacing;

            // Draw player panels
            int playerPanelWidth = SuperDarts.Viewport.Width / SuperDarts.Players.Count;

            if (playerPanelWidth > PlayerPanelMaxWidth)
                playerPanelWidth = PlayerPanelMaxWidth;

            position = new Vector2(SuperDarts.Viewport.Width * 0.5f - SuperDarts.Players.Count / 2.0f * playerPanelWidth, SuperDarts.Viewport.Height * 0.8f);
            List<Player> leaders = Mode.GetLeaders();
            for (int i = 0; i < PlayerCount; i++)
            {
                text = SuperDarts.Players[i].Name;
                SpriteFont nameFont = bigFont;
                Vector2 nameSize = nameFont.MeasureString(text);

                Color background = Color.White * 0.33f;
                Color foreground = Color.White;
                Color shadow = Color.Black;

                Color scoreBackground = SuperDarts.Players[i].Color; //SuperDarts.Options.PlayerColors[i % SuperDarts.Options.PlayerColors.Length] * 0.33f;
                var namePanelRectangle = new Rectangle((int)position.X, (int)position.Y, playerPanelWidth, (int)nameSize.Y);

                string score = Mode.GetScore(Mode.Players[i]).ToString();
                Vector2 scoreSize = scoreFont.MeasureString(score);

                int y = (int)(position.Y + nameSize.Y);
                int height = SuperDarts.Viewport.Height - y;
                Rectangle scoreRectangle = new Rectangle((int)position.X, y, playerPanelWidth, height);

                if (Mode.CurrentPlayerIndex == i)
                {
                    background = Color.White;
                    shadow = Color.White;
                    foreground = SuperDarts.Players[i].Color;

                    //Draw glow
                    spriteBatch.Draw(glow, new Vector2(scoreRectangle.Center.X, scoreRectangle.Center.Y), null, SuperDarts.Players[i].Color * glowAlpha, 0, new Vector2(glow.Width, glow.Height) * 0.5f, 3.0f, SpriteEffects.None, 0);
                }
                else
                {
                    scoreBackground *= 0.33f;
                }

                //Draw player name panel
                spriteBatch.Draw(playerNameBackground, namePanelRectangle, background);

                //Draw player name
                center = new Vector2(playerPanelWidth, nameSize.Y) * 0.5f;
                offset = nameSize * 0.5f;
                spriteBatch.DrawString(nameFont, text, position + center - offset + Vector2.One, shadow);
                spriteBatch.DrawString(nameFont, text, position + center - offset, foreground);

                //Draw score background rectangle
                spriteBatch.Draw(ScreenManager.BlankTexture, scoreRectangle, scoreBackground);

                center = new Vector2(playerPanelWidth, height) * 0.5f;
                offset = scoreSize * 0.5f;

                //Draw score
                var scorePos = new Vector2(position.X, y) + center - offset;
                TextBlock.DrawShadowed(spriteBatch, scoreFont, score.ToString(), Color.White, scorePos);

                //Draw crown
                if (leaders.Contains(SuperDarts.Players[i]))
                    spriteBatch.Draw(crown, position + new Vector2(0, nameSize.Y + 10), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                position.X += playerPanelWidth;
            }

            // Draw images of darts
            DrawDartScore(spriteBatch);

            spriteBatch.End();
        }

        private Vector2 DrawSerialPortStatus(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(serialDisconnected.Width, serialDisconnected.Height) * Vector2.UnitX;
            if (!SuperDarts.SerialManager.IsPortOpen)
            {
                spriteBatch.Draw(serialDisconnected, new Vector2(SuperDarts.Viewport.Width - 20, 20) - offset, Color.White);
            }
            else
            {
                serialAnimation.Draw(spriteBatch, new Vector2(SuperDarts.Viewport.Width - 20, 20) - offset, Vector2.Zero);
            }
            return offset;
        }

        public virtual void DrawDartScore(SpriteBatch spriteBatch, Vector2 dartSize, Vector2 position, bool vertical)
        {
            Vector2 orientation;

            if (vertical)
                orientation = Vector2.UnitY;
            else
                orientation = Vector2.UnitX;

            Vector2 dartTextureSize = new Vector2(DartTexture.Width, DartTexture.Height);
            Vector2 temp = position + dartSize * orientation * Mode.CurrentPlayerRound.Darts.Count;

            //Draw images
            for (int i = Mode.CurrentPlayerRound.Darts.Count; i < Mode.DartsPerRound; i++)
            {
                //Blinking dart
                Vector2 dartTexturePosition = temp + dartSize * 0.5f - dartTextureSize * 0.5f;
                if (i == Mode.CurrentPlayerRound.Darts.Count)
                {
                    spriteBatch.Draw(solidDart, dartTexturePosition, Color.White * (float)(Math.Sin(elapsedTime * MathHelper.Pi / dartBlinkRate)));
                }

                spriteBatch.Draw(DartTexture, dartTexturePosition, Color.White);
                Vector2 numberOffset = new Vector2(numberTextures[0].Width, numberTextures[0].Height) * 0.5f;

                if (SuperDarts.Options.Debug)
                {
                    numberOffset = ScreenManager.Trebuchet24.MeasureString((i + 1).ToString()) * 0.5f;
                    spriteBatch.DrawString(ScreenManager.Trebuchet24, (i + 1).ToString(), temp + dartSize * 0.5f - numberOffset, Color.White);
                }

                spriteBatch.Draw(numberTextures[i], temp + dartSize * 0.5f - numberOffset, Color.White);

                temp += dartSize * orientation;
            }

            temp = position;
            Vector2 center = dartSize * 0.5f;

            // Draw dart score
            foreach (Dart dart in Mode.CurrentPlayerRound.Darts)
            {
                string text;
                Color c;
                dart.GetVerbose(out text, out c);

                Vector2 offset = ScreenManager.Trebuchet32.MeasureString(text) * 0.5f;
                TextBlock.DrawShadowed(spriteBatch, ScreenManager.Trebuchet32, text, c, temp + center - offset);
                temp += dartSize * orientation;
            }
        }

        public virtual void DrawDartScore(SpriteBatch spriteBatch)
        {
            Vector2 dartSize = new Vector2(DartTexture.Width, DartTexture.Height) * 1.4f;
            Vector2 position = new Vector2(SuperDarts.Viewport.Width * 0.5f - Mode.DartsPerRound * 0.5f * dartSize.X, SuperDarts.Viewport.Height * 0.68f);
            DrawDartScore(spriteBatch, dartSize, position, false);
        }

        Color getColor(Round r)
        {
            int score = Mode.GetScore(r);

            if (score >= 100)
                return Color.Cyan;
            if (score >= 150)
                return Color.Magenta;

            return Color.White;
        }


        /// <summary>
        /// Draws the score that is displayed for each round to left of the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void DrawRoundScores(SpriteBatch spriteBatch, Vector2 position)
        {
            string text = "";
            var font = ScreenManager.Trebuchet24;

            for (int i = 0; i < Mode.CurrentPlayer.Rounds.Count; i++)
            {
                Round r = Mode.CurrentPlayer.Rounds[i];

                int temp = Mode.GetScore(r);
                Color c = getColor(r);

                if (i == Mode.CurrentRoundIndex)
                    c = Color.Lerp(Color.LightYellow, Color.Yellow, (float)((Math.Sin(elapsedTime * 1.0f/500f) + 1.0f) / 2.0f));
                else if (i > Mode.CurrentRoundIndex)
                    c = Color.White * 0.33f;

                text = "R" + (i + 1).ToString() + "." + temp.ToString();
                TextBlock.DrawShadowed(spriteBatch, font, text, c, position);
                position.Y += font.LineSpacing * 0.8f;
            }
        }
    }
}
