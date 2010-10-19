namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DEVICE_MEDIA_INFO_DISK
    {
        public long Cylinders;
        public STORAGE_MEDIA_TYPE MediaType;
        public uint TracksPerCylinder;
        public uint SectorsPerTrack;
        public uint BytesPerSector;
        public uint NumberMediaSides;
        public MEDIA_CHARACTERISTICS MediaCharacteristics;
    }
}

