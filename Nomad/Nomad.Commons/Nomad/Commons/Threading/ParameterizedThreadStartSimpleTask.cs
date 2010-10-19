namespace Nomad.Commons.Threading
{
    using System;
    using System.Threading;

    internal class ParameterizedThreadStartSimpleTask : SimpleTask
    {
        private ParameterizedThreadStart FAction;
        private object FState;

        public ParameterizedThreadStartSimpleTask(ParameterizedThreadStart action, object state)
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

