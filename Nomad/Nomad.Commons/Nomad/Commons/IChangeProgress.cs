namespace Nomad.Commons
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IChangeProgress
    {
        event EventHandler<ProgressEventArgs> Progress;
    }
}

