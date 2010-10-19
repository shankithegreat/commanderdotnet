namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=4)]
    public struct MIMECSETINFO
    {
        public uint uiCodePage;
        public uint uiInternetEncoding;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=50)]
        public string wszCharset;
    }
}

