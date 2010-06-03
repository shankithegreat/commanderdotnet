using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace TestForm
{
    public partial class FileListViewBase : ListView
    {
        private FileSystemNode selectedNode;
        private Dictionary<int, ListViewItem> items = new Dictionary<int, ListViewItem>();


        public FileListViewBase()
        {
            InitializeComponent();

            this.RetrieveVirtualItem += FileListView_RetrieveVirtualItem;
        }



        public FileSystemNode SelectedNode
        {
            get { return selectedNode; }
            set
            {
                if (selectedNode != value)
                {
                    selectedNode = value;
                    LoadNode(selectedNode);
                }
            }
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Assign the image lists to the ListView
            ShellImageList.Set32SmallImageList(this);
            ShellImageList.SetLargeImageList(this);
        }

        protected override void OnItemActivate(EventArgs e)
        {
            var item = items[this.SelectedIndices[0]];


            if (item != null)
            {
                FileSystemNode node = (FileSystemNode)item.Tag;

                if (node.AllowOpen)
                {
                    this.SelectedNode = node;
                }
            }

            base.OnItemActivate(e);
        }


        private void LoadNode(FileSystemNode node)
        {
            if (node.ChildNodes != null && node.ChildNodes.Length > 0)
            {
                this.BeginUpdate();
                largeImageList.Images.Clear();
                items.Clear();

                int count = node.ChildNodes.Length;
                try
                {
                    this.VirtualListSize = count;
                }
                catch (NullReferenceException)
                {
                }
                this.VirtualMode = node.ChildNodes.Length > 0;

                this.SelectedIndices.Clear();
                this.SelectedIndices.Add(0);
                this.EndUpdate();
            }
        }

        private ListViewItem GetItem(int index)
        {
            if (this.items.ContainsKey(index))
            {
                return this.items[index];
            }
            else
            {
                ListViewItem item = new ListViewItem(selectedNode.ChildNodes[index].Name, selectedNode.ChildNodes[index].GetImageIndex());
                item.Tag = selectedNode.ChildNodes[index];
                for (int i = 1; i < this.Columns.Count; i++)
                {
                    item.SubItems.Add("");
                }

                this.items.Add(index, item);
                return item;
            }
        }

        private void FileListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = GetItem(e.ItemIndex);
        }
    }

    public delegate string GetColumntNodeContentEventHandler(ColumnHeader column, FileSystemNode node);

    public delegate void NodeSelectedEventHandler(object sender, FileSystemNode node);
}
