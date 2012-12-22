using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperDarts
{
    public class FpsCounter : DrawableGameComponent
    {
        int frames = 0;
        float elapsedTime;
        float fps = 0.0f;

        SpriteFont font;
        ContentManager content;
        SpriteBatch spriteBatch;

        KeyboardState lastKeyBoardState;
        KeyboardState currentKeyboardState;

        bool visible = false;

        public FpsCounter(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            font = content.Load<SpriteFont>(@"Fonts\Trebuchet22");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            frames++;

            if (visible)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "FPS : " + fps.ToString(), new Vector2(20, 20) + Vector2.One, Color.Black);
                spriteBatch.DrawString(font, "FPS : " + fps.ToString(), new Vector2(20, 20), Color.White);
                spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lastKeyBoardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (lastKeyBoardState.IsKeyUp(Keys.F5) && currentKeyboardState.IsKeyDown(Keys.F5))
            {
                visible = !visible;
            }

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime >= 1.0f)
            {
                elapsedTime -= 1.0f;
                fps = frames;
                frames = 0;
            }
        }
    }
}