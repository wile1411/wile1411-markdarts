using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SuperDarts
{
    public enum StackPanelOrientation
    {
        Horizontal,
        Vertical
    }

    public enum MenuScreenState
    {
        Active,
        Exiting
    }

    public abstract class MenuScreen : GameScreen
    {
        public StackPanel StackPanel = new StackPanel();
        public StackPanel MenuItems = new StackPanel();
        int selectedEntry = -1;
        public int PaddingX = 24;
        public int PaddingY = 8;
        public Vector2 Position;

        float elapsedTime = 0;
        float blinkRate = 1000.0f;

        public MenuScreenState State = MenuScreenState.Active;
        float transitionAlpha = 0;

        public MenuScreen(string title)
        {
            this.Position = new Vector2(0.125f, 0.125f);
            StackPanel.Items.Add(new TextBlock(title) { Color = Color.LightBlue, Font = ScreenManager.Trebuchet32 });
            StackPanel.Items.Add(MenuItems);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (MenuItems.Items.Count > 0)
            {
                selectedEntry = 0;
                (MenuItems.Items[selectedEntry] as MenuEntry).Color = SuperDarts.Options.SelectedMenuItemForeground;
            }
        }

        public override void HandleInput(InputState inputState)
        {
            int oldSelectedEntry = selectedEntry;

            HandleKeyboardInput(inputState);
            HandleMouseInput(inputState);

            if (oldSelectedEntry != selectedEntry)
            {
                (MenuItems.Items[oldSelectedEntry] as MenuEntry).Color = SuperDarts.Options.MenuItemForeground;
                (MenuItems.Items[selectedEntry] as MenuEntry).Color = SuperDarts.Options.SelectedMenuItemForeground;
                SuperDarts.SoundManager.PlaySound(SoundCue.MenuSelect);
            }
        }

        /// <summary>
        /// Temporary(or not? :D) solution for handling mouse input
        /// </summary>
        /// <param name="inputState"></param>
        private void HandleMouseInput(InputState inputState)
        {
            int height = 0;

            for (int i = 0; i < StackPanel.Items.Count; i++)
            {
                if (StackPanel.Items[i] == MenuItems)
                {
                    for (int j = 0; j < MenuItems.Items.Count; j++)
                    {
                        Rectangle r = new Rectangle((int)(Position.X * SuperDarts.Viewport.Width), (int)(Position.Y * SuperDarts.Viewport.Height + height), MenuItems.Items[j].Width, MenuItems.Items[j].Height);
                        if (r.Contains(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y))
                        {
                            selectedEntry = j;

                            if (inputState.MouseClick)
                            {
                                (MenuItems.Items[j] as MenuEntry).Select();
                            }
                            else if (inputState.MouseRightClick)
                            {
                                (MenuItems.Items[j] as MenuEntry).Cancel();
                            }

                            break;
                        }
                        height += MenuItems.Items[j].Height;
                    }
                    break;
                }
                height += StackPanel.Items[i].Height;
            }
        }

        private void HandleKeyboardInput(InputState inputState)
        {
            MenuItems.Items[selectedEntry].HandleInput(inputState);

            if (inputState.MenuDown)
            {
                selectedEntry++;
            }
            if (inputState.MenuUp)
            {
                selectedEntry--;
            }

            if (selectedEntry > MenuItems.Items.Count - 1)
                selectedEntry = 0;
            if (selectedEntry < 0)
                selectedEntry = MenuItems.Items.Count - 1;

            if (inputState.MenuCancel)
            {
                CancelScreen();
            }
        }

        public virtual void CancelScreen()
        {
            SuperDarts.SoundManager.PlaySound(SoundCue.MenuBack);
            ExitScreen(this, null);
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            elapsedTime += dt;

            float transitionSpeed = 5f;
            switch (isCoveredByOtherScreen)
            {
                case true:
                    transitionAlpha -= dt * transitionSpeed;
                    break;
                case false:
                    transitionAlpha += dt * transitionSpeed;
                    break;
            }

            transitionAlpha = MathHelper.Clamp(transitionAlpha, 0, 1.0f);

            base.Update(gameTime, isCoveredByOtherScreen);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 screen = new Vector2(SuperDarts.Viewport.Width, SuperDarts.Viewport.Height);
            Vector2 temp = Position * screen;
            temp.X += MathHelper.Lerp(-Position.X * screen.X, 0, transitionAlpha);

            float sin = ((float)Math.Sin(elapsedTime * 5f) + 1.0f) / 2.0f;
            float alpha = sin * 0.8f + 0.2f;
            Color c = Color.Lerp(Color.White, SuperDarts.Options.SelectedMenuItemForeground, alpha);
            (MenuItems.Items[selectedEntry] as MenuEntry).Color = c;

            spriteBatch.Begin();
            StackPanel.Draw(spriteBatch, temp, transitionAlpha);
            spriteBatch.End();
        }
    }
}