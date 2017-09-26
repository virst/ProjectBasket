using System;
using System.Collections.Generic;
using System.Text;
using LabGen17;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Rcv r = new Rcv(4);

            r.Val = 11;

            Console.WriteLine("R = " + r.Row.ToString() + " C = " + r.Col.ToString());

            r.Row = 1; r.Col = 2;

            Console.WriteLine("V = " + r.Val.ToString());

            Console.ReadKey();
        }
    }
}
