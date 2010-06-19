using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestForm.Messages;

namespace TestForm
{
    public partial class FileListView : FileListViewBase
    {
        public FileListView()
        {
            InitializeComponent();

            MessageDispatcher.Dispatcher.Subscribe(this);
        }

        
        [DirectorySelected]
        private void dispatcher_DirectorySelected(DirectorySelectedArgs e)
        {
           if(Directory.Exists(e.SelectedDirectory))
           {
               this.SelectedNode = new DirectoryNode(null, new DirectoryInfo(e.SelectedDirectory));
           }
        }
    }
}
