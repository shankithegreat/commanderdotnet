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
        //private ShellDrag dragClass;
        //private ShellDrop dropClass;
        //private BrowserLVDropWrapper dw;
        //private BrowserLVDragWrapper drw;
        ThumbnailCreator thumbnailCreator = new ThumbnailCreator();

        public FileView()
        {
            InitializeComponent();

            thumbnailCreator.DesiredSize = new Size(256, 256);

            this.HandleCreated += new EventHandler(FileView_HandleCreated);

            ShellImageList.SetSmallImageList(listView);
            //ShellImageList.SetLargeImageList(listView);
            //listView.LargeImageList = largeImageList;
            largeImageList.ImageSize = thumbnailCreator.DesiredSize;

            if (this.Focused)
            {
                FileView_Enter(this, null);
            }
            else
            {
                FileView_Leave(this, null);
            }
        }

        private void FileView_HandleCreated(object sender, EventArgs e)
        {
            //dropClass = new ShellDrop(listView);
            //dropClass.RetrieveDestinationRirectory += new RetrieveDestinationRirectoryEventHandler(dropClass_RetrieveDestinationRirectory);
            //dropClass.Drop += new DropEventHandler(dropClass_Drop);
            //drw = new BrowserLVDragWrapper(this);
            //dw = new BrowserLVDropWrapper(this);                
            //dragClass = new ShellDrag();
        }

        private void dropClass_Drop(object sender, DropEventArgs e)
        {
        }

        internal void dropClass_RetrieveDestinationRirectory(object sender, RetrieveDestinationRirectoryEventArgs e)
        {
            e.DestinationDirectory = GetDirectoryFromPoint(e.Point);
        }

        public FileSystemInfo GetFileSystemItemFromPoint(Point point)
        {
            ListViewHitTestInfo hitTest = listView.HitTest(point);
            if (hitTest.Item != null)
            {
                ListViewItem item = hitTest.Item;
                return (FileSystemInfo)item.Tag;
            }
            else
            {
                return currentDirectory;
            }
        }

        public DirectoryInfo GetDirectoryFromPoint(Point point)
        {
            FileSystemInfo item = GetFileSystemItemFromPoint(point);
            if (item is DirectoryInfo)
            {
                return (DirectoryInfo)item;
            }
            else
            {
                return currentDirectory;
            }
        }

        internal ShellListView ListView
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

        public FileSystemInfo GetFocusedItem()
        {
            ListViewItem item = listView.FocusedItem;
            if (item != null)
            {
                FileSystemInfo fsi = (FileSystemInfo)item.Tag;
                return fsi;
            }
            return null;
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
            largeImageList.Images.Clear();
            listView.Tag = directory;

            FillListView(directory, list);

            currentDirectory = directory;
            titleLabel.Text = GetTitleLabelText(currentDirectory);
            OnDirectorySelected(currentDirectory);
            return true;
        }

        private void FillListView(DirectoryInfo directory, FileSystemInfo[] list)
        {
            if (directory.Parent != null)
            {
                ListViewItem item = listView.Items.Add("..");
                item.Tag = directory.Parent;
            }

            foreach (FileSystemInfo fsi in list)
            {
                ListViewItem item = listView.Items.Add(fsi.Name, GetImageIndex(fsi));
                item.Tag = fsi;
            }
        }

        private int GetImageIndex(FileSystemInfo item)
        {
            if (listView.View == View.LargeIcon)
            {
                try
                {
                    Bitmap bmp = thumbnailCreator.GetThumbnail(item.FullName);
                    if (bmp != null)
                    {
                        return largeImageList.Images.Add(bmp, Color.Transparent);
                    }
                }
                catch
                {
                }
                Icon icon = SafeNativeMethods.GetLargeAssociatedIcon(item.FullName);
                if (icon != null)
                {
                    largeImageList.Images.Add(icon);
                    return largeImageList.Images.Count - 1;
                }
                return -1;
            }
            return SafeNativeMethods.GetAssociatedIconIndex(item.FullName);
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
            if (DirectorySelected != null)
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
                            SelectAll();
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
            OnKeyDown(e);
        }

        private void SelectAll()
        {
            if (listView.Items.Count >= 1)
            {
                listView.Items[0].Selected = (currentDirectory.Parent == null);
            }
            for (int i = 1; i < listView.Items.Count; i++)
            {
                ListViewItem item = listView.Items[i];
                item.Selected = true;
            }
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //dragClass.DragCommand(e.Button, GetSelected());
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            /*string[] ss = e.Data.GetFormats();
            ShellDll.IDataObject d = (ShellDll.IDataObject)e.Data.GetData(typeof(ShellDll.IDataObject));
            d.
            IntPtr obj = (IntPtr)e.Data.GetData(typeof(IntPtr));
            DragDropEffects effects = e.Effect;
            dropClass.DragEnter(obj, (ShellAPI.MK)e.KeyState, new ShellAPI.POINT(e.X, e.Y), ref effects);
            e.Effect = effects;*/
        }


        public void SetView(View view)
        {
            if (view == View.LargeIcon)
            {
                //listView.LargeImageList = largeImageList;
                ShellImageList.SetLargeImageList(listView, largeImageList.Handle);
            }
            else
            {
                ShellImageList.SetLargeImageList(listView);
            }
            listView.View = view;
            if (view == View.LargeIcon)
            {
                LoadDirectory();
            }
        }
        
    }

       

}
