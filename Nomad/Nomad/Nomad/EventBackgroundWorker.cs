namespace Nomad
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class EventBackgroundWorker : CustomBackgroundWorker
    {
        private Stopwatch FStopwatch;
        protected Exception ThreadError;

        public event AsyncCompletedEventHandler Completed;

        public event ProgressChangedEventHandler ProgressChanged;

        protected EventBackgroundWorker()
        {
        }

        private void DoBackgroundWork()
        {
            if (this.FStopwatch == null)
            {
                this.FStopwatch = new Stopwatch();
            }
            else
            {
                this.FStopwatch.Reset();
            }
            this.FStopwatch.Start();
            this.ThreadError = null;
            try
            {
                this.DoWork();
            }
            catch (Exception exception)
            {
                this.ThreadError = exception;
            }
            finally
            {
                this.FStopwatch.Stop();
            }
            bool cancellationPending = base.CancellationPending;
            base.CancelAsync();
            if (this.Completed != null)
            {
                this.Completed(this, new AsyncCompletedEventArgs(this.ThreadError, cancellationPending, null));
            }
        }

        protected abstract void DoWork();
        protected void RaiseProgressChanged(int progressPercent, object userState)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, new ProgressChangedEventArgs(progressPercent, userState));
            }
        }

        public void RunAsync(ThreadPriority priority)
        {
            base.RunAsync(new ThreadStart(this.DoBackgroundWork), priority, this.Name);
        }

        public TimeSpan Duration
        {
            get
            {
                return ((this.FStopwatch != null) ? this.FStopwatch.Elapsed : TimeSpan.Zero);
            }
        }

        public override bool IsCompleted
        {
            get
            {
                return (((this.FStopwatch != null) && base.CancellationPending) && !this.FStopwatch.IsRunning);
            }
        }

        public abstract string Name { get; }
    }
}

