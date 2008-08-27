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

namespace Commander
{
    public delegate void DirectorySelectedEventHandler(DirectoryInfo directory);

    public partial class FileView : UserControl
    {
        private ShellContextMenu contextMenu = new ShellContextMenu();
        private DirectoryInfo selectedDirectory = null;

        public FileView()
        {
            InitializeComponent();

            ShellImageList.SetSmallImageList(listView);
            ShellImageList.SetLargeImageList(listView);
        }

        public ListView ListView
        {
            get
            {
                return listView;
            }
        }

        public event DirectorySelectedEventHandler DirectorySelected;


        public void LoadDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                return;
            }

            FileSystemInfo[] list;

            try
            {
                list = directory.GetFileSystemInfos();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            titleLabel.Text = Path.Combine(directory.FullName, "*.*");

            listView.Items.Clear();
            listView.Tag = directory;

            foreach (FileSystemInfo fsi in list)
            {
                ListViewItem item = listView.Items.Add(fsi.Name, SafeNativeMethods.GetAssociatedIconIndex(fsi.FullName));
                item.Tag = fsi;
            }

            selectedDirectory = directory;
            OnDirectorySelected(selectedDirectory);
        }

        private void listView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && sender is ListView)
            {
                ListView listView = (ListView)sender;
                if (listView.SelectedItems.Count > 0)
                {
                    Point location = listView.PointToScreen(e.Location);

                    List<string> list = new List<string>(listView.SelectedItems.Count);
                    foreach (ListViewItem selectItem in listView.SelectedItems)
                    {
                        FileSystemInfo fsi = (FileSystemInfo)selectItem.Tag;
                        list.Add(fsi.FullName);
                    }

                    contextMenu.Show(location, list.ToArray());
                }
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo directory = (DirectoryInfo)listView.Tag;
            LoadDirectory(directory.Parent);
        }

        private void rootButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo directory = (DirectoryInfo)listView.Tag;
            LoadDirectory(directory.Root);
        }

        private void titleLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point location = ((Control)sender).PointToScreen(e.Location);

                contextMenu.Show(location, selectedDirectory.FullName);
            }
        }

        private void listView_ItemActivate(object sender, EventArgs e)
        {
            FileSystemInfo fsi = (FileSystemInfo)listView.SelectedItems[0].Tag;
            if (fsi is DirectoryInfo)
            {
                DirectoryInfo directory = (DirectoryInfo)fsi;
                LoadDirectory(directory);
            }
            else
            {
                contextMenu.DefaultCommand((FileInfo)fsi);
            }
        }


        protected virtual void OnDirectorySelected(DirectoryInfo directory)
        {
            if(DirectorySelected != null)
            {
                DirectorySelected(directory);
            }
        }
    }
}
