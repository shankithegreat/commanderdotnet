namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct GET_MEDIA_TYPES
    {
        public DEVICE_TYPE DeviceType;
        public uint MediaInfoCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public DEVICE_MEDIA_INFO[] MediaInfo;
    }
}

