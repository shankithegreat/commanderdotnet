namespace Nomad.Commons.Threading
{
    using System;

    public abstract class SimpleTask : IDisposable
    {
        [ThreadStatic]
        private static SimpleTask FCurrent;
        private System.Exception FException;
        protected InternalState FInternalState;
        protected TaskStatus FStatus;

        protected SimpleTask()
        {
        }

        public void Cancel()
        {
            this.CheckDisposed();
            this.FInternalState |= InternalState.CancellationRequested;
        }

        protected void CancellationConfirmed()
        {
            this.FInternalState |= InternalState.CancellationConfirmed;
        }

        protected void CheckDisposed()
        {
            if ((this.FInternalState & InternalState.Disposed) > 0)
            {
                throw new ObjectDisposedException("Task");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        internal void InternalRun()
        {
            FCurrent = this;
            try
            {
                this.OnStarted();
                try
                {
                    this.FStatus = TaskStatus.Running;
                    this.Run();
                    this.FStatus = ((this.FInternalState & InternalState.CancellationConfirmed) > 0) ? TaskStatus.Canceled : TaskStatus.RanToCompletion;
                }
                catch (OperationCanceledException)
                {
                    this.FStatus = TaskStatus.Canceled;
                }
                catch (System.Exception exception)
                {
                    this.FException = exception;
                    this.FStatus = TaskStatus.Faulted;
                    if (!this.OnError(exception))
                    {
                        throw;
                    }
                }
                finally
                {
                    this.OnFinished();
                }
            }
            finally
            {
                FCurrent = null;
            }
        }

        protected virtual bool OnError(System.Exception e)
        {
            return false;
        }

        protected virtual void OnFinished()
        {
        }

        protected virtual void OnStarted()
        {
        }

        protected abstract void Run();

        public bool CompletedSynchronously
        {
            get
            {
                return ((this.FInternalState & InternalState.CompletedSyncronously) > 0);
            }
        }

        public static SimpleTask Current
        {
            get
            {
                return FCurrent;
            }
        }

        public System.Exception Exception
        {
            get
            {
                return this.FException;
            }
        }

        public bool IsCancellationRequested
        {
            get
            {
                return ((this.FInternalState & InternalState.CancellationRequested) > 0);
            }
        }

        public bool IsCancelled
        {
            get
            {
                return (this.FStatus == TaskStatus.Canceled);
            }
        }

        public bool IsCompleted
        {
            get
            {
                switch (this.FStatus)
                {
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        return true;
                }
                return false;
            }
        }

        public bool IsFaulted
        {
            get
            {
                return (this.FStatus == TaskStatus.Faulted);
            }
        }

        public TaskStatus Status
        {
            get
            {
                return this.FStatus;
            }
        }

        [Flags]
        protected enum InternalState
        {
            CancellationConfirmed = 2,
            CancellationRequested = 1,
            CompletedSyncronously = 4,
            Disposed = 8
        }
    }
}

