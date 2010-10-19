namespace Nomad.Commons.Threading
{
    using System;

    public class ActionTask<T> : Task
    {
        private Action<T> FAction;
        private T FState;

        internal ActionTask(Action<T> action, T state)
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

