using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ViSysMon;


namespace SysMonitorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SysMonAnalyst sa = new SysMonAnalyst();
            var defColor = Console.ForegroundColor;

            while (true)
            {
                var i = sa.GetSysStatus();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("UseCpu = " + i.UseCpu.ToString("F3"));
                Console.WriteLine("AvailableMemory = " + (i.AvailableMemory / 1024 / 1024).ToString("F1"));
                Console.WriteLine("TotalMemory = " + (i.TotalMemory / 1024 / 1024).ToString("F1"));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Messages : ");
                foreach (SysMonInfo.MessageInfo m in i.Messages)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("SenderName ");
                    Console.ForegroundColor = defColor;
                    Console.WriteLine(m.SenderName);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Subject ");
                    Console.ForegroundColor = defColor;
                    Console.WriteLine(m.Subject);
                }
                Console.ReadKey();

            }

        }

    }
}
