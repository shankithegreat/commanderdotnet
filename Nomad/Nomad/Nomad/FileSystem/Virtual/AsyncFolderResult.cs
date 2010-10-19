namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Threading;

    internal class AsyncFolderResult : IAsyncResult
    {
        private bool FCompletedSynchronously;
        public readonly CustomAsyncFolder Folder;
        private WaitHandle FWaitHandle;

        public AsyncFolderResult(CustomAsyncFolder folder)
        {
            this.Folder = folder;
            this.FWaitHandle = new ManualResetEvent(true);
            this.FCompletedSynchronously = true;
        }

        public AsyncFolderResult(CustomAsyncFolder folder, WaitHandle waitHandle)
        {
            this.Folder = folder;
            this.FWaitHandle = waitHandle;
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
                return this.FWaitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return this.FCompletedSynchronously;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return (this.FCompletedSynchronously || this.FWaitHandle.WaitOne(0, false));
            }
        }
    }
}

