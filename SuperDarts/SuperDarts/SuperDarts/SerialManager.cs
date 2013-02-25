using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Collections;
using Microsoft.Xna.Framework;

namespace SuperDarts
{
    [Serializable]
    public class IntPair : IComparable
    {
        public int X, Y = 0;

        public IntPair(int a, int b)
        {
            X = a;
            Y = b;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            IntPair temp = obj as IntPair;

            if (temp != null)
            {
                if (temp.X == X && temp.Y == Y)
                    return 0;
            }

            return -1;
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

        public override string ToString()
        {
            return "X: " + X + ", " + "Y: " + Y;
        }

        #endregion
    }

    public class SerialManager
    {
        private static SerialPort SerialPort;

        public delegate void DartRegisteredDelegate(int segment, int multiplier);
        public delegate void DartHitDelegate(IntPair coords);

        public DartRegisteredDelegate OnDartRegistered;
        public DartHitDelegate OnDartHit;

        public SerialManager()
        {
            SerialPort = new SerialPort();

            SerialPort.BaudRate = SuperDarts.Options.BaudRate;
            SerialPort.PortName = "COM" + SuperDarts.Options.ComPort.ToString();
            SerialPort.NewLine = Options.TERM_CHAR;

            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        }

        public bool IsPortOpen
        {
            get
            {
                return SerialPort.IsOpen;
            }
        }

        public void OpenPort()
        {
            if (SerialPort.IsOpen)
                return;

            try
            {
                SerialPort.Open();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Could not establish a connection with the dart board:");
                sb.AppendLine("\""+ex.Message+"\"");
                sb.AppendLine();
                sb.AppendLine("Please make sure that the dart board is connected and\nthat the correct settings have been configured in the options.");

                MessageBoxScreen mb = new MessageBoxScreen("Error", sb.ToString(), MessageBoxButtons.Ok);
                ((TextBlock)mb.StackPanel.Items[0]).Color = Color.Red;

                SuperDarts.ScreenManager.AddScreen(mb);
                return;
            }

            if (SerialPort.IsOpen)
            {

            }
        }

        void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            
            if (indata[0] == 'B') // Button messages are prefixed with "B:"
                ParseButton(indata);
            else
                ParseScore(indata);
        }

        bool[] buttonStates = new bool[] { false, false, false, false, false };

        public bool[] ButtonStates
        {
            get
            {
                bool[] temp = buttonStates;
                buttonStates = new bool[] { false, false, false, false, false }; // Reset the buttonStates every time the current buttonsStates gets used
                return temp;
            }
        }

        private void ParseButton(string indata)
        {
            // A button message is in the format B: X, where X is the index of the pressed button
            string temp = indata.Substring(2); // temp now holds X
            int buttonIndex = int.Parse(temp);

            buttonStates[buttonIndex] = true;
        }

        private void ParseScore(string indata)
        {
            // A dart hit is in the format H: X, Y, where X, Y is the coordinate of the hit segment
            string[] temp = indata.Substring(2).Split(',');

            if (temp.Length == 2)
            {
                IntPair coords = new IntPair(int.Parse(temp[0]), int.Parse(temp[1]));

                if (OnDartHit != null)
                {
                    OnDartHit(coords);
                }

                // Check if the given coordinates are mapped to a segment
                if (SuperDarts.Options.SegmentMap.ContainsValue(coords))
                {
                    //Find the key (segment, multiplier) which contains the value of the received X, Y coordinates
                    IntPair segmentInfo = SuperDarts.Options.SegmentMap.First(p => coords.Equals(p.Value)).Key;

                    if (segmentInfo != null && OnDartRegistered != null)
                    {
                        OnDartRegistered(segmentInfo.X, segmentInfo.Y);
                    }
                }
            }
        }

        public void Close()
        {
            if (SerialPort.IsOpen)
                SerialPort.Close();
        }
    }
}
