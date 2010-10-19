namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.IO;

    public class WcxException : IOException
    {
        public readonly int ErrorCode;

        public WcxException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public WcxException(int errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }
    }
}

