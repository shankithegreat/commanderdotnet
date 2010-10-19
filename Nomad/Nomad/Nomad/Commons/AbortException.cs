namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;

    public class AbortException : WarningException
    {
        public AbortException()
        {
            base.HResult = -2147467260;
        }
    }
}

