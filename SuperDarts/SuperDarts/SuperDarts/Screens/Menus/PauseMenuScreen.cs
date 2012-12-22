using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperDarts.Screens.Menus;

namespace SuperDarts
{
    public class PauseMenuScreen : MenuScreen
    {
        MenuEntry Return = new MenuEntry("Return");
        MenuEntry Quit = new MenuEntry("Quit");

        MenuEntry Unthrow = new MenuEntry("Unthrow Last Dart");
        MenuEntry Summary = new MenuEntry("Throw Summary");
        MenuEntry Options = new MenuEntry("Options");

        MenuEntry Help = new MenuEntry("Help & About");

        private GameplayScreen gameplayScreen;

        public PauseMenuScreen(GameplayScreen gameplayScreen)
            : base("Game Paused")
        {
            this.gameplayScreen = gameplayScreen;

            Return.OnSelected += new EventHandler(ExitScreen);

            //Check if there is a dart that we can remove
            if (!SuperDarts.Players.Any(x => x.Rounds.Any(y => y.Darts.Any())))
                Unthrow.Enabled = false;

            Unthrow.OnSelected += new EventHandler(Unthrow_OnSelected);
            Summary.OnSelected += new EventHandler(Summary_OnSelected);
            Options.OnSelected += new EventHandler(Options_OnSelected);
            Quit.OnSelected += new EventHandler(Quit_OnSelected);
            Help.OnSelected += new EventHandler(Help_OnSelected);

            MenuItems.AddItems(Return, Unthrow, Summary, Options, Help, Quit);
        }

        void Summary_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new ThrowSummaryScreen());
        }

        void Help_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new HelpMenuScreen());
        }

        void Options_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void Unthrow_OnSelected(object sender, EventArgs e)
        {
            MessageBoxScreen confirm = new MessageBoxScreen("Confirm", "Are you sure you want to remove\nthe last thrown dart?", MessageBoxButtons.YesNo);
            confirm.OnYes += new EventHandler(ConfirmUnthrow_OnAccept);
            SuperDarts.ScreenManager.AddScreen(confirm);
        }

        void ConfirmUnthrow_OnAccept(object sender, EventArgs e)
        {
            gameplayScreen.Mode.Unthrow();
            ExitScreen(this, null);
        }

        void Quit_OnSelected(object sender, EventArgs e)
        {
            MessageBoxScreen mbox = new MessageBoxScreen("Quit", "Are you sure you want to quit?", MessageBoxButtons.YesNo);
            mbox.OnYes += new EventHandler(mbox_OnAccept);
            SuperDarts.ScreenManager.AddScreen(mbox);
        }

        void mbox_OnAccept(object sender, EventArgs e)
        {
            gameplayScreen.ExitScreen(this, null);
            ExitScreen(this, null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            var bgAlpha = 0.8f;
            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(0, 0, SuperDarts.Viewport.Width, SuperDarts.Viewport.Height), Color.Black * bgAlpha);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
