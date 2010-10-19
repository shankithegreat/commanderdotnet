namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=4)]
    public struct SCRIPTINFO
    {
        public byte ScriptId;
        public uint uiCodePage;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x30)]
        public string wszDescription;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszFixedWidthFont;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string wszProportionalFont;
    }
}

