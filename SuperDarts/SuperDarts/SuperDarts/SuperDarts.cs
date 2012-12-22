using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO.Ports;

//Unthrow last dart efter bust i 01 blir knas

namespace SuperDarts
{
    public class SuperDarts : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager GraphicsDeviceManager;
        SpriteBatch spriteBatch;

        public static ScreenManager ScreenManager;
        public static Options Options = Options.Load();

        public static SerialManager SerialManager = new SerialManager();
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());

            SuperDarts.SerialManager.OpenPort(); //temporary?
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //SerialManager.Close();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
