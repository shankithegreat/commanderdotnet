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


        protected override void OnNodeSelected(FileSystemNode node)
        {
            base.OnNodeSelected(node);

            //MessageDispatcher.Dispatcher.Invoke(new DirectorySelectedAttribute(), new DirectorySelectedArgs(node.Path));
        }


        [DirectorySelected]
        private void dispatcher_DirectorySelected(DirectorySelectedArgs e)
        {
            if (this.SelectedNode == null || (this.SelectedNode.Path != e.SelectedDirectory && Directory.Exists(e.SelectedDirectory)))
            {
                this.SelectedNode = new DirectoryNode(null, new DirectoryInfo(e.SelectedDirectory));
            }
        }

        [ShellDirectorySelected]
        private void dispatcher_ShellDirectorySelected(ShellDirectorySelectedArgs e)
        {
            if (this.SelectedNode == null || (this.SelectedNode.Path != e.SelectedDirectory.Path))
            {
                this.SelectedNode = new ShellDirectoryNode(null, e.SelectedDirectory);
            }
        }
    }
}
