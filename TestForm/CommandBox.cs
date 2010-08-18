using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestForm.Messages;

namespace TestForm
{
    public partial class CommandBox : CommandBoxBase
    {
        public CommandBox()
        {
            InitializeComponent();

            MessageDispatcher.Dispatcher.Subscribe(this);
        }


        private void CommandBox_CdCommand(object sender, string path)
        {
            MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(path));
        }
    }
}
