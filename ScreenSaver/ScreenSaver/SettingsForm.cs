/*
 * SettingsForm.cs
 * By Frank McCown
 * Summer 2010
 * 
 * Feel free to modify this code.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");
            if (key == null)
            {
                numericUpDown1.Value = 45;
                numericUpDown2.Value = 60;
            }
            else
            {
                try
                {
                    numericUpDown1.Value = Convert.ToDecimal((key.GetValue("text") as string).Split(';')[0]);
                    numericUpDown2.Value = Convert.ToDecimal((key.GetValue("text") as string).Split(';')[0]);
                }
                catch (Exception)
                {

                    numericUpDown1.Value = 45;
                    numericUpDown2.Value = 60;
                }
            }
                
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Demo_ScreenSaver");

            key.SetValue("text", numericUpDown1.Value + ";" + numericUpDown2.Value);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value < numericUpDown1.Value)
                numericUpDown2.Value = numericUpDown1.Value;
        }
    }
}
