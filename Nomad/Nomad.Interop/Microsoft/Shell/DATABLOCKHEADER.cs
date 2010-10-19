namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DATABLOCKHEADER
    {
        public int cbSize;
        public uint dwSignature;
    }
}

