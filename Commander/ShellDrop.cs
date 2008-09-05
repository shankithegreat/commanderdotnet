using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ShellDll;

namespace Commander
{
    internal delegate void DropEventHandler(object sender, DropEventArgs e);
    internal delegate void RetrieveDestinationRirectoryEventHandler(object sender, RetrieveDestinationRirectoryEventArgs e);

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

    internal class RetrieveDestinationRirectoryEventArgs : EventArgs
    {
        private Point point;
        private DirectoryInfo directory = null;

        public RetrieveDestinationRirectoryEventArgs(Point point)
        {
            this.point = point;
        }

        public Point Point
        {
            get
            {
                return point;
            }
        }

        public DirectoryInfo DestinationDirectory
        {
            get
            {
                return directory;
            }
            set
            {
                directory = value;
            }
        }
    }

    

    /// <summary>
    /// This class takes care of every drop operation in a BrowserListView
    /// </summary>
    internal class ShellDrop : ShellDll.IDropTarget, IDisposable
    {
        private ListView owner;
        private IntPtr handle;

        // The current IDropTarget the cursor is over and the pointers to the target and dataobject
        private ShellDll.IDropTarget dropTarget;
        private IntPtr dropTargetPtr;
        private IntPtr dropDataObject;

        private IDropTargetHelper dropHelper;
        private IntPtr dropHelperPtr;

        // The current ListViewItem the cursor is over to drop on
        //private ListViewItem dropListItem;

        // The selected state from the dropListItem before the cursor moved over it
        //private bool wasSelected;

        // The parent ShellItems of the drop- and dragitem
        //private ShellItem parentDropItem, parentDragItem;

        // The mouse and keys state and DragDropEffects when a drag enter occurs
        private ShellAPI.MK mouseButtons;
        private DragDropEffects startEffects;

        // A bool to indicate whether this class has been disposed
        private bool disposed = false;

        // The event for when a drop is occuring
        public event DropEventHandler Drop;
        public event RetrieveDestinationRirectoryEventHandler RetrieveDestinationRirectory;

        
        /// <summary>
        /// Registers the ListView for drag/drop operations and uses this class as the IDropTarget
        /// </summary>
        /// <param name="br">The browser for which to support the drop</param>
        public ShellDrop(ListView owner)
        {
            this.owner = owner;
            this.handle = owner.Handle;
            
            ShellAPI.RegisterDragDrop(this.handle, this);

            owner.HandleCreated += new EventHandler(owner_HandleCreated);
            owner.HandleDestroyed += new EventHandler(owner_HandleDestroyed);

            ShellHelper.GetIDropTargetHelper(out dropHelperPtr, out dropHelper);
        }

        ~ShellDrop()
        {
            ((IDisposable)this).Dispose();
        }

        void owner_HandleCreated(object sender, EventArgs e)
        {
            this.handle = owner.Handle;
            ShellAPI.RegisterDragDrop(this.handle, this);
        }

        void owner_HandleDestroyed(object sender, EventArgs e)
        {
            ShellAPI.RevokeDragDrop(this.handle);
        }

        /// <summary>
        /// This event will be raised whenever a drop occurs on the ListView.
        /// </summary>
        /// <param name="e">The DropEventArgs for the event</param>
        private void OnDrop(DropEventArgs e)
        {
            if (Drop != null)
                Drop(this, e);
        }

        private void OnRetrieveDestinationRirectory(RetrieveDestinationRirectoryEventArgs e)
        {
            if (RetrieveDestinationRirectory != null)
            {
                RetrieveDestinationRirectory(this, e);
            }
        }
      
        public int DragEnter(IntPtr pDataObj, ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {

            mouseButtons = grfKeyState;
            startEffects = pdwEffect;

            owner.Focus();
            ReleaseCom();

            dropDataObject = pDataObj;

            Point point = owner.PointToClient(new Point(pt.x, pt.y));

            RetrieveDestinationRirectoryEventArgs args = new RetrieveDestinationRirectoryEventArgs(point);
            OnRetrieveDestinationRirectory(args);

            IShellFolder parent = ShellFolder.GetParentShellFolder(args.DestinationDirectory);
            IntPtr[] pidls = ShellFolder.GetPIDLs(args.DestinationDirectory);

            ShellHelper.GetIDropTarget(pidls, parent, out dropTargetPtr, out dropTarget);

            if (dropTarget != null)
            {
                dropTarget.DragEnter(pDataObj, grfKeyState, pt, ref pdwEffect);
            }

            if (dropHelper != null)
            {
                dropHelper.DragEnter(owner.Handle, pDataObj, ref pt, pdwEffect);
            }

            return ShellAPI.S_OK;
        }

        public int DragOver(ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {
            bool reset = false;

            Point point = owner.PointToClient(new Point(pt.x, pt.y));


            if (dropTarget != null)
            {
                dropTarget.DragLeave();
            }

            ReleaseCom();

            RetrieveDestinationRirectoryEventArgs args = new RetrieveDestinationRirectoryEventArgs(point);
            OnRetrieveDestinationRirectory(args);

            IShellFolder parent = ShellFolder.GetParentShellFolder(args.DestinationDirectory);
            IntPtr[] pidls = ShellFolder.GetPIDLs(args.DestinationDirectory);

            ShellHelper.GetIDropTarget(pidls, parent, out dropTargetPtr, out dropTarget);
            reset = true;

            if (dropTarget != null)
            {
                if (reset)
                {
                    dropTarget.DragEnter(dropDataObject, grfKeyState, pt, ref pdwEffect);
                }
                else
                {
                    dropTarget.DragOver(grfKeyState, pt, ref pdwEffect);
                }
            }
            else
            {
                pdwEffect = DragDropEffects.None;
            }

            if (dropHelper != null)
            {
                dropHelper.DragOver(ref pt, pdwEffect);
            }

            return ShellAPI.S_OK;
        }

        public int DragLeave()
        {
            if (dropTarget != null)
            {
                dropTarget.DragLeave();

                ReleaseCom();
                dropDataObject = IntPtr.Zero;
            }

            if (dropHelper != null)
            {
                dropHelper.DragLeave();
            }

            return ShellAPI.S_OK;
        }

        public int DragDrop(IntPtr pDataObj, ShellAPI.MK grfKeyState, ShellAPI.POINT pt, ref DragDropEffects pdwEffect)
        {
            OnDrop(new DropEventArgs(mouseButtons, owner));

            if (!((mouseButtons & ShellAPI.MK.RBUTTON) != 0 ||
                  grfKeyState == ShellAPI.MK.CONTROL ||
                  grfKeyState == ShellAPI.MK.ALT ||
                  grfKeyState == (ShellAPI.MK.CONTROL | ShellAPI.MK.SHIFT)))
            {
                ReleaseCom();
                pdwEffect = DragDropEffects.None;

                if (dropHelper != null)
                {
                    dropHelper.Drop(pDataObj, ref pt, pdwEffect);
                }

                return ShellAPI.S_OK;
            }

            if (dropTarget != null)
            {
                dropTarget.DragDrop(pDataObj, grfKeyState, pt, ref pdwEffect);

                ReleaseCom();
                dropDataObject = IntPtr.Zero;
            }

            if (dropHelper != null)
            {
                dropHelper.Drop(pDataObj, ref pt, pdwEffect);
            }

            return ShellAPI.S_OK;
        }

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
       
    }

}
