using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts.Screens.Menus
{
    public class HelpMenuScreen : MenuScreen
    {
        MenuEntry back;
        public HelpMenuScreen()
            : base("Help & About")
        {
            back = new MenuEntry("Back");
            back.OnSelected += new EventHandler(ExitScreen);

            MenuItems.AddItems(back);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Space - Select/Skip");
            sb.AppendLine("F5 - Toggle FPS");
            sb.AppendLine("F6 - Open dart input");

            StackPanel.Items.Insert(1, new TextBlock(sb.ToString()));
        }
    }
}
