using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace ConsoleApplication5
{
    class Program
    {

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);

        static IntPtr hWnd = IntPtr.Zero;
        static IntPtr hDC = IntPtr.Zero;

        static void Main(string[] args)
        {
            hWnd = GetConsoleWindow();
            hDC = GetDC(hWnd);
            if (hWnd == IntPtr.Zero)
                return;
            if (hDC == IntPtr.Zero)
                return;

            using (Graphics consoleGraphics = Graphics.FromHdc(hDC))
            {
                Point[] points = new Point[] {  new Point { X = 400, Y = 70  },  new Point { X = 250,  Y = 200 },  new Point { X = 100, Y = 70 }    };
                consoleGraphics.FillPolygon(Brushes.Green, points);
            }

            for (int i = 0; i < 54; i++)
                Console.ReadLine();

            ReleaseDC(hWnd, hDC);
            DeleteDC(hDC);
        }
    }
}
