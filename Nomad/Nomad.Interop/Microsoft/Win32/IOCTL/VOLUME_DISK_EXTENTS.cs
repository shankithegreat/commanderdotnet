namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct VOLUME_DISK_EXTENTS
    {
        public uint NumberOfDiskExtents;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public DISK_EXTENT[] Extents;
    }
}

