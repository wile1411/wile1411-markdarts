using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class ZeroOneMenuScreen : MenuScreen
    {
        MenuEntry me301 = new MenuEntry("301");
        MenuEntry me401 = new MenuEntry("401");
        MenuEntry me501 = new MenuEntry("501");
        MenuEntry me701 = new MenuEntry("701");
        MenuEntry me901 = new MenuEntry("901");
        MenuEntry meBack = new MenuEntry("Back");

        public ZeroOneMenuScreen() : base("Zero One")
        {
            me301.OnSelected += new EventHandler(me301_OnSelected);
            me401.OnSelected += new EventHandler(me401_OnSelected);
            me501.OnSelected += new EventHandler(me501_OnSelected);
            me701.OnSelected += new EventHandler(me701_OnSelected);
            me901.OnSelected += new EventHandler(me901_OnSelected);

            meBack.OnSelected += new EventHandler(ExitScreen);

            MenuItems.AddItems(me301, me401, me501, me701, me901, meBack);
        }

        void me901_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(me901_PlayerSelect);
            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void me901_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new ZeroOne(players, 901) { MaxRounds = 12 }));
        }

        void me701_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(me701_PlayerSelect);

            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void me701_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new ZeroOne(players, 701) { MaxRounds = 12 }));
        }

        void me501_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(me501_PlayerSelect);

            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void me501_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new ZeroOne(players, 501)));
        }

        void me401_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(me401_PlayerSelect);

            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void me401_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new ZeroOne(players, 401)));
        }

        void me301_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(me301_PlayerSelect);

            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void me301_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new ZeroOne(players, 301)));
        }
    }
}
