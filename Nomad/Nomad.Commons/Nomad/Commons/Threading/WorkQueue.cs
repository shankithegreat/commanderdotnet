namespace Nomad.Commons.Threading
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class WorkQueue
    {
        protected Queue<SimpleTask> Queue = new Queue<SimpleTask>();

        public event EventHandler<ExceptionEventArgs> Error;

        protected WorkQueue()
        {
        }

        protected void OnError(ExceptionEventArgs e)
        {
            if (this.Error != null)
            {
                this.Error(this, e);
            }
        }

        public abstract void QueueTask(SimpleTask item);
        public void QueueUserWorkItem(WaitCallback callBack)
        {
            this.QueueTask(new WaitCallbackSimpleTask(callBack, null));
        }

        public void QueueUserWorkItem<T>(Action<T> callBack, T state)
        {
            this.QueueTask(new ActionSimpleTask<T>(callBack, state));
        }

        public void QueueUserWorkItem(WaitCallback callBack, object state)
        {
            this.QueueTask(new WaitCallbackSimpleTask(callBack, state));
        }
    }
}

