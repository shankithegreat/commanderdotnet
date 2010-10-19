namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHELLDETAILS
    {
        public int fmt;
        public int cxChar;
        public STRRET str;
    }
}

