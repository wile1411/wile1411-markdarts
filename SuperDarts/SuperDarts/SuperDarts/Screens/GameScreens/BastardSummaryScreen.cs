﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class BastardSummaryScreen : MessageBoxScreen
    {
        Bastard Mode;

        public BastardSummaryScreen(Bastard mode) : base("Throw Summary", "", MessageBoxButtons.Ok)
        {
            Mode = mode;
            this.OnOk += new EventHandler(BastardSummaryScreen_OnOk);
        }

        void BastardSummaryScreen_OnOk(object sender, EventArgs e)
        {
            ExitScreen(this, null);
            Mode.showPlayerChangeScreen();
        }

        public void UpdateText()
        {
            //IEnumerable<IGrouping<Player, BastardHit>> hitsPerPlayer = Mode.BastardHits.Where(hit => hit.Round == Mode.CurrentRoundIndex && hit.ThrownBy == Mode.CurrentPlayer).GroupBy(hit => hit.SegmentOwner);

            StringBuilder sb = new StringBuilder();

            /*foreach (IGrouping<Player, BastardHit> group in hitsPerPlayer)
            {
                if (group.Key != null) //Segment was owned by someone
                {
                    sb.Append(group.Key.Name + ": ");

                    int temp = 0;

                    foreach (BastardHit hit in group)
                    {
                        if (group.Key == Mode.CurrentPlayer)
                            temp -= hit.Dart.Multiplier;
                        else
                            temp += hit.Dart.Multiplier;
                    }

                    sb.Append(temp.ToString());
                    sb.AppendLine();
                }
                else
                {
                    IEnumerable<IGrouping<int, BastardHit>> temp = group.GroupBy(hit => hit.Dart.Segment);

                    foreach (IGrouping<int, BastardHit> hit in temp)
                    {
                        sb.AppendLine(hit.Key + ": " + hit.Sum(h => h.Dart.Multiplier).ToString());
                    }
                }
            }*/

            Message.Text = sb.ToString();
        }
    }
}
