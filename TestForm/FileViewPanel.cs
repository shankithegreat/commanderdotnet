using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShellDll;
using TestForm;
using TestForm.Messages;

namespace Commander
{
    public partial class FileViewPanel : UserControl
    {
        public FileViewPanel()
        {
            InitializeComponent();
        }
        

        private void upButton_Click(object sender, EventArgs e)
        {
            MessageDispatcher.Dispatcher.Invoke(new UpDirectorySelectedAttribute());
        }

        private void rootButton_Click(object sender, EventArgs e)
        {
            MessageDispatcher.Dispatcher.Invoke(new RootDirectorySelectedAttribute());
        }
    }
}
