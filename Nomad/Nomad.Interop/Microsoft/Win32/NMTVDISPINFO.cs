namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMTVDISPINFO
    {
        public NMHDR hdr;
        public TVITEM item;
    }
}

