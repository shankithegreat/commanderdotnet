namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=4)]
    public struct MIMECPINFO
    {
        public uint dwFlags;
        public uint uiCodePage;
        public uint uiFamilyCodePage;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
        public string wszDescription;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=50)]
        public string wszWebCharset;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=50)]
        public string wszHeaderCharset;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=50)]
        public string wszBodyCharset;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszFixedWidthFont;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszProportionalFont;
        public byte bGDICharset;
    }
}

