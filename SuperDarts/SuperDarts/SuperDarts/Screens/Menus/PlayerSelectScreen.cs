using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class PlayerSelectScreen : MenuScreen
    {
        public delegate void PlayerSelectDelegate(int players);
        public PlayerSelectDelegate OnPlayerSelect;

        public PlayerSelectScreen(int maxPlayers)
            : base("Select Number of Players")
        {
            Initialize(1, maxPlayers);
        }

        public PlayerSelectScreen(int minPlayers, int maxPlayers)
            : base("Select Number of Players")
        {
            Initialize(minPlayers, maxPlayers);
        }

        public PlayerSelectScreen(int[] players)
            : base("Select Number of Players")
        {
            Initialize(players);
        }

        void Initialize(int[] players)
        {
            foreach (var i in players)
            {
                DialMenuEntry entry = new DialMenuEntry(i, "Player");
                entry.OnSelected += new EventHandler(Entry_OnSelected);
                MenuItems.Items.Add(entry);
            }

            MenuEntry back = new MenuEntry("Back");
            back.OnSelected += new EventHandler(ExitScreen);
            MenuItems.Items.Add(back);
        }

        void Initialize(int minPlayers, int maxPlayers)
        {
            Initialize(Enumerable.Range(minPlayers, maxPlayers - minPlayers + 1).ToArray());
        }

        void Entry_OnSelected(object sender, EventArgs e)
        {
            DialMenuEntry entry = (DialMenuEntry)sender;

            if (OnPlayerSelect != null)
                OnPlayerSelect((int)entry.Value);
        }
    }
}
