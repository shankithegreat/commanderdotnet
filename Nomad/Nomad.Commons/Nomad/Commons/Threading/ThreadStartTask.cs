namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    public class ThreadStartTask : Task
    {
        private ThreadStart FAction;

        internal ThreadStartTask(ThreadStart action)
        {
            this.FAction = action;
        }

        protected override void Run()
        {
            this.FAction();
        }
    }
}

