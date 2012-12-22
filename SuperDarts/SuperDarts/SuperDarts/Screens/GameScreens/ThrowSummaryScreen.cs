using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class ThrowSummaryScreen : MenuScreen
    {
        MenuEntry back = new MenuEntry("Back");

        public ThrowSummaryScreen()
            : base("Throw Summary")
        {
            back.OnSelected += new EventHandler(back_OnSelected);

            MenuItems.AddItems(back);
            Position.Y = 0.8f;
        }

        void back_OnSelected(object sender, EventArgs e)
        {
            this.ExitScreen(this, null);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            float spacing = 12.0f;
            float width = SuperDarts.Viewport.Width / SuperDarts.Players.Count * 0.8f;
            Vector2 position = new Vector2(SuperDarts.Viewport.Width * 0.5f - width * SuperDarts.Players.Count * 0.5f, SuperDarts.Viewport.Height * 0.2f);
            var font = ScreenManager.Trebuchet24;

            for (int i = 0; i < SuperDarts.Players.Count; i++)
            {
                TextBlock.DrawShadowed(spriteBatch, font, SuperDarts.Players[i].Name, SuperDarts.Players[i].Color, position);
                position.Y += font.MeasureString(SuperDarts.Players[i].Name).Y + spacing;

                for (int j = 0; j < SuperDarts.Players[i].Rounds.Count; j++)
                {
                    string text = "";
                    if(i == 0)
                        text += "R" + (j + 1).ToString() + ".";

                    for (int k = 0; k < SuperDarts.Players[i].Rounds[j].Darts.Count; k++)
                    {
                        switch (SuperDarts.Players[i].Rounds[j].Darts[k].Multiplier)
                        {
                            case 2:
                                text += "D";
                                break;
                            case 3:
                                text += "T";
                                break;
                        }

                        text += SuperDarts.Players[i].Rounds[j].Darts[k].Segment.ToString();

                        if (k != SuperDarts.Players[i].Rounds[j].Darts.Count - 1)
                            text += ",";
                    }

                    TextBlock.DrawShadowed(spriteBatch, font, text, Color.White, position);
                    position.Y += font.LineSpacing + spacing;
                }

                position.X += width + spacing;
                position.Y = SuperDarts.Viewport.Height * 0.2f;
            }

            spriteBatch.End();
        }
    }
}
