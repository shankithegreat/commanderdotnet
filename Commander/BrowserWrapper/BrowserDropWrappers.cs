using System;
using System.Collections.Generic;
using System.Text;
using ShellDll;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Commander
{

    /// <summary>
    /// This class takes care of every drop operation in a BrowserListView
    /// </summary>
    internal class BrowserDropWrapper : ShellDll.IDropTarget, IDisposable
    {
        #region Fields

        // The browser for which to do the drop work
        private FileListView listView;

        private IntPtr listViewHandle;

        // The current IDropTarget the cursor is over and the pointers to the target and dataobject
        private ShellDll.IDropTarget dropTarget;
        private IntPtr dropTargetPtr;
        private IntPtr dropDataObject;

        private IDropTargetHelper dropHelper;
        private IntPtr dropHelperPtr;

        // The current ListViewItem the cursor is over to drop on
        private ListViewItem dropListItem;

        // The selected state from the dropListItem before the cursor moved over it
        private bool wasSelected;

        // The parent ShellItems of the drop- and dragitem
        private ShellItem parentDropItem, parentDragItem;

        // The mouse and keys state and DragDropEffects when a drag enter occurs
        private ShellAPI.MK mouseButtons;
        private DragDropEffects startEffects;

        // A bool to indicate whether this class has been disposed
        private bool disposed = false;

        // The event for when a drop is occuring
        public event DropEventHandler Drop;

        private ShellBrowser browser = new ShellBrowser();

        #endregion

        /// <summary>
        /// Registers the ListView for drag/drop operations and uses this class as the IDropTarget
        /// </summary>
        /// <param name="br">The browser for which to support the drop</param>
        public BrowserDropWrapper(FileListView listView)
        {
            this.listView = listView;
            this.listView.AllowDrop = true;

            listViewHandle = this.listView.Handle;
            //ShellAPI.RegisterDragDrop(this.listView.Handle, this);

            listView.HandleCreated += new EventHandler(FileView_HandleCreated);
            listView.HandleDestroyed += new EventHandler(FileView_HandleDestroyed);
            listView.DragEnter += new DragEventHandler(listView_DragEnter);
            listView.DragOver += new DragEventHandler(listView_DragOver);
            listView.DragLeave += new EventHandler(listView_DragLeave);
            listView.DragDrop += new DragEventHandler(listView_DragDrop);

            ShellHelper.GetIDropTargetHelper(out dropHelperPtr, out dropHelper);
        }

        void listView_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            IntPtr data = GetIDataObject(files);
            ShellAPI.MK keyState = (ShellAPI.MK)(int)e.KeyState;
            ShellAPI.POINT point = new ShellAPI.POINT(e.X, e.Y);
            DragDropEffects effects = e.AllowedEffect;

            DragEnter(data, keyState, point, ref effects);

            e.Effect = effects;
        }

        void listView_DragOver(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            IntPtr data = GetIDataObject(files);
            ShellAPI.MK keyState = (ShellAPI.MK)(int)e.KeyState;
            ShellAPI.POINT point = new ShellAPI.POINT(e.X, e.Y);
            DragDropEffects effects = e.AllowedEffect; ;

            DragOver(keyState, point, ref effects);

            e.Effect = effects;
        }

        void listView_DragLeave(object sender, EventArgs e)
        {
            DragLeave();
        }

        void listView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            IntPtr data = GetIDataObject(files);
            ShellAPI.MK keyState = (ShellAPI.MK)(int)e.KeyState;
            ShellAPI.POINT point = new ShellAPI.POINT(e.X, e.Y);
            DragDropEffects effects = e.AllowedEffect;

            DragDrop(data, keyState, point, ref effects);

            e.Effect = effects;
        }

        private IntPtr GetIDataObject(string[] fileDropData)
        {
            List<ShellItem> list = new List<ShellItem>();
            foreach (string f in fileDropData)
            {
                IntPtr pidl = ShellFolder.GetPathPIDL(f);
                string parentDirectory = ShellFolder.GetParentDirectoryPath(f);
                IntPtr parentShellFolder = ShellFolder.GetShellFolderIntPtr(parentDirectory);


                ShellItem item = new ShellItem(browser, pidl, parentShellFolder);
                list.Add(item);
            }

            return ShellHelper.GetIDataObject(list.ToArray());
        }

        ~BrowserDropWrapper()
        {
            ((IDisposable)this).Dispose();
        }

        #region Handle Changes

        void FileView_HandleCreated(object sender, EventArgs e)
        {
            listViewHandle = listView.Handle;
            ShellAPI.RegisterDragDrop(listViewHandle, this);
        }

        void FileView_HandleDestroyed(object sender, EventArgs e)
        {
            ShellAPI.RevokeDragDrop(listViewHandle);
        }

        #endregion

        #region Public

        /// <summary>
        /// This ShellItem is the parent item of the item being currently dragged. This field is used
        /// to check whether an item is being moved to it's original folder. If this is the case, we don't
        /// have to do anything, cause the item is allready there.
        /// </summary>
        public ShellItem ParentDragItem
        {
            get { return parentDragItem; }
            set { parentDragItem = value; }
        }

        #endregion

        #region Generated Events

        /// <summary>
        /// This event will be raised whenever a drop occurs on the ListView.
        /// </summary>
        /// <param name="e">The DropEventArgs for the event</param>
        private void OnDrop(DropEventArgs e)
        {
            if (Drop != null)
                Drop(this, e);
        }

        #endregion

        #region IDropTarget Members

        public int DragEnter(IntPtr pDataObj, ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {
            mouseButtons = grfKeyState;
            startEffects = pdwEffect;

            listView.Focus();
            //fileView.SelectionChange = false;
            ReleaseCom();

            dropDataObject = pDataObj;

            #region Get DropItem
            Point point = listView.PointToClient(new Point(pt.x, pt.y));
            ListViewHitTestInfo hitTest = listView.HitTest(point);
            if (hitTest.Item != null &&
                (listView.View != View.Details || hitTest.SubItem == null || hitTest.Item.Name == hitTest.SubItem.Name) &&
                (hitTest.Location == ListViewHitTestLocations.Image ||
                 hitTest.Location == ListViewHitTestLocations.Label ||
                 hitTest.Location == ListViewHitTestLocations.StateImage))
            {
                dropListItem = hitTest.Item;

                wasSelected = dropListItem.Selected;
                dropListItem.Selected = true;

                ShellItem item = GetShellItem(hitTest.Item);
                parentDropItem = item;

                ShellHelper.GetIDropTarget(item, out dropTargetPtr, out dropTarget);
            }
            else
            {
                dropListItem = null;
                ShellItem item = GetShellItem(listView.CurrentDirectory);
                parentDropItem = item;
                ShellHelper.GetIDropTarget(item, out dropTargetPtr, out dropTarget);
            }
            #endregion

            if (dropTarget != null)
                dropTarget.DragEnter(pDataObj, grfKeyState, pt, ref pdwEffect);

            if (dropHelper != null)
                dropHelper.DragEnter(listView.Handle, pDataObj, ref pt, pdwEffect);

            return ShellAPI.S_OK;
        }

        public int DragOver(ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {
            bool reset = false;

            #region Get DropItem

            Point point = listView.PointToClient(new Point(pt.x, pt.y));
            ListViewHitTestInfo hitTest = listView.HitTest(point);
            if (hitTest.Item != null &&
                (listView.View != View.Details || hitTest.SubItem == null || hitTest.Item.Name == hitTest.SubItem.Name) &&
                (hitTest.Location == ListViewHitTestLocations.Image ||
                 hitTest.Location == ListViewHitTestLocations.Label ||
                 hitTest.Location == ListViewHitTestLocations.StateImage))
            {
                if (!hitTest.Item.Equals(dropListItem))
                {
                    if (dropTarget != null)
                        dropTarget.DragLeave();

                    ReleaseCom();

                    if (dropListItem != null)
                        dropListItem.Selected = wasSelected;

                    dropListItem = hitTest.Item;
                    wasSelected = dropListItem.Selected;
                    dropListItem.Selected = true;

                    ShellItem item = GetShellItem(hitTest.Item);
                    parentDropItem = item;

                    ShellHelper.GetIDropTarget(item, out dropTargetPtr, out dropTarget);
                    reset = true;
                }
            }
            else
            {
                if (dropListItem != null)
                {
                    if (dropTarget != null)
                        dropTarget.DragLeave();

                    ReleaseCom();

                    dropListItem.Selected = wasSelected;

                    dropListItem = null;

                    ShellItem item = GetShellItem(listView.CurrentDirectory);
                    parentDropItem = item;
                    ShellHelper.GetIDropTarget(item, out dropTargetPtr, out dropTarget);

                    reset = true;
                }
            }

            #endregion

            if (dropTarget != null)
            {
                if (reset)
                {
                    dropTarget.DragEnter(dropDataObject, grfKeyState, pt, ref pdwEffect);
                }
                else
                    dropTarget.DragOver(grfKeyState, pt, ref pdwEffect);
            }
            else
                pdwEffect = DragDropEffects.None;

            if (dropHelper != null)
                dropHelper.DragOver(ref pt, pdwEffect);

            return ShellAPI.S_OK;
        }

        private ShellItem GetShellItem(ListViewItem item)
        {
            FileSystemInfo fsi = listView.GetFileSystemInfo(item);
            return GetShellItem(fsi);
        }

        private ShellItem GetShellItem(FileSystemInfo fsi)
        {
            IntPtr pidl = ShellFolder.GetPathPIDL(fsi);
            string parentDirectory = ShellFolder.GetParentDirectoryPath(fsi);
            IntPtr parentShellFolder = ShellFolder.GetShellFolderIntPtr(parentDirectory);

            return new ShellItem(browser, pidl, parentShellFolder);
        }

        public int DragLeave()
        {
            ResetDrop();
            if (dropTarget != null)
            {
                dropTarget.DragLeave();

                ReleaseCom();
                dropDataObject = IntPtr.Zero;
            }

            if (dropHelper != null)
                dropHelper.DragLeave();

            return ShellAPI.S_OK;
        }

        public int DragDrop(IntPtr pDataObj, ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {
            OnDrop(new DropEventArgs(mouseButtons, listView));

            if (!((mouseButtons & ShellAPI.MK.RBUTTON) != 0 ||
                  grfKeyState == ShellAPI.MK.CONTROL ||
                  grfKeyState == ShellAPI.MK.ALT ||
                  grfKeyState == (ShellAPI.MK.CONTROL | ShellAPI.MK.SHIFT)) && ShellItem.Equals(parentDragItem, parentDropItem))
            {
                ResetDrop();
                ReleaseCom();
                pdwEffect = DragDropEffects.None;

                if (dropHelper != null)
                    dropHelper.Drop(pDataObj, ref pt, pdwEffect);

                return ShellAPI.S_OK;
            }

            ResetDrop();
            if (dropTarget != null)
            {
                dropTarget.DragDrop(pDataObj, grfKeyState, pt, ref pdwEffect);

                ReleaseCom();
                dropDataObject = IntPtr.Zero;
            }

            if (dropHelper != null)
                dropHelper.Drop(pDataObj, ref pt, pdwEffect);

            return ShellAPI.S_OK;
        }

        /// <summary>
        /// Reset all fields to the default values and release the IDropTarget
        /// </summary>
        private void ResetDrop()
        {
            if (dropListItem != null)
            {
                dropListItem.Selected = wasSelected;
                dropListItem = null;
                parentDropItem = null;
            }

            //fileView.SelectionChange = true;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// If not disposed, dispose the class
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                DisposeDropWrapper();
                GC.SuppressFinalize(this);

                disposed = true;
            }
        }

        /// <summary>
        /// Revokes the ListView from getting drop messages and releases the IDropTarget
        /// </summary>
        private void DisposeDropWrapper()
        {
            ReleaseCom();

            if (dropHelper != null)
            {
                Marshal.ReleaseComObject(dropHelper);
            }
        }

        /// <summary>
        /// Release the IDropTarget and free's the allocated memory
        /// </summary>
        private void ReleaseCom()
        {
            if (dropTarget != null)
            {
                Marshal.ReleaseComObject(dropTarget);

                dropTarget = null;
                dropHelperPtr = IntPtr.Zero;
            }
        }

        #endregion
    }

    internal delegate void DropEventHandler(object sender, DropEventArgs e);

    internal class DropEventArgs : EventArgs
    {
        private ShellAPI.MK mouseButtons;
        private Control dragStartControl;

        public DropEventArgs(ShellAPI.MK mouseButtons, Control dragStartControl)
        {
            this.mouseButtons = mouseButtons;
            this.dragStartControl = dragStartControl;
        }

        public ShellAPI.MK MouseButtons { get { return mouseButtons; } }
        public Control DragStartControl { get { return dragStartControl; } }
    }
}
