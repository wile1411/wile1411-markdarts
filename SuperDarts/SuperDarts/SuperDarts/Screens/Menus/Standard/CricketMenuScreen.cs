using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class CricketMenuScreen : MenuScreen
    {
        MenuEntry meStandard = new MenuEntry("Standard Cricket");
        MenuEntry meCutThroat = new MenuEntry("Cut Throat Cricket");
        MenuEntry meRandom = new MenuEntry("Random Cricket");
        MenuEntry mePick = new MenuEntry("Pick Throat Cricket");
        MenuEntry meHidden = new MenuEntry("Hidden Cricket");
        MenuEntry meBack = new MenuEntry("Back");

        public CricketMenuScreen() : base("Cricket")
        {
            meStandard.OnSelected += new EventHandler(meStandard_OnSelected);
            meCutThroat.OnSelected += new EventHandler(meCutThroat_OnSelected);
            meRandom.OnSelected += new EventHandler(meRandom_OnSelected);
            mePick.OnSelected += new EventHandler(mePick_OnSelected);
            meHidden.OnSelected += new EventHandler(meHidden_OnSelected);
            meBack.OnSelected += new EventHandler(ExitScreen);

            MenuItems.AddItems(meStandard, meBack); //meCutThroat, meRandom, mePick, meHidden,
        }

        void meHidden_OnSelected(object sender, EventArgs e)
        {
        }

        void mePick_OnSelected(object sender, EventArgs e)
        {
        }

        void meRandom_OnSelected(object sender, EventArgs e)
        {
        }

        void meCutThroat_OnSelected(object sender, EventArgs e)
        {
        }

        void meStandard_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(2);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(meStandard_PlayerSelect);
            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void meStandard_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new Cricket(players)));
        }
    }
}
