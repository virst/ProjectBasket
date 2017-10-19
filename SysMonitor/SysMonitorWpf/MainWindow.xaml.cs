using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViSysMon;
using ViSysMonWpf;

namespace SysMonitorWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SysMonAnalystPrc sa;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sa = new SysMonAnalystPrc();
            ii = new SysMonInfoPrc(null, sa);

            UpdateInfo();
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,300);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        ColorPrc p = new ColorPrc();
        private SysMonInfoPrc ii;

        void UpdateInfo()
        {
            var i = sa.GetSysStatus();
            
            /*
             * sb.AppendLine("UseCpu - " + this.UseCpu.ToString("F2") + " %" );
            sb.AppendLine("AvailableMemory - " + this.AvailableMemoryMB.ToString("F1") + " Mbyte");
            sb.AppendLine("TotalMemory - " + this.TotalMemoryMB.ToString("F1") + " Mbyte");

            sb.AppendLine("DiskRead - " + this.DiskReadMB.ToString("F1") + " Mbyte/sec");
            sb.AppendLine("DiskWrite - " + this.DiskWrite.ToString("F1") + " Mbyte/sec");*/

            tbProcessor.Text = i.UseCpu.ToString("F2");
            tbAvailable.Text = i.AvailableMemoryMB.ToString("F1");
            tbTotalPhysicalMemory.Text = i.TotalMemoryMB.ToString("F1");
            tbRead.Text = i.DiskReadMB.ToString("F1");
            tbWrite.Text = i.DiskWriteMB.ToString("F1");
            
            ii.I = i;

            Rectangle1.Fill = new SolidColorBrush(p.getColorPrc((int)ii.UseCpuPrc));
            Rectangle2.Fill = new SolidColorBrush(p.getColorPrc((int)ii.UseMemoryPrc));
            Rectangle3.Fill = new SolidColorBrush(p.getColorPrc((int)ii.DiskReadPrc));
            Rectangle4.Fill = new SolidColorBrush(p.getColorPrc((int)ii.DiskWritePrc));

            tbAvailable_Prc.Text = ii.UseMemoryPrc.ToString("F2");
            tbRead_Prc.Text = ii.DiskReadPrc.ToString("F2");
            tbWrite_Prc.Text = ii.DiskWritePrc.ToString("F2");

        }

        float prc(float c, float m) => m == 0 ? 0 : c/m;
      
    }
}
