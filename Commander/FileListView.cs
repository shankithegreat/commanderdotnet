using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace Commander
{
    public partial class FileListView : ListView
    {
        private ThumbnailCreator thumbnailCreator = new ThumbnailCreator();
        private ShellContextMenu contextMenu = new ShellContextMenu();
        private DirectoryInfo currentDirectory;
        private List<FileSystemInfo> list = new List<FileSystemInfo>();
        private Dictionary<int, ListViewItem> items = new Dictionary<int, ListViewItem>();
        private BrowserDragWrapper dragWrapper;
        private BrowserDropWrapper dropWrapper;
        private delegate void UpdateEventHandler();


        public FileListView()
        {
            InitializeComponent();

            this.Update += LoadDirectory;

            this.typeColumn.Tag = new GetColumntContentEventHandler(GetFileType);
            this.sizeColumn.Tag = new GetColumntContentEventHandler(GetFileSize);
            this.dateColumn.Tag = new GetColumntContentEventHandler(GetFileDate);

            this.ColoringCompressed = true;
            this.RetrieveVirtualItem += FileListView_RetrieveVirtualItem;

            this.thumbnailCreator.DesiredSize = new Size(256, 256);
            this.largeImageList.ImageSize = thumbnailCreator.DesiredSize;

            this.dragWrapper = new BrowserDragWrapper(this);
            this.dropWrapper = new BrowserDropWrapper(this);

            this.dragWrapper.DragEnd += delegate(object sender, EventArgs e)
            {
                currentDirectory.Refresh();
            };

            this.dropWrapper.Drop += delegate(object sender, DropEventArgs e)
            {
                currentDirectory.Refresh();
            };
        }

        /// <summary>
        /// Gets or sets the directory which is displayed in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DirectoryInfo CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
            set
            {
                if (!Utility.IoHelper.Equals(value, this.currentDirectory))
                {
                    LoadDirectory(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets how hiden items are displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public bool ShowHiden { get; set; }

        /// <summary>
        /// Gets or sets how compressed items are displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public bool ColoringCompressed { get; set; }


        public event DirectorySelectedEventHandler CurrentDirectoryChanged;

        private event UpdateEventHandler Update;


        public void Copy()
        {
            contextMenu.CopyCommand(GetSelected());
        }

        public void Paste()
        {
            contextMenu.PasteCommand(CurrentDirectory);
        }

        public void Cut()
        {
            contextMenu.CutCommand(GetSelected());
        }

        public void Delete()
        {
            contextMenu.DeleteCommand(GetSelected());
        }

        public void ShowCurrentContextMenu(Point location)
        {
            contextMenu.Show(location, currentDirectory.FullName);
        }

        public FileSystemInfo[] GetSelected()
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            foreach (int index in this.SelectedIndices)
            {
                list.Add(GetFileSystemInfo(index));
            }
            return list.ToArray();
        }

        public FileSystemInfo GetFileSystemInfo(ListViewItem item)
        {
            return GetFileSystemInfo(item.Index);
        }

        public FileSystemInfo GetFileSystemInfo(int index)
        {
            return this.list[index] ?? this.CurrentDirectory;
        }

        public DirectoryInfo GetSelectedDirectory()
        {
            if (this.SelectedIndices.Count == 1)
            {
                FileSystemInfo item = this.list[this.SelectedIndices[0]] ?? this.CurrentDirectory;
                if (item is DirectoryInfo)
                {
                    return (DirectoryInfo)item;
                }
            }
            return currentDirectory;
        }

        public FileSystemInfo GetFocusedItem()
        {
            ListViewItem item = this.FocusedItem;
            if (item != null)
            {
                return GetFileSystemInfo(item);
            }
            return null;
        }


        public FileSystemInfo GetFileSystemItemFromPoint(Point point)
        {
            ListViewHitTestInfo hitTest = this.HitTest(point);
            if (hitTest.Item != null)
            {
                ListViewItem item = hitTest.Item;
                return GetFileSystemInfo(item);
            }
            else
            {
                return currentDirectory;
            }
        }

        protected DirectoryInfo GetDirectoryFromPoint(Point point)
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

        protected virtual void OnCurrentDirectoryChanged(DirectoryInfo directory)
        {
            if (CurrentDirectoryChanged != null)
            {
                fileSystemWatcher.Path = directory.FullName;
                fileSystemWatcher.EnableRaisingEvents = true;
                CurrentDirectoryChanged(this, directory);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Assign the image lists to the ListView
            ShellImageList.Set32SmallImageList(this);
            ShellImageList.SetLargeImageList(this);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.SelectedIndices.Count > 0)
                {
                    Point location = this.PointToScreen(e.Location);

                    List<string> list = new List<string>(this.SelectedIndices.Count);
                    foreach (FileSystemInfo fsi in GetSelected())
                    {
                        list.Add(fsi.FullName);
                    }

                    contextMenu.Show(location, list.ToArray());
                }
            }

            base.OnMouseUp(e);
        }

        protected override void OnItemActivate(EventArgs e)
        {
            FileSystemInfo fsi = this.list[this.SelectedIndices[0]] ?? this.CurrentDirectory.Parent;
            if (fsi is DirectoryInfo)
            {
                DirectoryInfo directory = (DirectoryInfo)fsi;
                try
                {
                    LoadDirectory(directory);
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
            else
            {
                contextMenu.DefaultCommand((FileInfo)fsi);
            }

            base.OnItemActivate(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && !e.Shift && !e.Alt)
            {
                switch (e.KeyCode)
                {
                    // Copy
                    case Keys.C:
                    case Keys.Insert:
                        {
                            this.Copy();
                            break;
                        }
                    // Paste
                    case Keys.V:
                        {
                            this.Paste();
                            break;
                        }
                    // Cut
                    case Keys.X:
                        {
                            this.Cut();
                            break;
                        }
                    // Select All
                    case Keys.A:
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
                    // Paste
                    case Keys.Insert:
                        {
                            if (e.Shift && !e.Control && !e.Alt)
                            {
                                this.Paste();
                            }

                            e.Handled = true;
                            e.SuppressKeyPress = true;

                            break;
                        }
                    // Delete
                    case Keys.Delete:
                        {
                            if (!e.Control && !e.Alt)
                            {
                                this.Delete();
                            }

                            e.Handled = true;
                            e.SuppressKeyPress = true;

                            break;
                        }

                    case Keys.Space:
                        {
                            if (!e.Shift && !e.Alt)
                            {
                                FileSystemInfo fsi = this.GetFocusedItem();
                                if (fsi is DirectoryInfo)
                                {
                                    DirectoryInfo di = (DirectoryInfo)fsi;
                                    ListViewItem item = this.FocusedItem;
                                    long size = Utility.IoHelper.GetDirectorySize(di);
                                    this.Cursor = Cursors.WaitCursor;
                                    try
                                    {
                                        item.SubItems[sizeColumn.Index].Text = size.ToString("N0");
                                    }
                                    finally
                                    {
                                        this.Cursor = Cursors.Arrow;
                                    }
                                    this.Invalidate(item.Bounds, false);
                                }
                            }

                            e.Handled = true;
                            e.SuppressKeyPress = true;

                            break;
                        }
                    case Keys.Enter:
                        {
                            if (e.Alt && e.Shift)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                try
                                {
                                    int index = 0;
                                    foreach (FileSystemInfo fsi in this.list)
                                    {
                                        if (fsi is DirectoryInfo)
                                        {
                                            DirectoryInfo di = (DirectoryInfo)fsi;
                                            long size = Utility.IoHelper.GetDirectorySize(di);
                                            ListViewItem item = GetItem(index);
                                            item.SubItems[sizeColumn.Index].Text = size.ToString("N0");
                                            this.Refresh();
                                        }
                                        index++;
                                    }
                                }
                                finally
                                {
                                    this.Cursor = Cursors.Arrow;
                                }

                            }

                            e.Handled = true;
                            e.SuppressKeyPress = true;

                            break;
                        }
                }
            }

            base.OnKeyDown(e);
        }

        protected virtual void OnUpdate()
        {
            if (Update != null)
            {
                Update();
            }
        }


        private void LoadDirectory()
        {
            LoadDirectory(currentDirectory);
        }

        private void LoadDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                ReadDirectoryItems(directory);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new DirectoryNotFoundException(e.Message, e);
            }

            FillListView();

            this.currentDirectory = directory;
            OnCurrentDirectoryChanged(currentDirectory);
        }

        private void FillListView()
        {
            this.BeginUpdate();
            this.largeImageList.Images.Clear();

            if (list.Count > 0)
            {
                int count = this.list.Count;
                try
                {
                    this.VirtualListSize = count;
                }
                catch (NullReferenceException)
                {
                }
            }
            this.VirtualMode = this.list.Count > 0;

            if (list.Count > 0)
            {
                this.SelectedIndices.Clear();
                ListViewItem item = GetItem(0);
                item.Selected = true;
                this.FocusedItem = item;
                this.SelectedIndices.Add(0);
            }

            this.EndUpdate();
        }

        private ListViewItem CreateItem(int index)
        {
            return CreateItem(this.list[index]);
        }

        private ListViewItem CreateItem(FileSystemInfo fsi)
        {
            ListViewItem item;

            if (fsi == null)
            {
                item = new ListViewItem("..");

                for (int i = 1; i < this.Columns.Count; i++)
                {
                    item.SubItems.Add(string.Empty);
                }
            }
            else
            {
                item = new ListViewItem(fsi.Name, GetImageIndex(fsi));

                if ((fsi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    item.ForeColor = SystemColors.InactiveCaptionText;
                }
                else if (ColoringCompressed && (fsi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed)
                {
                    item.ForeColor = Color.Blue;
                }

                for (int i = 1; i < this.Columns.Count; i++)
                {
                    item.SubItems.Add(GetColumnContext(this.Columns[i], fsi));
                }

            }

            return item;
        }

        private string GetColumnContext(ColumnHeader column, FileSystemInfo fsi)
        {
            string result = string.Empty;

            GetColumntContentEventHandler handler = column.Tag as GetColumntContentEventHandler;

            if (handler != null)
            {
                return handler(column, fsi);
            }

            return result;
        }

        private string GetFileType(ColumnHeader column, FileSystemInfo fsi)
        {
            if (fsi is FileInfo)
            {
                if (string.IsNullOrEmpty(fsi.Extension))
                {
                    return fsi.Extension;
                }

                return fsi.Extension.Remove(0, 1);
            }

            return string.Empty;
        }

        private string GetFileSize(ColumnHeader column, FileSystemInfo fsi)
        {
            if (fsi is FileInfo)
            {
                FileInfo file = (FileInfo)fsi;
                return file.Length.ToString("N0");
            }

            return string.Empty;
        }

        private string GetFileDate(ColumnHeader column, FileSystemInfo fsi)
        {
            return fsi.CreationTime.ToString("dd.MM.yyyy hh:mm");
        }

        private List<FileSystemInfo> ReadDirectoryItems(DirectoryInfo directory)
        {
            directory.Refresh();
            FileSystemInfo[] items = directory.GetFileSystemInfos();

            this.list.Clear();
            this.items.Clear();

            if (directory.Parent != null)
            {
                this.list.Add(null);
            }

            foreach (FileSystemInfo fsi in items)
            {
                if ((fsi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden || this.ShowHiden)
                {
                    this.list.Add(fsi);
                }
            }

            return this.list;
        }

        private void FileListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = GetItem(e.ItemIndex);
        }

        private ListViewItem GetItem(int index)
        {
            if (this.items.ContainsKey(index))
            {
                return this.items[index];
            }
            else
            {
                ListViewItem item = CreateItem(index);
                this.items.Add(index, item);
                return item;
            }
        }

        private void InitImageList(View view)
        {
            if (this.Parent != null)
            {
                if (view == View.LargeIcon)
                {
                    ShellImageList.SetLargeImageList(this, largeImageList.Handle);
                }
                else
                {
                    ShellImageList.SetLargeImageList(this);
                }
            }
        }

        private int GetImageIndex(FileSystemInfo item)
        {
            switch (this.View)
            {
                case View.LargeIcon:
                    {
                        /*try
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

                        return -1;*/

                        return SafeNativeMethods.GetLargeAssociatedIconIndex(item.FullName);
                    }
                default:
                    {
                        return SafeNativeMethods.GetSmallAssociatedIconIndex(item.FullName);
                    }
            }
        }

        private void SelectAll()
        {
            if (this.Items.Count >= 1)
            {
                this.Items[0].Selected = (currentDirectory.Parent == null);
            }

            for (int i = 1; i < this.Items.Count; i++)
            {
                ListViewItem item = this.Items[i];
                item.Selected = true;
            }
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            OnUpdate();
        }

        private void fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            OnUpdate();
        }
    }


    public delegate string GetColumntContentEventHandler(ColumnHeader column, FileSystemInfo fsi);
}
