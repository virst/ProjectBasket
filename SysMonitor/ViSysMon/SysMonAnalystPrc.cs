using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViSysMon
{
    public class SysMonAnalystPrc : SysMonAnalyst
    {
        public float Max_Read { private set; get; }
        public float Max_Write { private set; get; }

        public float Max_ReadMB => Max_Read / SysMonInfo.MByte;
        public float Max_WriteMB => Max_Write / SysMonInfo.MByte;

       public override SysMonInfo GetSysStatus(bool noMail)
        {
            var r = base.GetSysStatus(noMail);

            if (Max_Read < r.DiskRead)
                Max_Read = r.DiskRead;

            if (Max_Write < r.DiskWrite)
                Max_Write = r.DiskWrite;

            return r;

        }
    }
}
