namespace Nomad.Commons.Threading
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class CustomThreadQueue : WorkQueue
    {
        private bool InProgress;
        private bool IsPersistent;

        protected CustomThreadQueue(bool persistent)
        {
            this.IsPersistent = persistent;
        }

        protected void DoWork(object state)
        {
            Queue<SimpleTask> queue;
            try
            {
                SimpleTask task;
                bool flag;
                goto Label_00AF;
            Label_0007:
                task = null;
                lock ((queue = base.Queue))
                {
                    if (base.Queue.Count == 0)
                    {
                        if (this.IsPersistent)
                        {
                            Monitor.Pulse(base.Queue);
                            Monitor.Wait(base.Queue);
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (base.Queue.Count > 0)
                    {
                        task = base.Queue.Dequeue();
                    }
                }
                if (task != null)
                {
                    try
                    {
                        task.InternalRun();
                    }
                    catch (Exception exception)
                    {
                        base.OnError(new ExceptionEventArgs(exception));
                    }
                }
            Label_00AF:
                flag = true;
                goto Label_0007;
            }
            finally
            {
                lock ((queue = base.Queue))
                {
                    this.InProgress = false;
                }
            }
        }

        public override void QueueTask(SimpleTask item)
        {
            lock (base.Queue)
            {
                base.Queue.Enqueue(item);
                if (!this.InProgress)
                {
                    this.InProgress = true;
                    this.StartWorkThread();
                }
                else
                {
                    Monitor.Pulse(base.Queue);
                }
            }
        }

        protected abstract void StartWorkThread();
    }
}

