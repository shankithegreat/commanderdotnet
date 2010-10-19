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
using Shell;
using TestForm.Messages;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var ds = ShellFolder.GetDesktopFolder();

            //MessageDispatcher.Dispatcher.Invoke(new ShellDirectorySelectedAttribute(), new ShellDirectorySelectedArgs(ds));
            MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(@"D:\"));
        }
    }
}
