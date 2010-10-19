namespace Microsoft.COM
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHDRAGIMAGE
    {
        public SIZE sizeDragImage;
        public Point ptOffset;
        public IntPtr hbmpDragImage;
        public uint crColorKey;
    }
}

