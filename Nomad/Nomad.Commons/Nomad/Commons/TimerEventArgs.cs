namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class TimerEventArgs : CancelEventArgs
    {
        public TimerEventArgs(int id)
        {
            this.Id = id;
        }

        public int Id { get; private set; }
    }
}

