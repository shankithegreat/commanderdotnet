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
        private DirectoryInfo currentDirectory = null;

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

        public DirectoryInfo CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
            set
            {
                if (LoadDirectory(value))
                {
                    titleLabel.Text = GetTitleLabelText(value);
                }
            }
        }

        public FileSystemInfo[] GetSelected()
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            foreach (ListViewItem item in listView.SelectedItems)
            {
                list.Add((FileSystemInfo)item.Tag);
            }
            return list.ToArray();
        }

        public DirectoryInfo GetSelectedDirectory()
        {
            if (listView.SelectedItems.Count == 1)
            {
                FileSystemInfo item = (FileSystemInfo)listView.SelectedItems[0].Tag;
                if (item is DirectoryInfo)
                {
                    return (DirectoryInfo)item;
                }
            }
            return currentDirectory;
        }

        public void Delete()
        {
            contextMenu.DeleteCommand(GetSelected());
            this.Refresh();
        }

        public void Copy()
        {
            contextMenu.CopyCommand(GetSelected());
            this.Refresh();
        }

        public void Paste()
        {
            contextMenu.PasteCommand(GetSelectedDirectory());
            this.Refresh();
        }

        public void Cut()
        {
            contextMenu.CutCommand(GetSelected());
            this.Refresh();
        }

        private bool LoadDirectory()
        {
            return LoadDirectory(currentDirectory);
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

            currentDirectory = directory;
            OnDirectorySelected(currentDirectory);
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

                contextMenu.Show(location, currentDirectory.FullName);
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
            e.Text = currentDirectory.FullName;
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

        public override void Refresh()
        {
            base.Refresh();
            
            LoadDirectory();
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            bool directoryUpdated = false;
            if (e.Control && !e.Shift && !e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                    case Keys.Insert:
                        // Copy
                        {
                            this.Copy();
                            break;
                        }                        
                    case Keys.V:
                        // Paste
                        {
                            this.Paste();
                            break;
                        }
                    case Keys.X:
                        // Cut
                        {
                            this.Cut();
                            break;
                        }
                        break;

                    case Keys.A:
                        // Select All
                        {
                            break;
                        }
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        // Paste
                        if (e.Shift && !e.Control && !e.Alt)
                        {
                            this.Paste();
                        }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;

                    case Keys.Delete:
                        // Delete
                        if (!e.Control && !e.Alt)
                        {
                            this.Delete();
                        }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }
    }
}
