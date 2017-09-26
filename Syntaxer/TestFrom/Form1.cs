using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Syntaxer;

namespace TestFrom
{
    public partial class Form1 : Form
    {
        SyntaxController sc;
        
        public Form1()
        {
            InitializeComponent();
            sc = new SyntaxController(this.richTextBox1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
