using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public class TimeoutScreen : GameScreen
    {
        public string Text { get; set; }

        SpriteFont _spriteFont = ScreenManager.Trebuchet64;
        public SpriteFont SpriteFont { get { return _spriteFont; } set { _spriteFont = value; } }

        public Color Color { get; set; }

        Vector2 _position = new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height) * 0.5f;
        public Vector2 Position { get { return _position; } set { _position = value; } }

        public TimeSpan Timeout { get; set; }

        public float ElapsedTime = 0;
        

        public delegate void TimeoutDelegate();
        public TimeoutDelegate OnTimeout;

        public ContentManager Content;

        public Color BackgroundColor { get; set; }

        Texture2D backgroundTexture;

        public TimeoutScreen(string text, TimeSpan timeout)
        {
            Text = text.ToUpper();

            Timeout = timeout;

            if (Timeout == TimeSpan.Zero)
                Timeout = TimeSpan.MaxValue;

            Position = new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height) * 0.5f;
            Color = Color.White;
            BackgroundColor = Color.White * 0.5f;
        }

        public void TimedOut()
        {
            ElapsedTime = 0;

            SuperDarts.ScreenManager.RemoveScreen(this);

            if (OnTimeout != null)
                OnTimeout();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if(Content == null)
                Content = new ContentManager(SuperDarts.ScreenManager.Game.Services, "Content");

            LoadContent(Content);
        }

        public virtual void LoadContent(ContentManager content)
        {
            backgroundTexture = content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + "MessageBackground");
        }

        public override void HandleInput(InputState inputState)
        {
            if (inputState.MenuCancel || inputState.MenuEnter)
            {
                TimedOut();
            }
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElapsedTime > Timeout.TotalMilliseconds)
            {
                TimedOut();
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, (int)(SuperDarts.Viewport.Height * 0.5f - backgroundTexture.Height * 0.5f), SuperDarts.Viewport.Width, backgroundTexture.Height - 25), BackgroundColor);

            Vector2 offset = _spriteFont.MeasureString(Text) * 0.5f;
            spriteBatch.DrawString(_spriteFont, Text, Position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(_spriteFont, Text, Position - offset, Color);
            spriteBatch.End();
        }
    }
}
