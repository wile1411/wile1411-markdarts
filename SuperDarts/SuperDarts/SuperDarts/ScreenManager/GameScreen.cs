using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public abstract class GameScreen
    {
        public bool IsCoveredByOtherScreen = false;

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            IsCoveredByOtherScreen = isCoveredByOtherScreen;
        }

        public virtual void ExitScreen(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.RemoveScreen(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void HandleInput(InputState inputState)
        {

        }
    }
}
