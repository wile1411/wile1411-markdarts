using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public abstract class GuiItem
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Margin Margin = Margin.Zero;

        public abstract void HandleInput(InputState input);
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha);
    }
}
