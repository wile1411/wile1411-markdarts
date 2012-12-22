using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SuperDarts
{
    public class PracticeHistoryScreen : MenuScreen
    {
        MenuEntry Back = new MenuEntry("Back");

        RecordManager recordManager;
        LineBrush lineBrush;

        public PracticeHistoryScreen() : base("Practice History")
        {
            this.Position = new Vector2(0.125f, 0.8f);

            Back.OnSelected += new EventHandler(ExitScreen);

            MenuItems.AddItems(Back);

            recordManager = RecordManager.Load();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            lineBrush = new LineBrush(2);
            lineBrush.LoadContent(SuperDarts.ScreenManager.GraphicsDevice);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (recordManager.Records.Count == 0)
            {
                var text = "There are no records saved";
                var offset = ScreenManager.Trebuchet24.MeasureString(text);
                TextBlock.DrawShadowed(spriteBatch, ScreenManager.Trebuchet24, text, Color.White, (new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height) - offset) * 0.5f);
            }
            else
            {
                drawGraph(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        private void drawGraph(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(0, 0, SuperDarts.Viewport.Width, SuperDarts.Viewport.Height), Color.Black);

            int padding = 60;

            int graphWidth = (int)(SuperDarts.Viewport.Width * 0.8f) - padding * 2;
            int graphHeight = (int)(SuperDarts.Viewport.Height * 0.6f) - padding * 2;
            int graphX = (int)(SuperDarts.Viewport.Width * 0.1f) + padding;
            int graphY = (int)(SuperDarts.Viewport.Width * 0.1f) + padding;

            int spacing = graphWidth / Math.Max((recordManager.Records.Count - 1), 1);

            int MinValue = recordManager.Records.Min(x => x.Score);
            int MaxValue = recordManager.Records.Max(x => x.Score);
            int dv = MaxValue - MinValue;

            if (dv == 0)
                dv = 1;

            int lastX = graphX;
            int lastY = graphY + graphHeight;

            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(graphX - padding, graphY - padding, graphWidth + padding * 2, graphHeight + padding * 2), Color.White);

            lineBrush.Color = Color.Black;

            lineBrush.Draw(spriteBatch, new Vector2(graphX, graphY), new Vector2(graphX + graphWidth, graphY));
            lineBrush.Draw(spriteBatch, new Vector2(graphX + graphWidth, graphY), new Vector2(graphX + graphWidth, graphY + graphHeight));
            lineBrush.Draw(spriteBatch, new Vector2(graphX + graphWidth, graphY + graphHeight), new Vector2(graphX, graphY + graphHeight));
            lineBrush.Draw(spriteBatch, new Vector2(graphX, graphY + graphHeight), new Vector2(graphX, graphY));

            lineBrush.Color = new Color(69, 142, 229);

            for (int i = 0; i < recordManager.Records.Count; i++)
            {
                int x = graphX + spacing * i;
                int y = graphY + graphHeight - graphHeight * (recordManager.Records[i].Score - MinValue) / dv;

                if (i > 0)
                    lineBrush.Draw(spriteBatch, new Vector2(lastX, lastY), new Vector2(x, y));

                spriteBatch.DrawString(ScreenManager.Arial12, recordManager.Records[i].Score.ToString(), new Vector2(x, y), Color.Black);

                Vector2 textSize = ScreenManager.Arial12.MeasureString(recordManager.Records[i].Date.ToShortDateString());

                Vector2 offset = textSize * 0.5f;

                offset.X = (int)offset.X;
                offset.Y = (int)offset.Y;

                float temp = textSize.X / spacing;

                int n = (int)Math.Round(temp);

                if (Math.Round(temp) < temp)
                    n += 1;

                if (n == 0)
                    n = 1;

                if (i % n == 0)
                    spriteBatch.DrawString(ScreenManager.Arial12, recordManager.Records[i].Date.ToShortDateString(), new Vector2(x, graphY + graphHeight + padding / 2) - offset, Color.Black);

                lastX = x;
                lastY = y;
            }
        }

        
    }
}
