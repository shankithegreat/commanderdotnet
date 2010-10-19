namespace Microsoft.IE
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DOCHOSTUIINFO
    {
        public uint cbSize;
        public uint dwFlags;
        public uint dwDoubleClick;
        [MarshalAs(UnmanagedType.BStr)]
        public string pchHostCss;
        [MarshalAs(UnmanagedType.BStr)]
        public string pchHostNS;
    }
}

