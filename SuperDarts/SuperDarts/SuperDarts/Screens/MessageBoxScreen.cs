using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public enum MessageBoxButtons
    {
        YesNo,
        YesNoCancel,
        Ok,
        Cancel
    }
    public class MessageBoxScreen : MenuScreen
    {
        public TextBlock Message { get; set; }

        MenuEntry meYes = new MenuEntry("Yes");
        MenuEntry meNo = new MenuEntry("No");
        MenuEntry meOk = new MenuEntry("Ok");
        MenuEntry meCancel = new MenuEntry("Cancel");

        public event EventHandler OnYes;
        public event EventHandler OnNo;
        public event EventHandler OnCancel;
        public event EventHandler OnOk;

        public MessageBoxScreen(string title, string message, MessageBoxButtons buttons)
            : base(title)
        {
            Message = new TextBlock(message);
            Message.Font = ScreenManager.Trebuchet24;
            StackPanel.Items.Insert(1, Message);

            meYes.OnSelected += new EventHandler(meYes_OnSelected);
            meNo.OnSelected += new EventHandler(meNo_OnSelected);
            meCancel.OnSelected += new EventHandler(meCancel_OnSelected);
            meOk.OnSelected += new EventHandler(meOk_OnSelected);

            meYes.Font = ScreenManager.Trebuchet24;
            meNo.Font = ScreenManager.Trebuchet24;
            meOk.Font = ScreenManager.Trebuchet24;
            meCancel.Font = ScreenManager.Trebuchet24;

            switch (buttons)
            {
                case MessageBoxButtons.YesNo:
                    MenuItems.AddItems(meYes, meNo);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    MenuItems.AddItems(meYes, meNo, meCancel);
                    break;
                case MessageBoxButtons.Ok:
                    MenuItems.AddItems(meOk);
                    break;
                case MessageBoxButtons.Cancel:
                    MenuItems.AddItems(meCancel);
                    break;
                default:
                    break;
            }

            Position = Vector2.One * 0.5f - 0.5f * new Vector2(StackPanel.Width, StackPanel.Height) / new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height);
        }

        public void meOk_OnSelected(object sender, EventArgs e)
        {
            if (OnOk != null)
                OnOk(this, null);

            ExitScreen(this, null);
        }

        public void meNo_OnSelected(object sender, EventArgs e)
        {
            if (OnNo != null)
                OnNo(this, null);

            ExitScreen(this, null);
        }

        public void meCancel_OnSelected(object sender, EventArgs e)
        {
            if (OnCancel != null)
                OnCancel(this, null);

            ExitScreen(this, null);
        }

        public void meYes_OnSelected(object sender, EventArgs e)
        {
            if (OnYes != null)
                OnYes(this, null);

            ExitScreen(this, null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            var bgAlpha = 0.8f;
            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(0, 0, SuperDarts.Viewport.Width, SuperDarts.Viewport.Height), Color.Black * bgAlpha);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
