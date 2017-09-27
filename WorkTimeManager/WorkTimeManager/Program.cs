using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTime;

namespace WorkTimeManager
{
    class Program
    {
        static void Main(string[] args)
        {
            float t = 0;
            int m = DateTime.Now.Month;
            Manager mn = new Manager();
            var tmpColor = Console.ForegroundColor;
          
            Manager.Work w = new Manager.Work();

            do
            {
                Console.Clear();
                t = mn.SumByMonth(m);
                Console.ForegroundColor = ConsoleColor.Green;;
                Console.WriteLine("Month = {0};Total time = {1:F2}",m,t);
                Console.ForegroundColor = tmpColor;
                Console.WriteLine("Укажите отработку в формате T 0.00 (T - код отработки; 0.00 - затраченое время)");
                var s = Console.ReadLine();
                w.Type = s[0];
                w.moment = DateTime.Today;
                w.Time = Convert.ToSingle(s.Split(' ')[1], Manager.Work.cf);
                mn.AddInFile(w);


            } while (t >= 0);
            Console.WriteLine("End!");
        }
    }
}
