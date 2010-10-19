namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DIBSECTION
    {
        public BITMAP dsBm;
        public BITMAPINFOHEADER dsBmih;
        public int dsBitField1;
        public int dsBitField2;
        public int dsBitField3;
        public IntPtr dshSection;
        public int dsOffset;
    }
}

