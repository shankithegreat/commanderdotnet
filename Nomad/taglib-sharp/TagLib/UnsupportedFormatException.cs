namespace TagLib
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UnsupportedFormatException : Exception
    {
        public UnsupportedFormatException()
        {
        }

        public UnsupportedFormatException(string message) : base(message)
        {
        }

        protected UnsupportedFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnsupportedFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

