using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class TextBlock : GuiItem
    {
        public string Text { get; set; }

        public TextBlock(string text)
        {
            Text = text;
            Color = Color.White;
            Font = ScreenManager.Trebuchet24;
        }

        public override void HandleInput(InputState input)
        {
            // Not much to handle for a textblock
        }

        public SpriteFont Font
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha)
        {
            spriteBatch.DrawString(Font, Text, position + Vector2.One, Color.Black * transitionAlpha);
            spriteBatch.DrawString(Font, Text, position, Color * transitionAlpha);
        }

        public static void DrawShadowed(SpriteBatch spriteBatch, SpriteFont font, string text, Color color, Vector2 position)
        {
            spriteBatch.DrawString(font, text, position + Vector2.One, Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        public override int Width
        {
            get
            {
                return (int)Font.MeasureString(Text).X;
            }
        }

        public override int Height
        {
            get
            {
                return (int)Font.MeasureString(Text).Y;
            }
        }
    }
}
