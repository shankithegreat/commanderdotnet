using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestForm.Messages;
using TestForm.Shell;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var ds = ShellFolder.GetDesktopFolder();

            var c = (ShellFolder)((ShellFolder)ds.Childs[0]).Childs[1];
            var d = (ShellFolder)((ShellFolder)ds.Childs[0]).Childs[2];

            var l = d.Childs[6];
            

            MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(@"C:\"));
        }
    }
}
