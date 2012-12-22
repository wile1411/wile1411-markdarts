using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public class BastardView : BaseView
    {
        Dartboard dartboard = new Dartboard();

        public BastardView(Bastard mode)
            : base(mode)
        {
            foreach (KeyValuePair<int, Player> p in mode.PlayerSegments)
            {
                dartboard.ColorSegment(p.Key, p.Value.Color * 0.33f);
            }

            dartboard.Scale = 0.6f;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            dartboard.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            dartboard.Position = new Vector2(SuperDarts.Viewport.Width * 0.5f, SuperDarts.Viewport.Height * 0.366f);
            dartboard.Draw(spriteBatch);

            spriteBatch.Begin();
            DrawRoundScores(spriteBatch, new Vector2(20, SuperDarts.Viewport.Height * 0.33f));
            spriteBatch.End();
        }
    }
}
