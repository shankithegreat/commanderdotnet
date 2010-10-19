namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    internal class ThreadStartSimpeTask : SimpleTask
    {
        private ThreadStart FAction;

        public ThreadStartSimpeTask(ThreadStart action)
        {
            this.FAction = action;
        }

        protected override void Run()
        {
            this.FAction();
        }
    }
}

