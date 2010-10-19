namespace Nomad.Commons.Threading
{
    using Nomad.Commons;
    using System;
    using System.Windows.Forms;

    public class IdleQueue : WorkQueue
    {
        private bool IdleEventAssigned;
        private bool ProcessingIdle;

        private void ApplicationIdle(object sender, EventArgs e)
        {
            if (!this.ProcessingIdle)
            {
                this.ProcessingIdle = true;
                if (base.Queue.Count > 0)
                {
                    try
                    {
                        base.Queue.Dequeue().InternalRun();
                    }
                    catch (Exception exception)
                    {
                        base.OnError(new ExceptionEventArgs(exception));
                    }
                }
                else
                {
                    if (this.IdleEventAssigned)
                    {
                        Application.Idle -= new EventHandler(this.ApplicationIdle);
                    }
                    this.IdleEventAssigned = false;
                }
                this.ProcessingIdle = false;
            }
        }

        public override void QueueTask(SimpleTask item)
        {
            base.Queue.Enqueue(item);
            if (!this.IdleEventAssigned)
            {
                Application.Idle += new EventHandler(this.ApplicationIdle);
                this.IdleEventAssigned = true;
            }
        }
    }
}

