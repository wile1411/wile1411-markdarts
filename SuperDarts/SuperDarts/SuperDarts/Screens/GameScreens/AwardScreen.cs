using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperDarts
{
    public enum AwardCue
    {
        LowTon,
        HatTrick,
        ThreeInTheBlack,
        HighTon,
        ThreeInABed,
        TonEighty,
        WhiteHorse,
    }

    public class AwardScreen : GameScreen
    {
        VideoPlayer videoPlayer;
        Dictionary<AwardCue, Video> Awards = new Dictionary<AwardCue, Video>();
        bool loaded = false;

        public AwardScreen()
        {
            videoPlayer = new VideoPlayer();
        }

        public void LoadContent(ContentManager content)
        {
            if (!loaded)
            {
                Awards.Add(AwardCue.HatTrick, content.Load<Video>(@"Awards\HatTrick"));
                Awards.Add(AwardCue.HighTon, content.Load<Video>(@"Awards\HighTon"));
                Awards.Add(AwardCue.LowTon, content.Load<Video>(@"Awards\LowTon"));
                Awards.Add(AwardCue.ThreeInABed, content.Load<Video>(@"Awards\ThreeInABed"));
                Awards.Add(AwardCue.ThreeInTheBlack, content.Load<Video>(@"Awards\ThreeInTheBlack"));
                Awards.Add(AwardCue.TonEighty, content.Load<Video>(@"Awards\TonEighty"));
                Awards.Add(AwardCue.WhiteHorse, content.Load<Video>(@"Awards\WhiteHorse"));
                loaded = true;
            }
        }

        public void Play(AwardCue cue)
        {
            if (!Awards.Keys.Contains(cue))
                throw new Exception("Award Error");

            videoPlayer.Play(Awards[cue]);
            SuperDarts.ScreenManager.AddScreen(this);
        }

        public void Stop()
        {
            videoPlayer.Stop();

             SuperDarts.ScreenManager.RemoveScreen(this);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (inputState.MenuEnter || inputState.MenuCancel)
                Stop();
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            if (videoPlayer.PlayPosition.Equals(videoPlayer.Video.Duration))
            {
                Stop();
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            if(videoPlayer.State == MediaState.Playing)
                spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, SuperDarts.Viewport.Width, SuperDarts.Viewport.Height), Color.White);
            spriteBatch.End();
        }
    }
}
