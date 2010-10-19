namespace Microsoft.Win32.IOCTL
{
    using System;

    [Flags]
    public enum MEDIA_CHARACTERISTICS : uint
    {
        MEDIA_CURRENTLY_MOUNTED = 0x80000000,
        MEDIA_ERASEABLE = 1,
        MEDIA_READ_ONLY = 4,
        MEDIA_READ_WRITE = 8,
        MEDIA_WRITE_ONCE = 2,
        MEDIA_WRITE_PROTECTED = 0x100
    }
}

