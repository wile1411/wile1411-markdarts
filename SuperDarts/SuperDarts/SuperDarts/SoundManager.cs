using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperDarts
{
    public enum SoundCue
    {
        MenuSelect,
        MenuEnter,
        SingleBull,
        DoubleBull,
        GameStart,
        Won,
        Bust,
        NewRound,
        LastRound,
        ThrowStart,
        Single,
        Double,
        Triple,
        CricketClosed,
        MenuBack
    }

    public class SoundManager
    {
        private static Dictionary<SoundCue, SoundEffect> LoadedSongs = new Dictionary<SoundCue, SoundEffect>();
        private ContentManager _content;

        public SoundManager(ContentManager content)
        {
            _content = content;
        }

        private string GetFilename(SoundCue cue)
        {
            string dir = @"Sounds\";
            switch (cue)
            {
                case SoundCue.MenuSelect:
                    return dir + "select";
                case SoundCue.MenuEnter:
                    return dir + "enter";
                case SoundCue.MenuBack:
                    return dir + "enter";
                case SoundCue.SingleBull:
                    return dir + "singlebullseye";
                case SoundCue.DoubleBull:
                    return dir + "doublebullseye";
                case SoundCue.GameStart:
                    return dir + "gamestart";
                case SoundCue.Won:
                    return dir + "applause";
                case SoundCue.Bust:
                    return dir + "bust";
                case SoundCue.LastRound:
                    return dir + "finalround";
                case SoundCue.NewRound:
                    return dir + "roundstart";
                case SoundCue.ThrowStart:
                    return dir + "throwstart";
                case SoundCue.Single:
                    return dir + "single";
                case SoundCue.Double:
                    return dir + "double";
                case SoundCue.Triple:
                    return dir + "triple";
                case SoundCue.CricketClosed:
                    return dir + "cricketclosed";
                default:
                    break;
            }

            return "";
        }

        public void PlaySound(SoundCue cue)
        {
            if (!LoadedSongs.Keys.Contains(cue))
                LoadedSongs.Add(cue, _content.Load<SoundEffect>(GetFilename(cue)));

            LoadedSongs[cue].Play(SuperDarts.Options.Volume, 0, 0);
        }
    }
}
