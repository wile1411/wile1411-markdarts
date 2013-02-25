using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    public class OptionsMenuScreen : MenuScreen
    {
        DialMenuEntry Volume = new DialMenuEntry(((int)(Math.Round(SuperDarts.Options.Volume * 100))).ToString() + "%", "Volume:");
        DialMenuEntry FullScreen = new DialMenuEntry(SuperDarts.Options.FullScreen ? "FullScreen" : "Windowed", "Screen Mode:");
        DialMenuEntry PlayerChangeTimeout;
        DialMenuEntry ComPort = new DialMenuEntry("COM" + SuperDarts.Options.ComPort.ToString(), "Serial Port:");
        DialMenuEntry Resolution = new DialMenuEntry(SuperDarts.Options.Resolutions[SuperDarts.Options.ResolutionIndex].ToString(), "Resolution:");
        DialMenuEntry Awards = new DialMenuEntry(SuperDarts.Options.PlayAwards ? "Yes" : "No", "Play Awards:");
        MenuEntry EditSegmentMap = new MenuEntry("Edit Segment Mapping");
        MenuEntry Back = new MenuEntry("Back");

        public string PlayerChangeTimeoutText
        {
            get
            {
                string text = SuperDarts.Options.PlayerChangeTimeout.ToString() + "s";

                if (SuperDarts.Options.PlayerChangeTimeout == 0)
                    text = "Disabled";

                return text;
            }
        }

        public OptionsMenuScreen() : base("Options")
        {
            Volume.OnMenuLeft += new EventHandler(Volume_OnMenuLeft);
            Volume.OnMenuRight += new EventHandler(Volume_OnMenuRight);
            Volume.OnSelected += new EventHandler(Volume_OnMenuRight);
            Volume.OnCanceled += new EventHandler(Volume_OnMenuLeft);

            FullScreen.OnSelected += new EventHandler(FullScreen_ValueChanged);
            FullScreen.OnCanceled += new EventHandler(FullScreen_ValueChanged);

            Back.OnSelected += new EventHandler(Back_OnSelected);

            PlayerChangeTimeout = new DialMenuEntry(PlayerChangeTimeoutText, "Player Change Timeout:");
            PlayerChangeTimeout.OnMenuLeft += new EventHandler(PlayerChangeTimout_OnMenuLeft);
            PlayerChangeTimeout.OnMenuRight += new EventHandler(PlayerChangeTimout_OnMenuRight);
            PlayerChangeTimeout.OnSelected += new EventHandler(PlayerChangeTimout_OnMenuRight);
            PlayerChangeTimeout.OnCanceled += new EventHandler(PlayerChangeTimout_OnMenuLeft);

            EditSegmentMap.OnSelected += new EventHandler(EditSegmentMap_OnSelected);

            ComPort.OnMenuLeft += new EventHandler(ComPort_OnMenuLeft);
            ComPort.OnMenuRight += new EventHandler(ComPort_OnMenuRight);
            ComPort.OnSelected += new EventHandler(ComPort_OnMenuRight);
            ComPort.OnCanceled += new EventHandler(ComPort_OnMenuLeft);

            Awards.OnSelected += new EventHandler(Awards_OnSelected);
            Awards.OnMenuLeft += new EventHandler(Awards_OnSelected);
            Awards.OnMenuRight += new EventHandler(Awards_OnSelected);
            Awards.OnCanceled += new EventHandler(Awards_OnSelected);


            Resolution.OnMenuRight += new EventHandler(Resolution_OnMenuRight);
            Resolution.OnMenuLeft += new EventHandler(Resolution_OnMenuLeft);
            Resolution.OnSelected += new EventHandler(Resolution_OnMenuRight);
            Resolution.OnCanceled += new EventHandler(Resolution_OnMenuLeft);

            MenuItems.AddItems(Resolution, Awards, Volume, PlayerChangeTimeout, ComPort, FullScreen, EditSegmentMap, Back);
        }

        void Awards_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.Options.PlayAwards = !SuperDarts.Options.PlayAwards;
            Awards.Value = SuperDarts.Options.PlayAwards ? "Yes" : "No";
        }

        void UpdateResolution()
        {
            SuperDarts.GraphicsDeviceManager.PreferredBackBufferWidth = SuperDarts.Options.Resolutions[SuperDarts.Options.ResolutionIndex].Width;
            SuperDarts.GraphicsDeviceManager.PreferredBackBufferHeight = SuperDarts.Options.Resolutions[SuperDarts.Options.ResolutionIndex].Height;

            SuperDarts.GraphicsDeviceManager.ApplyChanges();

            Resolution.Value = SuperDarts.Options.Resolutions[SuperDarts.Options.ResolutionIndex].ToString();
        }

        void Resolution_OnMenuLeft(object sender, EventArgs e)
        {
            SuperDarts.Options.ResolutionIndex--;
            if (SuperDarts.Options.ResolutionIndex < 0)
                SuperDarts.Options.ResolutionIndex = SuperDarts.Options.Resolutions.Length - 1;

            UpdateResolution();
        }

        void Resolution_OnMenuRight(object sender, EventArgs e)
        {
            SuperDarts.Options.ResolutionIndex++;
            if (SuperDarts.Options.ResolutionIndex >= SuperDarts.Options.Resolutions.Length)
                SuperDarts.Options.ResolutionIndex = 0;

            UpdateResolution();
        }

        void ComPort_OnMenuRight(object sender, EventArgs e)
        {
            SuperDarts.Options.ComPort++;

            if (SuperDarts.Options.ComPort > 10)
                SuperDarts.Options.ComPort = 1;

            ComPort.Value = "COM" + SuperDarts.Options.ComPort.ToString();
        }

        void ComPort_OnMenuLeft(object sender, EventArgs e)
        {
            SuperDarts.Options.ComPort--;

            if (SuperDarts.Options.ComPort < 1)
                SuperDarts.Options.ComPort = 10;

            ComPort.Value = "COM" + SuperDarts.Options.ComPort.ToString();
        }

        void Back_OnSelected(object sender, EventArgs e)
        {
            if (SuperDarts.Options.Save())
            {
                var mb = new MessageBoxScreen("Options saved to file: " + Options.OptionsFilename + "!", string.Empty, MessageBoxButtons.Ok);
                SuperDarts.ScreenManager.AddScreen(mb);
            }
            else
            {
                var mb = new MessageBoxScreen("Error", "Options could not be saved!", MessageBoxButtons.Ok);
                SuperDarts.ScreenManager.AddScreen(mb);
            }
            this.ExitScreen(this, null);
        }

        public override void CancelScreen()
        {
            Back_OnSelected(null, null);
            //base.CancelScreen();
        }

        void EditSegmentMap_OnSelected(object sender, EventArgs e)
        {
            SuperDarts.ScreenManager.AddScreen(new SegmentMapScreen("Edit Segment Map"));
        }

        void PlayerChangeTimout_OnMenuRight(object sender, EventArgs e)
        {
            SuperDarts.Options.PlayerChangeTimeout += 1;

            if (SuperDarts.Options.PlayerChangeTimeout > 30)
                SuperDarts.Options.PlayerChangeTimeout = 30;

            PlayerChangeTimeout.Value = PlayerChangeTimeoutText;
        }

        void PlayerChangeTimout_OnMenuLeft(object sender, EventArgs e)
        {
            SuperDarts.Options.PlayerChangeTimeout -= 1;

            if (SuperDarts.Options.PlayerChangeTimeout < 0)
            {
                SuperDarts.Options.PlayerChangeTimeout = 0;
            }

            PlayerChangeTimeout.Value = PlayerChangeTimeoutText;
        }

        void FullScreen_ValueChanged(object sender, EventArgs e)
        {
            SuperDarts.GraphicsDeviceManager.ToggleFullScreen();
            
            SuperDarts.Options.FullScreen = SuperDarts.GraphicsDeviceManager.IsFullScreen;

            FullScreen.Value = SuperDarts.Options.FullScreen ? "FullScreen" : "Windowed";
        }

        void Volume_OnMenuRight(object sender, EventArgs e)

        {
            SuperDarts.Options.Volume += 0.10f;

            if (SuperDarts.Options.Volume > 1.0f)
                SuperDarts.Options.Volume = 1.00f;

            updateVolumeValue();
            SuperDarts.SoundManager.PlaySound(SoundCue.SingleBull);
        }

        void updateVolumeValue()
        {
            Volume.Value = ((int)(Math.Round(SuperDarts.Options.Volume * 100))).ToString() + "%";
        }

        void Volume_OnMenuLeft(object sender, EventArgs e)
        {
            SuperDarts.Options.Volume -= 0.10f;

            if (SuperDarts.Options.Volume < 0)
                SuperDarts.Options.Volume = 0.00f;

            updateVolumeValue();
            SuperDarts.SoundManager.PlaySound(SoundCue.SingleBull);
        }
    }
}
