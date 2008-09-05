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
    internal class BrowserLVDragWrapper : IDropSource, IDisposable
    {
        #region Fields

        // The browser for which to do the drag work
        private FileView fileView;

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
        public BrowserLVDragWrapper(FileView br)
        {
            this.fileView = br;
            fileView.ListView.ItemDrag += new ItemDragEventHandler(ItemDrag);
        }

        ~BrowserLVDragWrapper()
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

            FileSystemInfo[] items = fileView.GetSelected();

            List<ShellItem> list = new List<ShellItem>();
            foreach (FileSystemInfo f in items)
            {
                //IntPtr[] pidls = ShellFolder.GetPIDLs(items);
                IntPtr pidl = ShellFolder.GetPathPIDL(items[0]);
                //IShellFolder parentShellFolder = ShellFolder.GetParentShellFolder(items[0]);
                string parentDirectory = ShellFolder.GetParentDirectoryPath(items[0]);
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

    #region Event Classes

    

    #endregion
}
