using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class PracticeMenuScreen : MenuScreen
    {
        MenuEntry Back = new MenuEntry("Back");
        MenuEntry CountUp = new MenuEntry("Count Up");
        MenuEntry Stats = new MenuEntry("Practice History");

        public PracticeMenuScreen()
            : base("Practice")
        {
            Back.OnSelected += new EventHandler(ExitScreen);
            CountUp.OnSelected += new EventHandler(CountUp_OnSelected);
            Stats.OnSelected += new EventHandler(Stats_OnSelected);

            MenuItems.AddItems(CountUp, Stats, Back);
        }

        void Stats_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new PracticeHistoryScreen());
        }

        void CountUp_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new GameplayScreen(new CountUp(1) { MaxRounds = 8, DartsPerRound = 3, IsPractice = true }));
        }

    }
}
