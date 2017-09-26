using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nedra_app.Utils
{
    internal static class Tag2
    {
        static Dictionary<Control,Dictionary<string,object>> data = new Dictionary<Control, Dictionary<string, object>>();

        public static object GetTag2Value(this Control c, string p)
        {
            if (data.ContainsKey(c) && data[c].ContainsKey(p))
            {
                return data[c][p];
            }
            return null;
        }

        public static void SetTag2Value(this Control c, string p, object v)
        {
            if (!data.ContainsKey(c))
            {
                data.Add(c,new Dictionary<string, object>());
                c.Disposed += C_Disposed;
            }

            var d = data[c];

            if(!d.ContainsKey(p))
                d.Add(p,null);

            d[p] = v;
        }

        private static void C_Disposed(object sender, EventArgs e)
        {
            data.Remove((Control) sender);
        }
    }
}
