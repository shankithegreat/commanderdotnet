namespace Microsoft.IO
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class Win32IOException : IOException
    {
        public Win32IOException() : this(new Win32Exception())
        {
        }

        public Win32IOException(Win32Exception error) : base(error.Message, error)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(error.NativeErrorCode);
            this.NativeErrorCode = error.NativeErrorCode;
        }

        public Win32IOException(int error) : this(new Win32Exception(error))
        {
        }

        private Win32IOException(Win32Exception error, bool dummy) : base(error.Message)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(error.NativeErrorCode);
            this.NativeErrorCode = error.NativeErrorCode;
        }

        public int NativeErrorCode { get; private set; }
    }
}

