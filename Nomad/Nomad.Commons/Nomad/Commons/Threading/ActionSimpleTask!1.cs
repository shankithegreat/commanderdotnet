namespace Nomad.Commons.Threading
{
    using System;

    internal class ActionSimpleTask<T> : SimpleTask
    {
        private Action<T> FAction;
        private T FState;

        public ActionSimpleTask(Action<T> action, T state)
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

