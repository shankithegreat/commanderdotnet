namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class CyclicStreamChainException : IOException
    {
        public CyclicStreamChainException()
        {
        }

        public CyclicStreamChainException(string message) : base(message)
        {
        }
    }
}

