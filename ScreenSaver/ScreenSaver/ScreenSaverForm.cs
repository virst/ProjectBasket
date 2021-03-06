﻿/*
 * ScreenSaverForm.cs
 * By Frank McCown
 * Summer 2010
 * 
 * Feel free to modify this code.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private int min, max , interv ;
        private Random rnd = new Random();

        private Point mouseLocation;
        private bool previewMode = false;
        private Random rand = new Random();
        private int pos = 0;


        private List<String> rs = new List<String>(); 

        public ScreenSaverForm()
        {
            InitializeComponent();
        }

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }

        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            // Make text smaller
           

            previewMode = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {            
            LoadSettings();

            Cursor.Hide();            
          //  TopMost = true;

            moveTimer.Interval = 1000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }

        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            if (interv == 0)
            {
               /*
                * this.pictureBox1.Image = rs[rnd.Next(rs.Count)];

                interv = rnd.Next(min, max);
                * */


                this.pictureBox1.Image = (Properties.Resources.ResourceManager.GetObject(rs[pos]) as Image);
                pos = (pos + 1) % rs.Count;
                interv = 30;
            }
            else
            {
                interv--;
            }
        }

        private void LoadSettings()
        {
           

           ResourceSet resSet = Properties.Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);
            
            foreach (DictionaryEntry entry in resSet)
            {
                if (entry.Value is Image)
                {
                    rs.Add(entry.Key.ToString());
                }
            }
            rs.Sort();
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");
            if (key == null)
            {
                min = 45;
                max = 60;
            }
            else
            {
                try
                {
                    min = (int)Convert.ToDecimal((key.GetValue("text") as string).Split(';')[0]);
                    max = (int)Convert.ToDecimal((key.GetValue("text") as string).Split(';')[0]);
                }
                catch (Exception)
                {

                    min = 45;
                    max = 60;
                }
            }

           

          //  rs = new List<string>(resNames);

        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }
    }
}
