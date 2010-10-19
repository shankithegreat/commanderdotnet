namespace Nomad.Commons.Threading
{
    using System;
    using System.Reflection;
    using System.Threading;

    public abstract class Task : SimpleTask, IAsyncResult, IDisposable
    {
        private EventWaitHandle FWaitHandle;

        protected Task()
        {
        }

        private void CheckState()
        {
            base.CheckDisposed();
            if (base.FStatus != TaskStatus.Created)
            {
                throw new InvalidOperationException();
            }
        }

        public static Task Create(ThreadStart action)
        {
            return new ThreadStartTask(action);
        }

        public static Task Create<T>(Action<T> action, T state)
        {
            return new ActionTask<T>(action, state);
        }

        public static Task Create(ParameterizedThreadStart action, object state)
        {
            return new ParameterizedThreadStartTask(action, state);
        }

        public static Task Create(WaitCallback action, object state)
        {
            return new WaitCallbackTask(action, state);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.FWaitHandle != null)
            {
                this.FWaitHandle.Close();
            }
            this.FWaitHandle = null;
            base.Dispose(disposing);
        }

        protected override void OnFinished()
        {
            Thread.MemoryBarrier();
            if (this.FWaitHandle != null)
            {
                this.FWaitHandle.Set();
            }
        }

        private void RunCallback(object state)
        {
            if (base.IsCancellationRequested)
            {
                base.FStatus = TaskStatus.Canceled;
            }
            else
            {
                try
                {
                    base.InternalRun();
                }
                catch
                {
                }
            }
        }

        public void RunSynchronously()
        {
            this.CheckState();
            try
            {
                base.InternalRun();
            }
            finally
            {
                base.FInternalState |= SimpleTask.InternalState.CompletedSyncronously;
            }
        }

        public void Start()
        {
            this.CheckState();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.RunCallback));
        }

        public void Start(ApartmentState apartment)
        {
            this.CheckState();
            if (apartment == ApartmentState.STA)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(this.RunCallback));
                thread.SetApartmentState(apartment);
                thread.Start();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.RunCallback));
            }
        }

        public void Wait()
        {
            this.Wait(-1);
        }

        public bool Wait(int millisecondsTimeout)
        {
            base.CheckDisposed();
            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (base.IsCompleted || this.AsyncWaitHandle.WaitOne(millisecondsTimeout, false))
            {
                switch (base.FStatus)
                {
                    case TaskStatus.RanToCompletion:
                        return true;

                    case TaskStatus.Canceled:
                        throw new OperationCanceledException();

                    case TaskStatus.Faulted:
                        throw new TargetInvocationException(base.Exception);
                }
                throw new InvalidOperationException();
            }
            return false;
        }

        public virtual object AsyncState
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
                base.CheckDisposed();
                if (this.FWaitHandle == null)
                {
                    EventWaitHandle handle = new ManualResetEvent(false);
                    if (Interlocked.CompareExchange<EventWaitHandle>(ref this.FWaitHandle, handle, null) != null)
                    {
                        handle.Close();
                    }
                    if (base.IsCompleted)
                    {
                        this.FWaitHandle.Set();
                    }
                }
                return this.FWaitHandle;
            }
        }
    }
}

