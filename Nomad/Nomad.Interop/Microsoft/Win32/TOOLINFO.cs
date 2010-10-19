namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct TOOLINFO
    {
        public int cbSize;
        public TTF uFlags;
        public IntPtr hwnd;
        public IntPtr uId;
        public Rectangle rect;
        public IntPtr hinst;
        public IntPtr lpszText;
    }
}

