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
    public delegate void DirectorySelectedEventHandler(object sender, DirectoryInfo directory);

    public partial class FileView : UserControl
    {
        private ShellContextMenu contextMenu = new ShellContextMenu();
        private DirectoryInfo selectedDirectory = null;

        public FileView()
        {
            InitializeComponent();

            ShellImageList.SetSmallImageList(listView);
            ShellImageList.SetLargeImageList(listView);

            if (this.Focused)
            {
                FileView_Enter(this, null);
            }
            else
            {
                FileView_Leave(this, null);
            }
        }

        public ListView ListView
        {
            get
            {
                return listView;
            }
        }

        public event DirectorySelectedEventHandler DirectorySelected;

        public DirectoryInfo SelectedDirectory
        {
            get
            {
                return selectedDirectory;
            }
            set
            {
                if (LoadDirectory(value))
                {
                    titleLabel.Text = GetTitleLabelText(value);
                }
            }
        }

        private bool LoadDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                return false;
            }

            FileSystemInfo[] list;

            try
            {
                list = directory.GetFileSystemInfos();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            listView.Items.Clear();
            listView.Tag = directory;

            foreach (FileSystemInfo fsi in list)
            {
                ListViewItem item = listView.Items.Add(fsi.Name, SafeNativeMethods.GetAssociatedIconIndex(fsi.FullName));
                item.Tag = fsi;
            }

            selectedDirectory = directory;
            OnDirectorySelected(selectedDirectory);
            return true;
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
                DirectorySelected(this, directory);
            }
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {
            listView.Focus();
        }

        private void FileView_Enter(object sender, EventArgs e)
        {
            titleLabel.BackColor = SystemColors.ActiveCaption;
            titleLabel.ForeColor = SystemColors.ActiveCaptionText;
        }

        private void FileView_Leave(object sender, EventArgs e)
        {
            titleLabel.BackColor = SystemColors.InactiveCaption;
            titleLabel.ForeColor = SystemColors.InactiveCaptionText;
        }

        private void titleLabel_BeforeEdit(object sender, BeforeEditEventArgs e)
        {
            e.Text = selectedDirectory.FullName;
        }

        private void titleLabel_AfterEdit(object sender, AfterEditEventArgs e)
        {
            DirectoryInfo directory;
            try
            {
                directory = new DirectoryInfo(e.Text);              
            }
            catch (Exception except)
            {
                e.Cancel = true;
                return;
            }

            if (LoadDirectory(directory))
            {
                e.Text = GetTitleLabelText(directory);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private string GetTitleLabelText(DirectoryInfo directory)
        {
            return Path.Combine(directory.FullName, "*.*");
        }
    }
}
