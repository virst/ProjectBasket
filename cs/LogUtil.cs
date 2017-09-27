using System;
using System.IO;
using System.Threading;

namespace Nedra_app.Utils
{
    public static class LogUtil
    {
        static readonly object O = new object();
        public static bool Active = true;
        public const string LogFile = "nedra.log";
        public const string LogFullFile = "nedraF.log";
        public const string DateMask = "O";

        private const long FileLimit = 100 * 1024 * 1024; // лог файлы не больше 100 Мб

        static LogUtil()
        {
            BackFile(LogFile);
            BackFile(LogFullFile);
        }

        private static void BackFile(string fn)
        {
            try
            {
                if (File.Exists(fn) && new FileInfo(fn).Length > FileLimit)
                {
                    string fn2 = fn.Replace(".", "_old.");
                    if (File.Exists(fn2))
                        File.Delete(fn2);
                    File.Move(fn, fn2);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void WriteFullLog(LogObject k)
        {
            try
            {
                new Thread(WriteLogF).Start(k);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void WriteLogF(object o)
        {
            if(!Active)
                return;
            
            lock (O)
            {
                try
                {
                    var t = o as LogObject;

                    if (t != null)
                    {
                        string s = t.Moment.ToString(DateMask) + " " + t.Group + " : " + t.Message + Environment.NewLine;
                        string x = XmlSer<LogObject>.ToXmlString(t) + Environment.NewLine;

                        File.AppendAllText(LogFile, s);
                        File.AppendAllText(LogFullFile, x);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public static void WriteLog(string zone, string s)
        {
            WriteFullLog(new LogObject(DateTime.Now, zone, s));
        }

    }

    public class LogObject
    {
        public DateTime Moment;
        public string Group;
        public string Message;
        public string Exception;
        public string StackTraceInfo;

        public LogObject() : this(DateTime.Now)
        {

        }

        public override string ToString()
        {
            return Moment.ToString(LogUtil.DateMask) + "-" + Group;
        }

        public LogObject(DateTime moment, string group = ""
            , string message = ""
            , string exception = null)
        {
            this.Moment = moment;
            this.Message = message;
            this.Exception = exception;
            this.Group = group;
            this.StackTraceInfo = new System.Diagnostics.StackTrace().ToString();
        }
    }
}
