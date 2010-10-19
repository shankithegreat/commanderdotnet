namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DESCRIPTOR_HEADER
    {
        public uint Version;
        public uint Size;
    }
}

