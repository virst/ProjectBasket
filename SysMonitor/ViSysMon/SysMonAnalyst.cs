using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.VisualBasic.Devices;

// https://msdn.microsoft.com/ru-ru/library/office/ff462097.aspx

namespace ViSysMon
{
    public class SysMonAnalyst
    {
        private readonly PerformanceCounter _cpucounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private readonly PerformanceCounter _memcounteraval = new PerformanceCounter("Memory", "Available Bytes");
        private readonly PerformanceCounter _diskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
        private readonly PerformanceCounter _diskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
        private readonly ulong _totalPhysicalMemory = new ComputerInfo().TotalPhysicalMemory;

        public virtual SysMonInfo GetSysStatus()
        {
            return GetSysStatus(true);
        }

        public virtual SysMonInfo GetSysStatus(bool noMail)
        {
            //var z = new Microsoft.VisualBasic.Devices.ComputerInfo();

            SysMonInfo ret = new SysMonInfo();  // new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory

            #region Counters 

            ret.UseCpu = _cpucounter.NextValue();
            ret.TotalMemory = _totalPhysicalMemory;
            ret.AvailableMemory = _memcounteraval.NextValue();
            ret.DiskRead = _diskRead.NextValue();
            ret.DiskWrite = _diskWrite.NextValue();

            #endregion

            if (noMail)
            {
                ret.Messages = new SysMonInfo.MessageInfo[0];
                return ret;
            }

            #region Outlook

            var application = GetApplicationObject();
            if (application != null)
            {

                Outlook.MAPIFolder inBox = application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                Outlook.Items items = (Outlook.Items)inBox.Items;
                items = items.Restrict("[UnRead] = true");
                ret.Messages = new SysMonInfo.MessageInfo[items.Count];
                //for(int i=0;i< items.Count;i++) 
                int i = 0;
                foreach (Outlook.MailItem eMails in items)
                {

                    ret.Messages[i] = new SysMonInfo.MessageInfo
                    {
                        SenderName = eMails.SenderName,
                        Subject = eMails.Subject
                    };
                    i++;

                }
            }
            else
                ret.Messages = new SysMonInfo.MessageInfo[0];

            #endregion

            return ret;
        }

        protected static Outlook.Application GetApplicationObject()
        {

            Outlook.Application application = null;

            // Check whether there is an Outlook process running.
            if (Process.GetProcessesByName("OUTLOOK").Any())
            {
                try
                {
                    // If so, use the GetActiveObject method to obtain the process and cast it to an Application object.
                    application = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
                }
                catch(Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
                // If not, create a new instance of Outlook and log on to the default profile.
                application = new Outlook.Application();
                Outlook.NameSpace nameSpace = application.GetNamespace("MAPI");
                nameSpace.Logon("", "", Missing.Value, Missing.Value);
                nameSpace = null;
            }

            // Return the Outlook Application object.
            return application;
        }
    }
}
