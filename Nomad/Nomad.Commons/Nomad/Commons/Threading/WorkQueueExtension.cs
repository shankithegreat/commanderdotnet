namespace Nomad.Commons.Threading
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class WorkQueueExtension
    {
        public static void QueueWeakWorkItem(this WorkQueue queue, WaitCallback callBack)
        {
            queue.QueueTask(new WeakWaitCallbackTask(callBack, null));
        }

        public static void QueueWeakWorkItem<T>(this WorkQueue queue, Action<T> callBack, T state)
        {
            queue.QueueTask(new WeakActionTask<T>(callBack, state));
        }

        public static void QueueWeakWorkItem(this WorkQueue queue, WaitCallback callBack, object state)
        {
            queue.QueueTask(new WeakWaitCallbackTask(callBack, state));
        }

        private class WeakActionTask<T> : SimpleTask
        {
            private T State;
            private WeakReference WeakCallback;

            public WeakActionTask(Action<T> callBack, T state)
            {
                this.WeakCallback = new WeakReference(callBack);
                this.State = state;
            }

            protected override void Run()
            {
                if (this.WeakCallback.IsAlive)
                {
                    Action<T> target = (Action<T>) this.WeakCallback.Target;
                    if (target != null)
                    {
                        target(this.State);
                        return;
                    }
                }
                base.CancellationConfirmed();
            }
        }

        private class WeakWaitCallbackTask : SimpleTask
        {
            private object State;
            private WeakReference WeakCallback;

            public WeakWaitCallbackTask(WaitCallback callBack, object state)
            {
                this.WeakCallback = new WeakReference(callBack);
                this.State = state;
            }

            protected override void Run()
            {
                if (this.WeakCallback.IsAlive)
                {
                    WaitCallback target = (WaitCallback) this.WeakCallback.Target;
                    if (target != null)
                    {
                        target(this.State);
                        return;
                    }
                }
                base.CancellationConfirmed();
            }
        }
    }
}

