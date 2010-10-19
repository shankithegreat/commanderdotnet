namespace Nomad.Commons.Threading
{
    using System;

    public enum TaskStatus
    {
        Created,
        Running,
        RanToCompletion,
        Canceled,
        Faulted
    }
}

