using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace SysMonitorWpf
{
    public class ColorPrc
    {
        private Color[] c = { Color.Green, Color.Yellow, Color.Red };

        private int a, b;
        private int p1 = 0, p2 = 100;

        private void cint(int prc)
        {
            float fp = mapF(prc, 0, 100, 0, c.Length - 1);

            a = (int)Math.Truncate(fp);
            b = (int)Math.Ceiling(fp);

            if (a == b)
                b++;

            if (b == c.Length)
            {
                b = a;
                a--;
            }

            p1 = map(a, 0, c.Length - 1, 0, 100);
            p2 = map(b, 0, c.Length - 1, 0, 100);
        }

        public Color getColorPrc(int p)
        {
            cint(p);
            int nr = map(p, p1, p2, c[a].R, c[b].R);
            int ng = map(p, p1, p2, c[a].G, c[b].G);
            int nb = map(p, p1, p2, c[a].B, c[b].B);

            return Color.FromArgb(nr, ng, nb);
        }

        private static int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private static float mapF(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }

}
