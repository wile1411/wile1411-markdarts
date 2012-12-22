using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class StandardMenuScreen : MenuScreen
    {
        MenuEntry back = new MenuEntry("Back");
        MenuEntry ZeroOne = new MenuEntry("Zero One");
        MenuEntry CountUp = new MenuEntry("Count Up");
        MenuEntry Cricket = new MenuEntry("Cricket");

        public StandardMenuScreen()
            : base("Standard")
        {
            back.OnSelected += new EventHandler(ExitScreen);
            ZeroOne.OnSelected += new EventHandler(ZeroOne_OnSelected);
            CountUp.OnSelected += new EventHandler(CountUp_OnSelected);
            Cricket.OnSelected += new EventHandler(Cricket_OnSelected);

            MenuItems.AddItems(ZeroOne, CountUp, Cricket, back);
        }

        void Cricket_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new CricketMenuScreen());
        }

        void CountUp_OnSelected(object sender, EventArgs e)
        {
            PlayerSelectScreen screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = new PlayerSelectScreen.PlayerSelectDelegate(CountUp_PlayerSelect);
            SuperDarts.ScreenManager.AddScreen(screen);
        }

        void CountUp_PlayerSelect(int players)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new CountUp(players)));
        }

        void ZeroOne_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new ZeroOneMenuScreen());
        }

    }
}
