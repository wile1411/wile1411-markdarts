using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperDarts
{
    public class SegmentMapScreen : MenuScreen
    {
        MenuEntry edit = new MenuEntry("Edit Bindings");
        MenuEntry bindAll = new MenuEntry("Bind All");
        MenuEntry clear = new MenuEntry("Clear All Bindings");
        MenuEntry back = new MenuEntry("Back");

        Dictionary<IntPair, IntPair> segmentMap;

        ContentManager content;

        Texture2D mapTexture;
        Texture2D segmentTexture;
        Texture2D doubleTexture;
        Texture2D tripleTexture;
        Texture2D singleBullTexture;
        Texture2D doubleBullTexture;

        bool drawCoords = false;
        IntPair mouseOver;
        float distance = 0;
        int[] segmentRotation = new int[] { 1, 8, 10, 3, 19, 5, 12, 14, 17, 6, 15, 18, 4, 16, 7, 13, 9, 2, 11, 0 };
        int[] segmentOrder = new int[] { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };
        Vector2 mouseVector = Vector2.Zero;
        BindSegmentScreen bindScreen;
        Texture2D[] texture;

        bool isEditing = false;
        bool hasMadeChanges = false;
        IntPair selectedSegment = new IntPair(0, 1);

        Vector2 boardPosition;

        public SegmentMapScreen(string title) : base(title)
        {
            edit.OnSelected += new EventHandler(edit_OnSelected);
            clear.OnSelected += new EventHandler(clear_OnSelected);
            back.OnSelected += new EventHandler(back_OnSelected);
            bindAll.OnSelected += new EventHandler(bindAll_OnSelected);

            MenuItems.AddItems(bindAll, edit, clear, back);

            if (SuperDarts.Options.SegmentMap.Any())
            {
                segmentMap = new Dictionary<IntPair, IntPair>(SuperDarts.Options.SegmentMap);
            }
            else
            {
                BuildSegmentMap();
            }
        }

        void bindAll_OnSelected(object sender, EventArgs e)
        {
            selectedSegment = new IntPair(0, 1);
            ShowBindSegmentScreen(GetSelectedSegment(), true);
        }

        void edit_OnSelected(object sender, EventArgs e)
        {
            isEditing = true;
        }

        void clear_OnSelected(object sender, EventArgs e)
        {
            MessageBoxScreen mb = new MessageBoxScreen("Confirm", "Are you sure you want to clear all bindings?", MessageBoxButtons.YesNo);
            mb.OnYes += new EventHandler(mbClear_OnYes);
            SuperDarts.ScreenManager.AddScreen(mb);
        }

        void mbClear_OnYes(object sender, EventArgs e)
        {
            BuildSegmentMap();
            hasMadeChanges = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(SuperDarts.ScreenManager.Game.Services, "Content");

            mapTexture = content.Load<Texture2D>(@"Images\SegmentMap");

            boardPosition = new Vector2(SuperDarts.Viewport.Width - mapTexture.Width * 0.5f, SuperDarts.Viewport.Height * 0.5f);

            segmentTexture = content.Load<Texture2D>(@"Images\Segment");
            tripleTexture = content.Load<Texture2D>(@"Images\Triple");
            doubleTexture = content.Load<Texture2D>(@"Images\Double");
            singleBullTexture = content.Load<Texture2D>(@"Images\SingleBull");
            doubleBullTexture = content.Load<Texture2D>(@"Images\DoubleBull");

            texture = new Texture2D[] { segmentTexture, doubleTexture, tripleTexture, singleBullTexture, doubleBullTexture };
        }

        void back_OnSelected(object sender, EventArgs e)
        {
            if (hasMadeChanges)
            {
                MessageBoxScreen mb = new MessageBoxScreen("Confirm", "Do you want to save the changes?", MessageBoxButtons.YesNoCancel);
                mb.OnYes += new EventHandler(mb_OnYes);
                mb.OnNo += new EventHandler(mb_OnNo);
                SuperDarts.ScreenManager.AddScreen(mb);
            }
            else
            {
                ExitScreen(this, null);
            }
        }

        void mb_OnNo(object sender, EventArgs e)
        {
            ExitScreen(this, null);
        }

        void mb_OnYes(object sender, EventArgs e)
        {
            SuperDarts.Options.SegmentMap = segmentMap;
            ExitScreen(this, null);
        }

        private void BuildSegmentMap()
        {
            segmentMap = new Dictionary<IntPair, IntPair>();

            for (int i = 1; i <= 20; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    segmentMap.Add(new IntPair(i, j), null);
                }
            }

            segmentMap.Add(new IntPair(25, 1), null);
            segmentMap.Add(new IntPair(25, 2), null);
        }

        public override void HandleInput(InputState inputState)
        {
            if (!isEditing)
            {
                base.HandleInput(inputState);
            }
            else
            {
                if (inputState.MenuCancel)
                {
                    isEditing = false;
                }
                if (inputState.MenuRight)
                {
                    selectedSegment.X++;
                }
                if (inputState.MenuLeft)
                {
                    selectedSegment.X--;
                }
                if (inputState.MenuUp)
                {
                    selectedSegment.Y++;
                }
                if (inputState.MenuDown)
                {
                    selectedSegment.Y--;
                }

                if (inputState.MenuEnter)
                {
                    ShowBindSegmentScreen(GetSelectedSegment(), false);
                }

                if (selectedSegment.X > 19)
                    selectedSegment.X = 0;
                if (selectedSegment.Y > 5)
                    selectedSegment.Y = 1;
                if (selectedSegment.X < 0)
                    selectedSegment.X = 19;
                if (selectedSegment.Y < 1)
                    selectedSegment.Y = 5;
            }

            Rectangle boardRectangle = new Rectangle((int)(boardPosition.X - mapTexture.Width * 0.5), (int)(boardPosition.Y - mapTexture.Height * 0.5), mapTexture.Width, mapTexture.Height);

            drawCoords = false;
            mouseOver = null;
            mouseVector = new Vector2(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y);

            if (boardRectangle.Contains(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y))
            {
                int dx = inputState.CurrentMouseState.X - (int)(boardPosition.X - mapTexture.Width * 0.5f);
                int dy = inputState.CurrentMouseState.Y - (int)(boardPosition.Y - mapTexture.Height * 0.5f);

                float rotation = (float)Math.Atan2(dy, dx);

                Vector2 tempVector = boardPosition - mouseVector;
                distance = tempVector.Length();
                tempVector.Normalize();

                Vector2 segmentVector = Vector2.UnitY;
                Matrix rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(18));

                if (distance < 350.0f)
                {
                    drawCoords = true;

                    if (distance < 15.0f)
                    {
                        mouseOver = new IntPair(25, 2);
                    }
                    else if (distance < 40.0f)
                    {
                        mouseOver = new IntPair(25, 1);
                    }
                    else
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            float angle = (float)Math.Acos(Vector2.Dot(segmentVector, tempVector));

                            float TRIPLE_RADIUS = 190.0f;
                            float DOUBLE_RADIUS = 320.0f;

                            if (Math.Abs(angle) < MathHelper.ToRadians(9.0f))
                            {
                                mouseOver = new IntPair(segmentOrder[i], 0);

                                if (distance > TRIPLE_RADIUS && distance < TRIPLE_RADIUS + 30.0f) // Triple
                                {
                                    mouseOver.Y = 3;
                                }
                                else if (distance > DOUBLE_RADIUS) //Double
                                {
                                    mouseOver.Y = 2;
                                }
                                else
                                {
                                    mouseOver.Y = 1;
                                }

                                break;
                            }

                            segmentVector = Vector2.Transform(segmentVector, rotationMatrix);
                        }
                    }

                    if (inputState.MouseClick)
                    {
                        if (mouseOver != null)
                        {
                            ShowBindSegmentScreen(mouseOver, false);
                        }
                    }
                }


            }
        }

        private IntPair GetSelectedSegment()
        {
            IntPair segment = new IntPair(segmentOrder[selectedSegment.X], selectedSegment.Y);

            if (segment.Y == 4 || segment.Y == 5)
            {
                segment.X = 25;
                segment.Y = segment.Y - 3;
            }

            return segment;
        }

        void ShowBindSegmentScreen(IntPair segment, bool bindingAll)
        {
            string prefix = "Single";

            if (segment.Y == 2)
                prefix = "Double";
            else if (segment.Y == 3)
                prefix = "Triple";

            string text = "Press " + prefix + " " + segment.X;

            bindScreen = new BindSegmentScreen(text, segment);
            bindScreen.OnDartHit += new EventHandler(bindScreen_OnDartHit);
            bindScreen.OnClear += new EventHandler(bindScreen_OnClear);

            if (bindingAll)
            {
                bindScreen.OnDartHit += new EventHandler(bindScreen_BindNext);
                bindScreen.OnClear += new EventHandler(bindScreen_BindNext);
                bindScreen.OnCancel += new EventHandler(bindScreen_BindNext);
            }

            SuperDarts.ScreenManager.AddScreen(bindScreen);
        }

        void bindScreen_BindNext(object sender, EventArgs e)
        {
            bool temp = true;

            if (selectedSegment.Y < 4)
            {
                selectedSegment.X++;
                if (selectedSegment.X > 19)
                {
                    selectedSegment.X = 0;
                    selectedSegment.Y++;
                }
            }
            else
            {
                selectedSegment.Y++;
                temp = false;
            }

            ShowBindSegmentScreen(GetSelectedSegment(), temp);
        }

        void bindScreen_OnClear(object sender, EventArgs e)
        {
            segmentMap[bindScreen.SelectedSegment] = null;
            hasMadeChanges = true;
        }

        void bindScreen_OnDartHit(object sender, EventArgs e)
        {
            segmentMap[bindScreen.SelectedSegment] = bindScreen.SegmentCoordinates;
            hasMadeChanges = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            
            Vector2 position;
            Vector2 offset;
            string text = "";

            offset = new Vector2(mapTexture.Width, mapTexture.Height) * 0.5f;
            position = boardPosition;
            spriteBatch.Draw(mapTexture, position - offset, Color.White);
            float rotation = 0;
            int index = 1;

            Dictionary<IntPair, IntPair> temp = new Dictionary<IntPair, IntPair>(segmentMap);

            foreach (KeyValuePair<IntPair, IntPair> p in temp)
            {
                Color c = Color.Green * 0.33f;

                if (p.Value == null)
                {
                    c = Color.White * 0.33f;
                }

                rotation = 0;

                index = 0;
                if (p.Key.X == 25)
                {
                    index = 2 + p.Key.Y;
                }
                else
                {
                    index = p.Key.Y - 1;
                    rotation = segmentRotation[p.Key.X - 1];
                }
                
                offset = new Vector2(mapTexture.Width, mapTexture.Height) * 0.5f;
                spriteBatch.Draw(texture[index], position, null, c, MathHelper.ToRadians(rotation * 18.0f), offset, 1.0f, SpriteEffects.None, 0); 
            }

            if (mouseOver != null)
            {
                if (segmentMap[mouseOver] != null)
                {
                    text = segmentMap[mouseOver].ToString();
                }
                else
                {
                    text = "Not Set";
                }

                position = mouseVector + new Vector2(80, 40);
                offset = ScreenManager.Trebuchet24.MeasureString(text) * 0.5f;

                if (drawCoords)
                    spriteBatch.DrawString(ScreenManager.Trebuchet24, text, position - offset, Color.White);
            }

            if (isEditing)
            {
                position = boardPosition;
                offset = new Vector2(mapTexture.Width, mapTexture.Height) * 0.5f;
                rotation = segmentRotation[segmentOrder[selectedSegment.X] - 1];
                index = selectedSegment.Y - 1;
                
                if(selectedSegment.Y == 4)
                    index = 3;
                else if(selectedSegment.Y == 5)
                    index = 4;

                spriteBatch.Draw(texture[index], position, null, Color.Yellow * 0.33f, MathHelper.ToRadians(rotation * 18.0f), offset, 1.0f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }
    }
}
