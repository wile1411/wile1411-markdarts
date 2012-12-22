using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class CricketView : BaseView
    {
        SpriteFont tempFont;

        Texture2D[] markTexture = new Texture2D[4];
        Texture2D[] numberTextures = new Texture2D[6];
        Texture2D bullTexture;
        Texture2D closedTexture;

        float _scale = 0.8f;

        public CricketView(GameMode mode)
            : base(mode)
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);

            tempFont = ScreenManager.Trebuchet24;

            for (int i = 0; i < 4; i++)
            {
                markTexture[i] = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + @"Marks\Mark" + i.ToString());
            }

            for (int i = 0; i < 6; i++)
            {
                numberTextures[i] = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + @"CricketNumbers\" + (i + 15).ToString());
            }

            bullTexture = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + @"CricketNumbers\Bull");
            closedTexture = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + @"CricketNumbers\Closed");
        }

        public override void DrawDartScore(SpriteBatch spriteBatch)
        {
            Vector2 dartSize = new Vector2(DartTexture.Width, DartTexture.Height) * 1.4f;
            int y = (int)(SuperDarts.Viewport.Height * 0.5f - (Mode.DartsPerRound * dartSize.Y * 0.5f));
            base.DrawDartScore(spriteBatch, dartSize, new Vector2(SuperDarts.Viewport.Width * 0.75f, y), true);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            //Draw round scores
            DrawRoundMarks(spriteBatch);

            Vector2 position = new Vector2(SuperDarts.Viewport.Width * 0.5f, SuperDarts.Viewport.Height * 0.1f);
            float scale = 0.6f;

            Color tempColor = Color.Black;
            tempColor.A = 80;

            //Draw panels for marks
            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle((int)(position.X - markTexture[0].Width * 1.5f * scale),
                    (int)(position.Y - markTexture[0].Height * 0.5f * scale),
                    (int)(markTexture[0].Width * scale),
                    (int)(markTexture[0].Height * 7 * scale)), tempColor);

            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle((int)(position.X + markTexture[0].Width * 0.5f * scale),
                    (int)(position.Y - markTexture[0].Height * 0.5f * scale),
                    (int)(markTexture[0].Width * scale),
                    (int)(markTexture[0].Height * 7 * scale)), tempColor);

            Vector2 temp = Vector2.Zero;

            //Draw marks
            for (int i = 20; i >= 15; i--)
            {
                DrawMarks(spriteBatch, position, i);
                position.Y += markTexture[0].Height * scale;
            }

            // Same thing for the bulls eye
            temp = position - new Vector2(markTexture[0].Width * 1.5f * scale, markTexture[0].Height * 0.5f * scale);
            DrawMarks(spriteBatch, position, 25, "BULL", Color.Red);

            spriteBatch.End();
        }

        private void DrawRoundMarks(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(20, SuperDarts.Viewport.Height * 0.4f);
            Vector2 offset = Vector2.Zero;
            float scale = 0.25f;
            float spacing = markTexture[0].Width * scale + 10.0f;

            for (int i = 0; i < Mode.CurrentPlayer.Rounds.Count; i++)
            {
                Round round = Mode.CurrentPlayer.Rounds[i];
                Color c = Color.White;

                if (i == Mode.CurrentRoundIndex)
                    c = Color.Yellow;

                string text = "R" + (i + 1).ToString() + ": ";

                Vector2 textSize = tempFont.MeasureString(text);
                offset = new Vector2(markTexture[0].Width, markTexture[0].Height) * 0.5f * scale;
                string debug = "";
                for (int j = 0; j < round.Darts.Count; j++)
                {
                    int scoredMarks = (Mode as Cricket).ScoredMarks[round.Darts[j]];
                    debug += scoredMarks.ToString();

                    if (j < round.Darts.Count - 1)
                        debug += ", ";

                    spriteBatch.Draw(markTexture[Math.Min(scoredMarks, 3)], position + textSize * new Vector2(1, 0.5f) - offset + Vector2.UnitX * spacing * j + Vector2.UnitX * 10.0f, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }

                if (SuperDarts.Options.Debug)
                    text += debug;

                TextBlock.DrawShadowed(spriteBatch, tempFont, text, c, position);

                position.Y += tempFont.MeasureString(text).Y;
            }
        }

        private void DrawMarks(SpriteBatch spriteBatch, Vector2 position, int i, string label, Color color)
        {
            Vector2 temp = position - new Vector2(markTexture[0].Width * 1.5f * _scale, markTexture[0].Height * 0.5f * _scale);
            Vector2 offset = Vector2.Zero;
            Color c;

            for (int j = 0; j < SuperDarts.Players.Count; j++)
            {
                //draw marks
                int marks = (Mode as Cricket).Segments[i].Marks[j];

                c = Color.White;

                if (!(Mode as Cricket).Segments[i].IsAlive)
                {
                    c = Color.White * 0.33f;
                }

                if (marks > 0)
                    spriteBatch.Draw(markTexture[Math.Min(marks, 3)], temp, null, c, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);

                Vector2 center = new Vector2(markTexture[0].Width, markTexture[0].Height) * _scale * 0.5f;
                offset = tempFont.MeasureString(marks.ToString()) * 0.5f;

                if (SuperDarts.Options.Debug)
                {
                    spriteBatch.DrawString(tempFont, marks.ToString(), temp + center - offset + Vector2.One, Color.Black);
                    spriteBatch.DrawString(tempFont, marks.ToString(), temp + center - offset, Color.White);
                }

                temp.X += markTexture[0].Width * _scale * 2f;
            }

            if (SuperDarts.Options.Debug)
            {
                offset = tempFont.MeasureString(label) * 0.5f;
                spriteBatch.DrawString(tempFont, label, position - offset + Vector2.One, Color.Black);
                spriteBatch.DrawString(tempFont, label, position - offset, color);
            }

            offset = new Vector2(numberTextures[0].Width, numberTextures[0].Height) * 0.5f;
            Texture2D tex;

            if (i != 25)
                tex = numberTextures[i - 15];
            else
                tex = bullTexture;

            c = Color.White;

            if (!(Mode as Cricket).Segments[i].IsAlive)
            {
                c = Color.White * 0.33f;
            }

            spriteBatch.Draw(tex, position - offset, c);

            if (!(Mode as Cricket).Segments[i].IsAlive)
            {
                offset = new Vector2(closedTexture.Width, closedTexture.Height) * 0.5f;
                spriteBatch.Draw(closedTexture, position - offset, Color.White);
            }
        }

        private void DrawMarks(SpriteBatch spriteBatch, Vector2 position, int i)
        {
            DrawMarks(spriteBatch, position, i, i.ToString(), Color.White);
        }
    }
}
