using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class LineBrush
    {
        private Texture2D _lineTexture;
        private int _thickness;
        private Vector2 _origin;
        private Vector2 _normalizedDifference = Vector2.Zero;
        private Vector2 _xVector = new Vector2(1, 0);
        private float _theta;
        private float _rotation;
        private Vector2 _scale;
        public Color Color { get; set; }
        private Vector2 _difference;

        public LineBrush(int thickness)
        {
            _thickness = thickness;
            _origin = new Vector2(0, thickness / 2f + 1);
            Color = Color.White;
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            _lineTexture = new Texture2D(graphics, _thickness + 2, 1, false, SurfaceFormat.Color);

            int count = 2 * (_thickness + 2);
            Color[] colorArray = new Color[count];
            colorArray[0] = Color.White;
            colorArray[1] = Color.White;

            for (int i = 2; i < count - 2; i++)
            {
                colorArray[i] = Color.White;
            }

            colorArray[count - 2] = Color.White;
            colorArray[count - 1] = Color.White;

            _lineTexture.SetData(new Color[] { Color.White, Color.White, Color.White, Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint)
        {
            Vector2.Subtract(ref endPoint, ref startPoint, out _difference);
            CalculateRotation(ref _difference);
            CalculateScale(ref _difference);

            //Note: Scale is used to create the thickness
            spriteBatch.Draw(_lineTexture, startPoint, null, Color, _rotation, _origin, _scale, SpriteEffects.None, 0);
        }

        private void CalculateRotation(ref Vector2 difference)
        {
            Vector2.Normalize(ref difference, out _normalizedDifference);
            Vector2.Dot(ref _xVector, ref _normalizedDifference, out _theta);

            _theta = (float)Math.Acos(_theta);
            if (difference.Y < 0)
            {
                _theta = -_theta;
            }
            _rotation = _theta;
        }

        private void CalculateScale(ref Vector2 difference)
        {
            float desiredLength = difference.Length();
            _scale.X = desiredLength / _lineTexture.Width;
            _scale.Y = 1;
        }
    }
}
