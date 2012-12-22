using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class PartyMenuScreen : MenuScreen
    {
        MenuEntry Back = new MenuEntry("Back");
        MenuEntry Bastard = new MenuEntry("Bastard Darts");

        public PartyMenuScreen()
            : base("Custom")
        {
            Back.OnSelected += new EventHandler(ExitScreen);
            Bastard.OnSelected += new EventHandler(Bastard_OnSelected);
            MenuItems.AddItems(Bastard, Back);
        }

        void Bastard_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(new[] { 2, 4, 5 });
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(Bastard_PlayerSelect);
            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void Bastard_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new Bastard(players)));
        }
    }
}
