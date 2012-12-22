using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class ScoreView : BaseView
    {
        SpriteFont tempFont;

        int numberWidth = 190;
        int numberHeight = 300;
        Texture2D numbers;

        public ScoreView(GameMode mode)
            : base(mode)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            numbers = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + "Numbers");

            tempFont = ScreenManager.Trebuchet24;
        }

        /// <summary>
        /// Draws the big score panel in the center of the screen
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="n"></param>
        public void DrawScore(SpriteBatch batch, int n)
        {
            string temp = n.ToString();

            float scale = 1.0f;

            Vector2 offset = new Vector2(temp.Length * numberWidth * scale, numberHeight * scale) * 0.5f;
            Vector2 position = new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height) * 0.5f - offset;

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '-')
                {
                    batch.Draw(numbers, position, new Rectangle(10 * numberWidth, 0, numberWidth, numberHeight), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    position.X += numberWidth * scale;
                }
                else
                {
                    int index = 0;
                    int.TryParse(temp[i].ToString(), out index);

                    batch.Draw(numbers, position, new Rectangle(index * numberWidth, 0, numberWidth, numberHeight), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    position.X += numberWidth * scale;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            //Draw score
            DrawScore(spriteBatch, Mode.GetScore(Mode.CurrentPlayer));

            //Draw round scores
            DrawRoundScores(spriteBatch, new Vector2(20, SuperDarts.Viewport.Height * 0.33f));

            //Draw points per dart
            DrawPpd(spriteBatch);

            spriteBatch.End();
        }

        private void DrawPpd(SpriteBatch spriteBatch)
        {
            float totalScore;
            Vector2 offset;
            string text = "";
            Vector2 position = new Vector2(SuperDarts.Viewport.Width * 0.5f, SuperDarts.Viewport.Height * 0.125f);

            float ppd = 0;
            int thrown = Mode.CurrentPlayer.Rounds.Sum(r => r.Darts.Count);
            totalScore = Mode.CurrentPlayer.Rounds.Sum(r => Mode.GetScore(r));

            if (thrown > 0)
                ppd = totalScore / thrown;
            else
                ppd = 0;

            text = "Points Per Dart: " + ppd.ToString("0.00");
            offset = tempFont.MeasureString(text) * 0.5f;
            spriteBatch.DrawString(tempFont, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(tempFont, text, position - offset, Color.White);

            //Draw points per round
            float ppr = totalScore / (Mode.CurrentRoundIndex + 1);

            position.Y += offset.Y * 2.0f + 10.0f;
            text = "Points Per Round: " + ppr.ToString("0.00");
            offset = tempFont.MeasureString(text) * 0.5f;
            spriteBatch.DrawString(tempFont, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(tempFont, text, position - offset, Color.White);
        }
    }
}
