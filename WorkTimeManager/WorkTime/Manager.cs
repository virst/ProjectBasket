using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTime
{
    public class Manager
    {
        public readonly List<Work> works = new List<Work>();
        public string FN = "Works.wtm";
        public void Load()
        {
            var ss = File.ReadAllLines(FN);
            works.Clear();
            foreach (string s in ss)
            {
                works.Add(new Work(s));
            }
        }

        public void AddInFile(Work w)
        {
            File.AppendAllText(FN,w + Environment.NewLine);
        }

        public float SumByMonth(int m)
        {
            Load();
            return works.Where(w => w.moment.Month == m).Sum(w => w.Time);
        }

        public class Work
        {
            private const char Splitter = '\t';
            public static readonly CultureInfo cf = new CultureInfo("en-US");
            public Work()
            {
            }

            public Work(string s)
            {
                var ss = s.Split(Splitter);
                Time = Convert.ToSingle(ss[0], cf);
                Type = ss[1][0];
                moment = Convert.ToDateTime(ss[2]);
            }

            public float Time;
            public char Type;
            public DateTime moment;

            public override string ToString()
            {
                return Time.ToString(cf) + Splitter + Type + Splitter + moment.ToString("d");
            }
        }


    }
}
