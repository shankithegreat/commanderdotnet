namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HDHITTESTINFO
    {
        public Point pt;
        public HHT flags;
        public int iItem;
    }
}

