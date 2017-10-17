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

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool withMails)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UseCpu - " + this.UseCpu.ToString("F2") + " %" );
            sb.AppendLine("AvailableMemory - " + (this.AvailableMemory / 1024 / 1024).ToString("F1") + " Mbyte");
            sb.AppendLine("TotalMemory - " + (this.TotalMemory / 1024 / 1024).ToString("F1") + " Mbyte");

            sb.AppendLine("DiskRead - " + (this.DiskRead * 1024).ToString("F0") + " Mbyte/sec");
            sb.AppendLine("DiskWrite - " + (this.DiskWrite * 1024).ToString("F0") + " Mbyte/sec");

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
