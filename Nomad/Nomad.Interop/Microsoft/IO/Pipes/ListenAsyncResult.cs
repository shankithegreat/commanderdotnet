namespace Microsoft.IO.Pipes
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ListenAsyncResult : IAsyncResult, IDisposable
    {
        private EventWaitHandle FAsyncWaitHandle = new ManualResetEvent(false);
        private IntPtr FOverlappedPtr;
        public readonly NativeOverlapped Overlapped = new NativeOverlapped();

        internal ListenAsyncResult()
        {
            this.Overlapped.EventHandle = this.FAsyncWaitHandle.SafeWaitHandle.DangerousGetHandle();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool dispose)
        {
            if (this.FOverlappedPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.FOverlappedPtr);
                this.FOverlappedPtr = IntPtr.Zero;
            }
        }

        ~ListenAsyncResult()
        {
            this.Dispose(false);
        }

        public object AsyncState
        {
            get
            {
                return null;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return this.FAsyncWaitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.FAsyncWaitHandle.WaitOne(0, false);
            }
        }

        public IntPtr OverlappedPtr
        {
            get
            {
                if (this.FOverlappedPtr == IntPtr.Zero)
                {
                    this.FOverlappedPtr = Marshal.AllocHGlobal(Marshal.SizeOf(this.Overlapped));
                    Marshal.StructureToPtr(this.Overlapped, this.FOverlappedPtr, false);
                }
                return this.FOverlappedPtr;
            }
        }
    }
}

