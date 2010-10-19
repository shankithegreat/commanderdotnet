namespace System.Windows.Forms
{
    using Microsoft.Win32;
    using System;

    public class LockWindowRedraw : IDisposable
    {
        private RDW InvalidateFlags;
        private IntPtr WindowHandle;

        public LockWindowRedraw(IWin32Window window, bool invalidate)
        {
            Control control = window as Control;
            if ((control == null) || control.IsHandleCreated)
            {
                this.WindowHandle = window.Handle;
                Windows.SendMessage(this.WindowHandle, 11, IntPtr.Zero, IntPtr.Zero);
            }
            if (invalidate)
            {
                this.InvalidateFlags = RDW.RDW_ALLCHILDREN | RDW.RDW_NOERASE | RDW.RDW_INVALIDATE;
            }
        }

        public void Dispose()
        {
            if (this.WindowHandle != IntPtr.Zero)
            {
                Windows.SendMessage(this.WindowHandle, 11, (IntPtr) 1, IntPtr.Zero);
                if (this.InvalidateFlags != ((RDW) 0))
                {
                    Windows.RedrawWindow(this.WindowHandle, IntPtr.Zero, IntPtr.Zero, this.InvalidateFlags);
                }
            }
        }
    }
}

