using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Commander;

namespace TestForm
{
    public partial class CommandToolStrip : CommandToolStripBase
    {
        public CommandToolStrip()
        {
            InitializeComponent();
        }


        private void CommandToolStrip_ButtonClick(string path, string[] args)
        {
            System.Diagnostics.Process.Start(path);
        }
    }
}
