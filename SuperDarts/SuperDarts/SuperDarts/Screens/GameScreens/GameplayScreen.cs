using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SuperDarts
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;

        public GameMode Mode;

        Texture2D background;

        Dartboard dartboard;
        bool showDartboard = false;

        Dartboard.SegmentClickedDelegate segmentClickedDelegate;
        SerialManager.DartRegisteredDelegate dartRegisteredDelegate;

        public GameplayScreen(GameMode mode)
        {
            mode.GameplayScreen = this;
            Mode = mode;
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(SuperDarts.ScreenManager.Game.Services, @"Content");
            }

            background = content.Load<Texture2D>(@"Images\Backgrounds\AbstractBackground"); // SuperDarts.Options.Theme

            Mode.LoadContent(content);

            SuperDarts.SoundManager.PlaySound(SoundCue.GameStart);

            dartRegisteredDelegate = new SerialManager.DartRegisteredDelegate(Mode.RegisterDart);
            SuperDarts.SerialManager.OnDartRegistered = dartRegisteredDelegate;
            SuperDarts.SerialManager.OnDartHit = null;

            dartboard = new Dartboard();
            dartboard.LoadContent(content);
            segmentClickedDelegate = new Dartboard.SegmentClickedDelegate(dartboard_OnSegmentClicked);
            dartboard.OnSegmentClicked += segmentClickedDelegate;

            dartboard.Scale = 0.5f;
        }

        void dartboard_OnSegmentClicked(IntPair segment)
        {
            Mode.RegisterDart(segment.X, segment.Y);
        }

        public override void UnloadContent()
        {
            SuperDarts.SerialManager.OnDartRegistered = null;
            base.UnloadContent();
            content.Unload();
        }

        public void Pause()
        {
            PauseMenuScreen pause = new PauseMenuScreen(this);
            SuperDarts.ScreenManager.AddScreen(pause);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (inputState.MenuCancel)
            {
                Pause();
            }

            if(inputState.IsKeyPressed(Keys.F6))
            {
                showDartboard = !showDartboard;
            }

            if (showDartboard)
            {
                dartboard.HandleInput(inputState);
            }

            if (inputState.IsKeyPressed(Keys.Space))
            {
                Mode.ForcePlayerChange();
            }
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);
            Mode.Update(gameTime, isCoveredByOtherScreen);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)SuperDarts.Viewport.Width, (int)SuperDarts.Viewport.Height), Mode.CurrentPlayer.Color);
            spriteBatch.End();

            Mode.Draw(spriteBatch);

            if (showDartboard)
            {
                dartboard.Position = new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height) * 0.5f;
                dartboard.Draw(spriteBatch);
            }
        }
    }
}
