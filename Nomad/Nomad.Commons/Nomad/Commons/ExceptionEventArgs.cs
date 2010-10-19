namespace Nomad.Commons
{
    using System;
    using System.Runtime.CompilerServices;

    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception e)
        {
            this.ErrorException = e;
        }

        public Exception ErrorException { get; private set; }
    }
}

