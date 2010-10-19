namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=4)]
    public struct RFC1766INFO
    {
        public uint lcid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=6)]
        public string wszRfc1766;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszLocaleName;
    }
}

