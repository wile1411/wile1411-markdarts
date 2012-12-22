using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperDarts.Screens.Menus;

namespace SuperDarts
{
    public class MainMenuScreen : MenuScreen
    {
        MenuEntry Standard = new MenuEntry("Standard");
        MenuEntry Practice = new MenuEntry("Practice");
        MenuEntry Party = new MenuEntry("Custom");

        MenuEntry meOptions = new MenuEntry("Options");
        MenuEntry Quit = new MenuEntry("Quit");
        MenuEntry Help = new MenuEntry("Help & About");

        public MainMenuScreen()
            : base("Main Menu")
        {

            meOptions.OnSelected += new EventHandler(meOptions_OnSelected);
            Quit.OnSelected += new EventHandler(ExitScreen);
            Standard.OnSelected += new EventHandler(Standard_OnSelected);
            Practice.OnSelected += new EventHandler(Practice_OnSelected);
            Party.OnSelected += new EventHandler(Party_OnSelected);
            Help.OnSelected += new EventHandler(Help_OnSelected);

            //Practice.Enabled = false;
            //Removed practice 2012-12-21
            MenuItems.AddItems(Standard, Party, Practice, meOptions, Help, Quit);
        }

        void Help_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new HelpMenuScreen());
        }

        void Party_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new PartyMenuScreen());
        }

        void Practice_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new PracticeMenuScreen());
        }

        void Standard_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new StandardMenuScreen());
        }

        void meOptions_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void ConfirmQuit(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.Game.Exit();
        }

        public override void ExitScreen(object sender, EventArgs e)
        {
            MessageBoxScreen confirm = new MessageBoxScreen("Quit", "Are you sure you want to quit?", MessageBoxButtons.YesNo);
            confirm.OnYes += new EventHandler(ConfirmQuit);
            SuperDarts.ScreenManager.AddScreen(confirm);
        }

    }
}
