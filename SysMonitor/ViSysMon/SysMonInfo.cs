using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViSysMon
{
    public class SysMonInfo
    {
        public float UseCpu;
        public float AvailableMemory;
        public ulong TotalMemory;
        public float DiskRead;
        public float DiskWrite;

        public MessageInfo[] Messages = null;

        public class MessageInfo
        {
            public string SenderName;
            public string Subject;
        }
    }
}
