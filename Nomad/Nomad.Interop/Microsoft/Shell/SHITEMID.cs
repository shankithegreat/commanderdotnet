namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct SHITEMID
    {
        public ushort cb;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public byte[] abID;
    }
}

