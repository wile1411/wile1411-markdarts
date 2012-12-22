using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    /// <summary>
    /// This is basically a simplified version of the ScreenManager that comes with
    /// the GameStateManagement sample found here:
    /// http://create.msdn.com/en-US/education/catalog/sample/game_state_management
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate;
        InputState inputState = new InputState();

        public static SpriteFont Arial12;
        public static SpriteFont Arial36;
        public static SpriteFont Arial48;
        public static SpriteFont Arial60;
        public static SpriteFont Arial64;
        public static SpriteFont Trebuchet22;
        public static SpriteFont Trebuchet24;
        public static SpriteFont Trebuchet32;
        public static SpriteFont Trebuchet48;
        public static SpriteFont Trebuchet64;


        public static Texture2D BlankTexture;
        public static Texture2D ButtonTexture;
        public static Texture2D ArrowIcon;
        public static Texture2D SelectedButtonTexture;
        public static Texture2D MessageBackground;

        SpriteBatch spriteBatch;
        ContentManager _content;

        bool _initialized = false;

        public ScreenManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            _content = this.Game.Content;

            Arial12 = _content.Load<SpriteFont>(@"Fonts\Arial12");
            Arial36 = _content.Load<SpriteFont>(@"Fonts\Arial36");
            Arial48 = _content.Load<SpriteFont>(@"Fonts\Arial48");
            Arial60 = _content.Load<SpriteFont>(@"Fonts\Arial60");
            Arial64 = _content.Load<SpriteFont>(@"Fonts\Arial64");
            Trebuchet22 = _content.Load<SpriteFont>(@"Fonts\Trebuchet22");
            Trebuchet24 = _content.Load<SpriteFont>(@"Fonts\Trebuchet24");
            Trebuchet32 = _content.Load<SpriteFont>(@"Fonts\Trebuchet32");
            Trebuchet48 = _content.Load<SpriteFont>(@"Fonts\Trebuchet48");
            Trebuchet64 = _content.Load<SpriteFont>(@"Fonts\Trebuchet64");

            BlankTexture = _content.Load<Texture2D>(@"Images\Blank");

            ButtonTexture = _content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\Button");
            ArrowIcon = _content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\Icons\Arrow");
            SelectedButtonTexture = _content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\ButtonSelected");
            MessageBackground = _content.Load<Texture2D>(@"Images\" + SuperDarts.Options.Theme + @"\" + "MessageBackground");

            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screens.Add(screen);

            if (_initialized)
                screen.LoadContent();
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            inputState.Update();

            screensToUpdate = new List<GameScreen>(screens);

            bool isCoveredByOtherScreen = false;

            for (int i = screensToUpdate.Count - 1; i >= 0; i--)
            {
                GameScreen screen = screensToUpdate[i];

                screen.Update(gameTime, isCoveredByOtherScreen);

                if (!isCoveredByOtherScreen)
                {
                    if(Game.IsActive)
                        screen.HandleInput(inputState);

                    isCoveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            List<GameScreen> screensToDraw = new List<GameScreen>(screens);

            screensToDraw.Reverse();

            while (screensToDraw.Any())
            {
                GameScreen screen = screensToDraw[screensToDraw.Count - 1];
                screensToDraw.RemoveAt(screensToDraw.Count - 1);
                screen.Draw(spriteBatch);
            }
        }
    }
}
