namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    internal class WaitCallbackSimpleTask : SimpleTask
    {
        private WaitCallback FAction;
        private object FState;

        public WaitCallbackSimpleTask(WaitCallback action, object state)
        {
            this.FAction = action;
            this.FState = state;
        }

        protected override void Run()
        {
            this.FAction(this.FState);
        }
    }
}

