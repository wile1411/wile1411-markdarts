using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SuperDarts
{
    public class InputState
    {
        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboarstState;

        public MouseState CurrentMouseState;
        MouseState lastMouseState;

        public bool[] CurrentBoardButtonStates;
        public bool[] LastBoardButtonStates;

        public void Update()
        {
            lastKeyboarstState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            LastBoardButtonStates = CurrentBoardButtonStates;
            CurrentBoardButtonStates = SuperDarts.SerialManager.ButtonStates;
        }

        public bool MouseMove
        {
            get
            {
                return CurrentMouseState.X != lastMouseState.X || CurrentMouseState.Y != lastMouseState.Y;
            }
        }

        public bool MouseClick
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            }
        }

        public bool MouseRightClick
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && lastKeyboarstState.IsKeyUp(key);
        }

        public bool MenuDown
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Down) && lastKeyboarstState.IsKeyUp(Keys.Down) ||
                    CurrentBoardButtonStates[0] == true && LastBoardButtonStates[0] == false;
            }
        }

        public bool MenuBack
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Back) && lastKeyboarstState.IsKeyUp(Keys.Back);
            }
        }

        public bool MenuCancel
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Escape) && lastKeyboarstState.IsKeyUp(Keys.Escape);
            }
        }

        public bool MenuEnter
        {
            get
            {
                return (currentKeyboardState.IsKeyDown(Keys.Enter) && lastKeyboarstState.IsKeyUp(Keys.Enter)) ||
                    (currentKeyboardState.IsKeyDown(Keys.Space) && lastKeyboarstState.IsKeyUp(Keys.Space)) ||
                    CurrentBoardButtonStates[2] == true && LastBoardButtonStates[2] == false;
            }
        }

        public bool MenuUp
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Up) && lastKeyboarstState.IsKeyUp(Keys.Up) ||
                    CurrentBoardButtonStates[1] == true && LastBoardButtonStates[1] == false;
            }
        }

        public bool MenuRight
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Right) && lastKeyboarstState.IsKeyUp(Keys.Right) ||
                    CurrentBoardButtonStates[4] == true && LastBoardButtonStates[4] == false;
            }
        }

        public bool MenuLeft
        {
            get
            {
                return currentKeyboardState.IsKeyDown(Keys.Left) && lastKeyboarstState.IsKeyUp(Keys.Left) ||
                    CurrentBoardButtonStates[3] == true && LastBoardButtonStates[3] == false;
            }
        }

    }
}
