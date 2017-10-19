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

        public const float MByte = 1024 * 1024;

        public float AvailableMemoryMB => AvailableMemory/MByte;
        public float TotalMemoryMB => TotalMemory/MByte;
        public float DiskReadMB => DiskRead/MByte;
        public float DiskWriteMB => DiskWrite/MByte;

        public MessageInfo[] Messages = null;

        public class MessageInfo
        {
            public string SenderName;
            public string Subject;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool withMails)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UseCpu - " + this.UseCpu.ToString("F2") + " %" );
            sb.AppendLine("AvailableMemory - " + this.AvailableMemoryMB.ToString("F1") + " Mbyte");
            sb.AppendLine("TotalMemory - " + this.TotalMemoryMB.ToString("F1") + " Mbyte");

            sb.AppendLine("DiskRead - " + this.DiskReadMB.ToString("F1") + " Mbyte/sec");
            sb.AppendLine("DiskWrite - " + this.DiskWriteMB.ToString("F1") + " Mbyte/sec");

            if (withMails)
            {
                Console.WriteLine("Messages : ");
                foreach (SysMonInfo.MessageInfo m in this.Messages)
                {

                    sb.Append("SenderName ");
                    sb.AppendLine(m.SenderName);
                    sb.Append("Subject ");
                    sb.AppendLine(m.Subject);
                }
            }

            return sb.ToString();
        }
    }
}
