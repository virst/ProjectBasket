using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParam
{
    public class AutoParamAdapter<T>
    {
        public static void SetParams(string[] args)
        {
            var tp = typeof(T);

            foreach (string a in args)
            {
                var pp = a.Split('=');
                if(pp.Length!=2)
                    continue;

                var f = tp.GetField(pp[0]);
                f.SetValue(null, Convert.ChangeType(pp[1],f.FieldType));
            }
        }
    }
}
