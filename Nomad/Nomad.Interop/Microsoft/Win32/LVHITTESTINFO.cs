namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LVHITTESTINFO
    {
        public Point pt;
        public LVHT flags;
        public int iItem;
        public int iSubItem;
        public int iGroup;
    }
}

