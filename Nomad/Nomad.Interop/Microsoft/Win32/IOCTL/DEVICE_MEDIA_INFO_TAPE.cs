namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVICE_MEDIA_INFO_TAPE
    {
        public STORAGE_MEDIA_TYPE MediaType;
        public MEDIA_CHARACTERISTICS MediaCharacteristics;
        public uint CurrentBlockSize;
        public STORAGE_BUS_TYPE BusType;
        public byte MediumType;
        public byte DensityCode;
    }
}

