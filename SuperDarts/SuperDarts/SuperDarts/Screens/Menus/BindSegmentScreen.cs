using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDarts
{
    public class BindSegmentScreen : MessageBoxScreen
    {
        public IntPair SegmentCoordinates;

        public event EventHandler OnDartHit;
        public event EventHandler OnClear;

        MenuEntry clear = new MenuEntry("Clear");
        public IntPair SelectedSegment;

        public BindSegmentScreen(string text, IntPair selectedSegment) : base("Bind Segments", text, MessageBoxButtons.Cancel)
        {
            clear.OnSelected += new EventHandler(clear_OnSelected);

            SelectedSegment = selectedSegment;

            MenuItems.Items.Add(clear);

            MenuItems.Items.Reverse(); //Want clear to end up at the top of the list

            SuperDarts.SerialManager.OnDartRegistered = null;
            SuperDarts.SerialManager.OnDartHit = new SerialManager.DartHitDelegate(HandleDart);
        }

        void clear_OnSelected(object sender, EventArgs e)
        {
            if (OnClear != null)
                OnClear(this, null);

            ExitScreen(this, null);
        }

        void HandleDart(IntPair coords)
        {
            SegmentCoordinates = coords;

            if (OnDartHit != null)
                OnDartHit(this, null);

            ExitScreen(this, null);
        }
    }
}
