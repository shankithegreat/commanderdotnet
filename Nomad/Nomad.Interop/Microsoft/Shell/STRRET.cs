namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Size=0x108)]
    public struct STRRET
    {
        [FieldOffset(4)]
        public IntPtr pOleStr;
        [FieldOffset(4)]
        public uint uOffset;
        [FieldOffset(0)]
        public STRRET_TYPE uType;
    }
}

