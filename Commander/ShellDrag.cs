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

    internal class ShellDrag : IDropSource, IDisposable
    {
        // The pointer to the IDataObject being dragged
        private IntPtr dataObjectPtr;

        // The mouseButtons state when a drag starts
        private MouseButtons startButton;

        // A bool to indicate whether this class has been disposed
        private bool disposed = false;

        // The events that will be raised when a drag starts and when it ends
        //public event DragEnterEventHandler DragStart;
        public event EventHandler DragEnd;

        public ShellDrag()
        {
        }

        public void DragCommand(MouseButtons button, params FileSystemInfo[] items)
        {
            ReleaseCom();
            startButton = button;

            IntPtr[] pidls = ShellFolder.GetPIDLs(items);
            IShellFolder parentShellFolder = ShellFolder.GetParentShellFolder(items[0]);

            dataObjectPtr = ShellHelper.GetIDataObject(pidls, parentShellFolder);

            if (dataObjectPtr != IntPtr.Zero)
            {
                DragDropEffects effects;
                //OnDragStart(new DragEnterEventArgs(parentShellFolder, dragStartControl));
                ShellAPI.DoDragDrop(dataObjectPtr, this, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move, out effects);
                OnDragEnd(new EventArgs());
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

        /*/// <summary>
        /// The event that is being raised when a drag starts
        /// </summary>
        /// <param name="e">the DragEnterEventArgs for the event</param>
        private void OnDragStart(DragEnterEventArgs e)
        {
            if (DragStart != null)
                DragStart(this, e);
        }*/

        /// <summary>
        /// The event that is being raised when a drag ends
        /// </summary>
        /// <param name="e">the EventArgs for the event</param>
        private void OnDragEnd(EventArgs e)
        {
            if (DragEnd != null)
                DragEnd(this, e);
        }

    }
}
