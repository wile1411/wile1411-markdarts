using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

//Unthrow last dart after bust in 01 causes issues

namespace SuperDarts
{
    public class SuperDarts : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager GraphicsDeviceManager;
        SpriteBatch spriteBatch;

        public static ScreenManager ScreenManager;
        public static Options Options;

        public static SerialManager SerialManager;
        public static SoundManager SoundManager;

        public static List<Player> Players = new List<Player>();

        FpsCounter fpsCounter;

        public static Viewport Viewport
        {
            get
            {
                return ScreenManager.Game.GraphicsDevice.Viewport;
            }
        }

        public SuperDarts()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Options = Options.Load();
            SerialManager = new SerialManager();

            GraphicsDeviceManager.PreferredBackBufferWidth = Options.Resolutions[Options.ResolutionIndex].Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = Options.Resolutions[Options.ResolutionIndex].Height;

            GraphicsDeviceManager.IsFullScreen = Options.FullScreen;

            SoundManager = new SoundManager(Content);
            ScreenManager = new ScreenManager(this);

            fpsCounter = new FpsCounter(this);

            Components.Add(ScreenManager);
            Components.Add(fpsCounter);

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());

            SuperDarts.SerialManager.OpenPort();

            checkSegmentMap();
        }

        private static void checkSegmentMap()
        {
            var boundSegments = Options.SegmentMap.Where(x => x.Value != null);
            var count = boundSegments.Count();
            if (count == 0)
            {
                var mb = new MessageBoxScreen("Segment Map Warning", "The segment map does not contain any bindings.\nEnter options to create the segment map.", MessageBoxButtons.Ok);
                ScreenManager.AddScreen(mb);
            }
            else if (count != 62)
            {
                var mb = new MessageBoxScreen("Segment Map Warning", "It seems like not all segments are bound\n(The segment map contains " + count + " values,\nbut there are 62 segments on a dart board).\nEnter options to create the segment map.", MessageBoxButtons.Ok);
                ScreenManager.AddScreen(mb);
            }

            foreach (var p1 in boundSegments)
            {
                foreach (var p2 in boundSegments)
                {
                    if (!p1.Key.Equals(p2.Key) && p1.Value.Equals(p2.Value))
                    {
                        Color c;
                        string text1, text2;
                        var dart1 = new Dart(null, p1.Key.X, p1.Key.Y);
                        dart1.GetVerbose(out text1, out c);
                        var dart2 = new Dart(null, p2.Key.X, p2.Key.Y);
                        dart2.GetVerbose(out text2, out c);
                        var mb = new MessageBoxScreen("Segment Map Warning", "The segment: " + text1 + " and " + text2 + "\ncontains the same coordinates: " + p1.Value.ToString() + "!", MessageBoxButtons.Ok);
                        ScreenManager.AddScreen(mb);
                    }
                }
            }
        }

        protected override void UnloadContent()
        {
            SerialManager.Close();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
