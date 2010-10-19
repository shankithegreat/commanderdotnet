namespace TagLib
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CorruptFileException : Exception
    {
        public CorruptFileException()
        {
        }

        public CorruptFileException(string message) : base(message)
        {
        }

        protected CorruptFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CorruptFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

