using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ShellDll;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace Commander
{
    /// <summary>
    /// This class takes care of every drag operation in a BrowserListView
    /// </summary>
    internal class BrowserDragWrapper : IDropSource, IDisposable
    {
        #region Fields

        // The browser for which to do the drag work
        private FileListView listView;

        // The pointer to the IDataObject being dragged
        private IntPtr dataObjectPtr;

        // The mouseButtons state when a drag starts
        private MouseButtons startButton;

        // A bool to indicate whether this class has been disposed
        private bool disposed = false;

        // The events that will be raised when a drag starts and when it ends
        public event DragEnterEventHandler DragStart;
        public event EventHandler DragEnd;

        #endregion

        /// <summary>
        /// Registers the ListView.ItemDrag to receive the event when an item is being dragged
        /// </summary>
        /// <param name="br">The browser for which to support the drag</param>
        public BrowserDragWrapper(FileListView listView)
        {
            this.listView = listView;
            this.listView.AllowDrop = true;
            this.listView.ItemDrag += ItemDrag;
        }

        ~BrowserDragWrapper()
        {
            ((IDisposable)this).Dispose();
        }

        #region Generated Events

        /// <summary>
        /// The event that is being raised when a drag starts
        /// </summary>
        /// <param name="e">the DragEnterEventArgs for the event</param>
        private void OnDragStart(DragEnterEventArgs e)
        {
            if (DragStart != null)
                DragStart(this, e);
        }

        /// <summary>
        /// The event that is being raised when a drag ends
        /// </summary>
        /// <param name="e">the EventArgs for the event</param>
        private void OnDragEnd(EventArgs e)
        {
            if (DragEnd != null)
                DragEnd(this, e);
        }

        #endregion

        #region IDropSource Members

        ShellBrowser browser = new ShellBrowser();

        /// <summary>
        /// This method initialises the dragging of a ListViewItem
        /// </summary>
        void ItemDrag(object sender, ItemDragEventArgs e)
        {
            ReleaseCom();

            startButton = e.Button;

            FileSystemInfo[] items = listView.GetSelected();

            List<ShellItem> list = new List<ShellItem>();
            foreach (FileSystemInfo f in items)
            {
                //IntPtr[] pidls = ShellFolder.GetPIDLs(items);
                IntPtr pidl = ShellFolder.GetPathPIDL(f);
                //IShellFolder parentShellFolder = ShellFolder.GetParentShellFolder(items[0]);
                string parentDirectory = ShellFolder.GetParentDirectoryPath(f);
                IntPtr parentShellFolder = ShellFolder.GetShellFolderIntPtr(parentDirectory);


                ShellItem item = new ShellItem(browser, pidl, parentShellFolder);
                list.Add(item);
            }

            dataObjectPtr = ShellHelper.GetIDataObject(list.ToArray());

            if (dataObjectPtr != IntPtr.Zero)
            {
                DragDropEffects effects;
                //OnDragStart(new DragEnterEventArgs((items[0].ParentItem != null ? items[0].ParentItem : items[0]), fileView.ListView));
                ShellAPI.DoDragDrop(dataObjectPtr, this, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move, out effects);
                //OnDragEnd(new EventArgs());
            }
        }

        public int QueryContinueDrag(bool fEscapePressed, ShellAPI.MK grfKeyState)
        {
            if (fEscapePressed)
                return ShellAPI.DRAGDROP_S_CANCEL;
            else
            {
                if ((startButton & MouseButtons.Left) != 0 && (grfKeyState & ShellAPI.MK.LBUTTON) == 0)
                    return ShellAPI.DRAGDROP_S_DROP;
                else if ((startButton & MouseButtons.Right) != 0 && (grfKeyState & ShellAPI.MK.RBUTTON) == 0)
                    return ShellAPI.DRAGDROP_S_DROP;
                else
                    return ShellAPI.S_OK;
            }
        }

        public int GiveFeedback(DragDropEffects dwEffect)
        {
            return ShellAPI.DRAGDROP_S_USEDEFAULTCURSORS;
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
                ReleaseCom();
                GC.SuppressFinalize(this);

                disposed = true;
            }
        }

        /// <summary>
        /// Release the IDataObject and free's the allocated memory
        /// </summary>
        private void ReleaseCom()
        {
            if (dataObjectPtr != IntPtr.Zero)
            {
                Marshal.Release(dataObjectPtr);
                dataObjectPtr = IntPtr.Zero;
            }
        }

        #endregion
    }

    internal delegate void DragEnterEventHandler(object sender, DragEnterEventArgs e);

    internal class DragEnterEventArgs : EventArgs
    {
        private IShellFolder parent;
        private Control dragStartControl;

        public DragEnterEventArgs(IShellFolder parent, Control dragStartControl)
        {
            this.parent = parent;
            this.dragStartControl = dragStartControl;
        }

        public IShellFolder Parent { get { return parent; } }
        public Control DragStartControl { get { return dragStartControl; } }
    }
}
