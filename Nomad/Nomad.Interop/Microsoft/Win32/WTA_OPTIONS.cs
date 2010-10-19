namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct WTA_OPTIONS
    {
        public WTNCA Flags;
        public uint Mask;
    }
}

