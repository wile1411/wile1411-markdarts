using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SuperDarts
{
    [Serializable]
    public struct Record
    {
        public int Score;
        public DateTime Date;

        public Record(int score, DateTime date)
        {
            Score = score;
            Date = date;
        }

        public override string ToString()
        {
            return Score.ToString() + "          " + Date.ToString();
        }
    }

    [Serializable]
    public class RecordManager
    {
        public List<Record> Records = new List<Record>();
        public const string FileName = "records.sd";

        public static RecordManager Load()
        {
            RecordManager rm = new RecordManager();

            if (File.Exists(FileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(FileName, FileMode.Open);
                rm.Records = (List<Record>)bf.Deserialize(fs);
                fs.Close();
            }

            return rm;
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(FileName, FileMode.Create);
            bf.Serialize(fs, Records);
            fs.Close();
        }
    }

    public class CountUp : GameMode
    {
        public bool IsPractice = false;

        public CountUp(int players)
            : base(players)
        {
            BaseScreen = new ScoreView(this);
        }

        public override int GetScore(Player p)
        {
            int score = 0; // - Handicap

            score += p.Rounds.Sum(r => GetScore(r));

            return score;
        }

        RecordManager recordManager;

        public void SaveScore(int score)
        {
            DateTime date = DateTime.Now;
            if (recordManager == null)
                recordManager = RecordManager.Load();
            recordManager.Records.Add(new Record(score, date));
        }

        public override void GameOver()
        {
            if (IsPractice)
            {
                SuperDarts.Players.ForEach(p => SaveScore(GetScore(p)));
                recordManager.Save();
            }

            base.GameOver();
        }

        public override string Name
        {
            get { return "CountUp"; }
        }
    }
}
