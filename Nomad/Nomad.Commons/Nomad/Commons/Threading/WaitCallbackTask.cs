namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    public class WaitCallbackTask : Task
    {
        private WaitCallback FAction;
        private object FState;

        internal WaitCallbackTask(WaitCallback action, object state)
        {
            this.FAction = action;
            this.FState = state;
        }

        protected override void Run()
        {
            this.FAction(this.FState);
        }

        public override object AsyncState
        {
            get
            {
                return this.FState;
            }
        }
    }
}

