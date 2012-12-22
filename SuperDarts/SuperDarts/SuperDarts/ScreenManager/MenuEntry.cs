using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public enum MenuEntryIcon
    {
        None,
        Arrow,
        PlusMinus,
        Check,
        Cancel
    }

    public class MenuEntry : TextBlock
    {
        public MenuEntryIcon Icon = MenuEntryIcon.None;

        public event EventHandler OnSelected;
        public event EventHandler OnCanceled;
        public event EventHandler OnMenuLeft;
        public event EventHandler OnMenuRight;

        public bool Enabled = true;

        public MenuEntry(string text) : base(text)
        {
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            HandleKeyboardInput(input);
        }

        public void MenuLeft()
        {
            if (OnMenuLeft != null && Enabled)
                OnMenuLeft(this, null);
        }

        public void MenuRight()
        {
            if (OnMenuRight != null && Enabled)
                OnMenuRight(this, null);
        }

        public void Select()
        {
            if (OnSelected != null && Enabled)
            {
                SuperDarts.SoundManager.PlaySound(SoundCue.MenuEnter);
                OnSelected(this, null);
            }
        }

        public void Cancel()
        {
            if (OnCanceled != null && Enabled)
            {
                SuperDarts.SoundManager.PlaySound(SoundCue.MenuBack);
                OnCanceled(this, null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha)
        {
            var color = this.Color;
            if (!Enabled)
                this.Color = color * 0.33f;
            base.Draw(spriteBatch, position, transitionAlpha);
            this.Color = color;
        }

        void HandleKeyboardInput(InputState input)
        {
            if (input.MenuLeft)
            {
                MenuLeft();
            }

            if (input.MenuRight)
            {
                MenuRight();
            }

            if (input.MenuEnter)
            {
                Select();
            }

            if (input.MenuBack)
            {
                Cancel();
            }
        }
    }
}
