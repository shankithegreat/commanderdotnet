namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=8)]
    public struct SCRIPFONTINFO
    {
        public long scripts;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszFont;
    }
}

