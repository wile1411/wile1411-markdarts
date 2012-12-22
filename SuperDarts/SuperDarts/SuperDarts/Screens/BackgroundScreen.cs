using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class BackgroundScreen : GameScreen
    {
        ContentManager content;
        Texture2D background;

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(SuperDarts.ScreenManager.Game.Services, "Content");
            }

            background = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\MenuBackground");
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, SuperDarts.Viewport.Width, SuperDarts.Viewport.Height), Color.White);
            string text = "Martin Persson 2012-12-21, www.martinpersson.org";

            Vector2 temp = ScreenManager.Arial12.MeasureString(text) * 0.5f;
            Vector2 offset = new Vector2((int)temp.X, (int)temp.Y);
            Vector2 position = new Vector2(SuperDarts.Viewport.Width * 0.5f, SuperDarts.Viewport.Height - ScreenManager.Arial12.MeasureString(text).Y);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset, Color.White);
            spriteBatch.End();
        }
    }
}
