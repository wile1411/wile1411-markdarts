using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public class Dartboard
    {
        public Vector2 Position = Vector2.Zero;
        public float Scale = 1.0f;

        public static int[] SegmentRotation = new int[] { 1, 8, 10, 3, 19, 5, 12, 14, 17, 6, 15, 18, 4, 16, 7, 13, 9, 2, 11, 0 };
        public static int[] SegmentOrder = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };

        public Dictionary<IntPair, Color> SegmentColor = new Dictionary<IntPair, Color>();

        Texture2D mapTexture;
        Texture2D segmentTexture;
        Texture2D doubleTexture;
        Texture2D tripleTexture;
        Texture2D singleBullTexture;
        Texture2D doubleBullTexture;

        Texture2D[] textures;

        float TRIPLE_RADIUS = 190.0f;
        float DOUBLE_RADIUS = 320.0f;
        float SINGLE_BULLSEYE_RADIUS = 15.0f;
        float DOUBLE_BULLSEYE_RADIUS = 40.0f;
        float SEGMENT_DEGREES = 18;

        public delegate void SegmentClickedDelegate(IntPair segment);
        public event SegmentClickedDelegate OnSegmentClicked;

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2(mapTexture.Width, mapTexture.Height) * 0.5f;
            }
        }

        public Dartboard()
        {

        }

        public void LoadContent(ContentManager content)
        {
            mapTexture = content.Load<Texture2D>(@"Images\SegmentMap");
            segmentTexture = content.Load<Texture2D>(@"Images\Segment");
            tripleTexture = content.Load<Texture2D>(@"Images\Triple");
            doubleTexture = content.Load<Texture2D>(@"Images\Double");
            singleBullTexture = content.Load<Texture2D>(@"Images\SingleBull");
            doubleBullTexture = content.Load<Texture2D>(@"Images\DoubleBull");

            textures = new Texture2D[] { segmentTexture, doubleTexture, tripleTexture, singleBullTexture, doubleBullTexture };
        }

        IntPair highlight = null;

        public void HandleInput(InputState input)
        {
            IntPair segment = GetSegment(new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y));
            if (segment != null)
            {
                if (input.MouseClick)
                {
                    if (OnSegmentClicked != null)
                    {
                        OnSegmentClicked(segment);
                    }
                }
            }

            highlight = segment;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix Transform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(new Vector3(Position, 0));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Transform);

            spriteBatch.Draw(mapTexture, Vector2.Zero, null, Color.White, 0, TextureCenter, 1.0f, SpriteEffects.None, 0);

            float rotation;
            Texture2D texture;

            foreach (KeyValuePair<IntPair, Color> p in SegmentColor)
            {
                GetTextureAndRotation(p.Key, out texture, out rotation);

                spriteBatch.Draw(texture, Vector2.Zero, null, p.Value, rotation, TextureCenter, 1.0f, SpriteEffects.None, 0);
            }

            if (highlight != null)
            {
                GetTextureAndRotation(highlight, out texture, out rotation);
                spriteBatch.Draw(texture, Vector2.Zero, null, Color.Yellow, rotation, TextureCenter, 1.0f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        private void GetTextureAndRotation(IntPair segment, out Texture2D texture, out float rotation)
        {
            if (segment.X == 25)
            {
                texture = textures[2 + segment.Y];
                rotation = 0;
            }
            else
            {
                texture = textures[segment.Y - 1];
                rotation = MathHelper.ToRadians(SegmentRotation[segment.X - 1] * SEGMENT_DEGREES);
            }
        }

        public IntPair GetSegment(Vector2 position)
        {
            Vector2 halfSize = this.TextureCenter * Scale;
            float distanceToCenter = Vector2.Distance(position, this.Position);

            if (distanceToCenter < (DOUBLE_RADIUS + 30.0f) * Scale)
            {
                if (distanceToCenter < SINGLE_BULLSEYE_RADIUS * Scale)
                {
                    return new IntPair(25, 2); // Double BullsEye
                }
                else if (distanceToCenter < DOUBLE_BULLSEYE_RADIUS * Scale)
                {
                    return new IntPair(25, 1); // Single BullsEye
                }
                else
                {
                    Vector2 segmentVector = Vector2.UnitY;

                    Matrix rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(SEGMENT_DEGREES));

                    int dx = (int)position.X - (int)this.Position.X;
                    int dy = (int)position.Y - (int)this.Position.Y;

                    float rotation = (float)Math.Atan2(dy, dx);

                    Vector2 tempVector = this.Position - position;
                    tempVector.Normalize();

                    for (int i = 0; i < 20; i++)
                    {
                        float angle = (float)Math.Acos(Vector2.Dot(segmentVector, tempVector));

                        if (Math.Abs(angle) < MathHelper.ToRadians(SEGMENT_DEGREES * 0.5f))
                        {
                            IntPair temp = new IntPair(SegmentOrder[i], 0);

                            if (distanceToCenter > TRIPLE_RADIUS * Scale && distanceToCenter < (TRIPLE_RADIUS + 30.0f) * Scale) // Triple
                            {
                                temp.Y = 3;
                            }
                            else if (distanceToCenter > DOUBLE_RADIUS * Scale) //Double
                            {
                                temp.Y = 2;
                            }
                            else
                            {
                                temp.Y = 1;
                            }

                            return temp;
                        }

                        segmentVector = Vector2.Transform(segmentVector, rotationMatrix);
                    }
                }
            }

            return null;
        }

        public void ColorSegment(int p, Color c)
        {
            for (int i = 0; i < 3; i++)
            {
                SegmentColor.Add(new IntPair(p, i + 1), c);
            }
        }
    }
}
