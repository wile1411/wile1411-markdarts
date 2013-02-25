﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    [Serializable]
    public class Resolution
    {
        public int Width;
        public int Height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return Width + ", " + Height;
        }
    }

    [Serializable]
    public class Options
    {
        public float Volume = 0.05f;
        public bool Debug = false;
        public int PlayerChangeTimeout = 8;
        public bool FullScreen = false;

        public readonly Resolution[] Resolutions = new Resolution[] { new Resolution(1920, 1200), new Resolution(1680, 1050), new Resolution(1200, 800), new Resolution(1024, 768) };
        public int ResolutionIndex = 0;

        // Serial Port Settings
        public int ComPort = 3;
        public int BaudRate = 9600;
        public const string TERM_CHAR = "\n";

        // Theme Settings
        public string Theme = "Dark";
        public Color SelectedMenuItemForeground = Color.Yellow;
        public Color MenuItemForeground = Color.White;
        public Color DisabledMenuItemForeground = Color.DarkGray;
        public Color[] PlayerColors = new Color[] { Color.Red, Color.Blue, Color.Orange, Color.Yellow, Color.Teal, Color.Gold, Color.Green, Color.Firebrick, Color.Lime };

        public bool PlayAwards = true;

        public int MaxRounds = 8;

        /// <summary>
        /// SegmentMap holds which dartboard coordinates that correspond to which segment.
        /// The dartboard coordinates may change depending on how you configure the cables running
        /// from the dartboard matrix to the circuit.
        /// 
        /// The key pair contains the segment, multiplier and the value pair contains the x, y coordinates which the key is mapped to.
        /// If for example the single 20 segment is pressed on the dartboard, and the coordinates 4, 18 are sent, the segment map should
        /// contain a key pair (20, 1) with value (4, 18).
        /// </summary>
        public Dictionary<IntPair, IntPair> SegmentMap = new Dictionary<IntPair, IntPair>();

        public const string OptionsFilename = "options.bin";

        public static Options Load()
        {
            if (File.Exists(OptionsFilename))
            {
                FileInfo info = new FileInfo(OptionsFilename);
                if (info.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Options loaded: " + OptionsFilename);
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(OptionsFilename, FileMode.Open);
                    Options temp = (Options)bf.Deserialize(fs);
                    fs.Close();

                    return temp;
                }
            }

            System.Diagnostics.Debug.WriteLine("Options file not found, using default");

            return new Options();
        }

        public bool Save()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(OptionsFilename, FileMode.Create);
                bf.Serialize(fs, this);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                //throw;
                return false;
            }
        }
    }
}
