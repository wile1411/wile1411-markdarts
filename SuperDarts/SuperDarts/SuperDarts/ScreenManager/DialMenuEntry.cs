using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class DialMenuEntry : MenuEntry
    {
        public object Value { get; set; }

        float spacing = 15.0f;

        public DialMenuEntry(object value, string text)
            : base(text)
        {
            Value = value;
        }

        public override void Draw(SpriteBatch batch, Vector2 position, float transitionAlpha)
        {
            base.Draw(batch, position, transitionAlpha);

            Vector2 temp = position + new Vector2(spacing + Width, 0);

            batch.DrawString(Font, Value.ToString(), temp + Vector2.One, Color.Black * transitionAlpha);
            batch.DrawString(Font, Value.ToString(), temp, Color * transitionAlpha);
        }
    }
}
