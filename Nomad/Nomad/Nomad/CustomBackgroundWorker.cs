namespace Nomad
{
    using Nomad.Commons;
    using System;
    using System.Threading;

    public class CustomBackgroundWorker : IDisposable, IAsyncResult
    {
        private Thread FBackgroundThread;
        private EventWaitHandle FExitThreadEvent = new ManualResetEvent(false);
        private EventWaitHandle FResumeThreadEvent = new ManualResetEvent(true);
        private WaitHandle[] ResumeThreadEventArray;

        public CustomBackgroundWorker()
        {
            this.ResumeThreadEventArray = new WaitHandle[] { this.FResumeThreadEvent, this.FExitThreadEvent };
        }

        public void AbortAsync()
        {
            if ((this.FBackgroundThread != null) && this.FBackgroundThread.IsAlive)
            {
                this.FBackgroundThread.Abort();
            }
        }

        public void CancelAsync()
        {
            this.FExitThreadEvent.Set();
        }

        protected bool CheckCancellationPending()
        {
            this.CheckSuspendingPending();
            return this.CancellationPending;
        }

        protected void CheckSuspendingPending()
        {
            WaitHandle.WaitAny(this.ResumeThreadEventArray);
        }

        public virtual void Dispose()
        {
            this.StopAsync();
        }

        public void ResumeAsync()
        {
            if ((this.FBackgroundThread != null) && this.FBackgroundThread.IsAlive)
            {
                this.FResumeThreadEvent.Set();
            }
        }

        protected void RunAsync(ThreadStart start, ThreadPriority priority, string ThreadName)
        {
            this.FExitThreadEvent.Reset();
            this.FBackgroundThread = new Thread(start);
            this.FBackgroundThread.IsBackground = true;
            this.FBackgroundThread.Priority = priority;
            this.FBackgroundThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            this.FBackgroundThread.Name = ThreadName;
            ErrorReport.RegisterThread(this.FBackgroundThread);
            this.FBackgroundThread.Start();
        }

        protected void StopAsync()
        {
            if ((this.FBackgroundThread != null) && this.FBackgroundThread.IsAlive)
            {
                this.FExitThreadEvent.Set();
                if (!(!this.FBackgroundThread.IsAlive || this.FBackgroundThread.Join(250)))
                {
                    this.FBackgroundThread.Abort();
                }
                if (this.FBackgroundThread.IsAlive)
                {
                    this.FBackgroundThread.Join(250);
                }
            }
            this.FBackgroundThread = null;
        }

        public void SuspendAsync()
        {
            if ((this.FBackgroundThread != null) && this.FBackgroundThread.IsAlive)
            {
                this.FResumeThreadEvent.Reset();
            }
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
                return this.FExitThreadEvent;
            }
        }

        protected Thread BackgroundThread
        {
            get
            {
                return this.FBackgroundThread;
            }
        }

        public bool CancellationPending
        {
            get
            {
                return this.FExitThreadEvent.WaitOne(0, false);
            }
            set
            {
                if (value)
                {
                    this.CancelAsync();
                }
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        protected EventWaitHandle ExitThreadEvent
        {
            get
            {
                return this.FExitThreadEvent;
            }
        }

        public virtual bool IsCompleted
        {
            get
            {
                return (((this.BackgroundThread != null) && this.CancellationPending) && !this.BackgroundThread.IsAlive);
            }
        }

        public bool SuspendingPending
        {
            get
            {
                return !this.FResumeThreadEvent.WaitOne(0, false);
            }
        }
    }
}

