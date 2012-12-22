using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public class AnimatedSprite
    {
        public bool Reverse = false;
        public int CurrentFrame = 0;
        public Rectangle SourceRectangle = Rectangle.Empty;
        public int Frames = 1;
        public float Fps = 30.0f;

        float elapsedTime = 0;
        int direction = 1;

        public Texture2D Texture;

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > 1 / Fps * 1000.0f)
            {
                elapsedTime = 0;
                CurrentFrame += direction;

                if (CurrentFrame > Frames - 1)
                {
                    if (Reverse)
                    {
                        direction = -1;
                        CurrentFrame = Frames - 1;
                    }
                    else
                        CurrentFrame = 0;
                }
                else if (CurrentFrame < 0)
                {
                    CurrentFrame = 0;
                    direction = 1;
                }
            }
        }


        public void Draw(SpriteBatch batch, Vector2 position, Vector2 offset)
        {
            int frameWidth = Texture.Width / Frames;
            SourceRectangle.X = CurrentFrame * frameWidth;

            batch.Draw(Texture, position - offset, SourceRectangle, Color.White);
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            Vector2 offset = new Vector2(SourceRectangle.Width, SourceRectangle.Height) * 0.5f;
            this.Draw(batch, position, offset);
        }
    }
}
