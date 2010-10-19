namespace Microsoft.Win32.IOCTL
{
    using System;

    public enum FILE_ACCESS : byte
    {
        FILE_ANY_ACCESS = 0,
        FILE_READ_ACCESS = 1,
        FILE_READ_DATA = 1,
        FILE_SPECIAL_ACCESS = 0,
        FILE_WRITE_ACCESS = 2,
        FILE_WRITE_DATA = 2
    }
}

