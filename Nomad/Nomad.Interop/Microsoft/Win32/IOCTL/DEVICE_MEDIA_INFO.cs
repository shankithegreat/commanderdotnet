namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public struct DEVICE_MEDIA_INFO
    {
        [FieldOffset(0)]
        public DEVICE_MEDIA_INFO_DISK DiskInfo;
        [FieldOffset(0)]
        public DEVICE_MEDIA_INFO_REMOVABLEDISK RemovableDiskInfo;
        [FieldOffset(0)]
        public DEVICE_MEDIA_INFO_TAPE TapeInfo;
    }
}

