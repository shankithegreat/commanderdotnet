using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commander
{
    public partial class TestForm : Form
    {
        private ShellDrop sd;

        public TestForm()
        {
            InitializeComponent();

            sd = new ShellDrop(listView1);
        }
    }
}
