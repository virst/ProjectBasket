using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViSysMon;

namespace ViSysMonWpf
{
    public class SysMonInfoPrc
    {
        public SysMonInfo I;
        private SysMonAnalystPrc _a;

        public SysMonInfoPrc(SysMonInfo i , SysMonAnalystPrc a)
        {
            this.I = i;
            this._a = a;
        }

        public float UseCpuPrc => I.UseCpu;
        public float UseMemoryPrc => (I.AvailableMemoryMB / I.TotalMemoryMB)*100;
        public float DiskReadPrc => (I.DiskReadMB/_a.Max_ReadMB)*100;
        public float DiskWritePrc => (I.DiskWriteMB / _a.Max_WriteMB)*100;
    }
}
