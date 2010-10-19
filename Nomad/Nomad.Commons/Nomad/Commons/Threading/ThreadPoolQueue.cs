namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    public class ThreadPoolQueue : CustomThreadQueue
    {
        public ThreadPoolQueue() : base(false)
        {
        }

        protected override void StartWorkThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoWork));
        }
    }
}

