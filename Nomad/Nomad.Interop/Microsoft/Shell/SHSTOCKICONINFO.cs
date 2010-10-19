namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=4)]
    public struct SHSTOCKICONINFO
    {
        public int cbSize;
        public IntPtr hIcon;
        public int iSysImageIndex;
        public int iIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string szPath;
    }
}

