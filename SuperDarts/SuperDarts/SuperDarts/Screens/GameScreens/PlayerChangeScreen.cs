using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public class PlayerChangeScreen : TimeoutScreen
    {

        AnimatedSprite playerChangeButton;

        public PlayerChangeScreen(string text, TimeSpan timeout)
            : base(text, timeout)
        {
        }

        public override void LoadContent()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            playerChangeButton = new AnimatedSprite();
            playerChangeButton.Texture = content.Load<Texture2D>(@"Images\PlayerChangeButton");
            playerChangeButton.Frames = 15;
            playerChangeButton.Reverse = true;
            playerChangeButton.Fps = 50.0f;
            playerChangeButton.SourceRectangle = new Rectangle(0, 0, 220, 299);
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            playerChangeButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            playerChangeButton.Draw(spriteBatch, new Vector2(SuperDarts.Viewport.Width * 0.85f, SuperDarts.Viewport.Height * 0.5f));
            spriteBatch.End();
        }
    }
}
