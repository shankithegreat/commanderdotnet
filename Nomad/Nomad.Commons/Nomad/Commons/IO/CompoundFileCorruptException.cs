namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public class CompoundFileCorruptException : IOException
    {
        private const int STG_E_DOCFILECORRUPT = -2147286775;

        public CompoundFileCorruptException()
        {
            base.HResult = -2147286775;
        }

        public CompoundFileCorruptException(string message) : base(message)
        {
            base.HResult = -2147286775;
        }
    }
}

