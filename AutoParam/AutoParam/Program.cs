using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoParam
{
    class Program
    {
        public static int ii=13;
        public static int ab=99;
        public static int f=1;
        public static DateTime dt=new DateTime(2015,4,5);
        public static string msg = "Hello";

        static void Main(string[] args)
        {
            AutoParamAdapter<Program>.SetParams(args);

            Console.WriteLine("ii = {0}", ii);
            Console.WriteLine("ab = {0}", ab);
            Console.WriteLine("f = {0}", f);
            Console.WriteLine("msg = {0}", msg);
            Console.WriteLine("dt = {0}", dt.ToLongDateString());

            Console.ReadKey();
        }
    }
}
