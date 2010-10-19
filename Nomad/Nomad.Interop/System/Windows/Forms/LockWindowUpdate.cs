namespace System.Windows.Forms
{
    using Microsoft.Win32;
    using System;

    public class LockWindowUpdate : IDisposable
    {
        private bool DisposeNeeded;

        public LockWindowUpdate(IWin32Window window)
        {
            this.DisposeNeeded = Lock(window);
        }

        public void Dispose()
        {
            if (this.DisposeNeeded)
            {
                Unlock();
                this.DisposeNeeded = false;
            }
        }

        public static bool Lock(IWin32Window window)
        {
            Control control = window as Control;
            return (((control == null) || control.IsHandleCreated) && Windows.LockWindowUpdate(window.Handle));
        }

        public static void Unlock()
        {
            Windows.LockWindowUpdate(IntPtr.Zero);
        }
    }
}

