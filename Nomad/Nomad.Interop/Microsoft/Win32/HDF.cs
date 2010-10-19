namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum HDF
    {
        HDF_BITMAP = 0x2000,
        HDF_BITMAP_ON_RIGHT = 0x1000,
        HDF_CENTER = 2,
        HDF_IMAGE = 0x800,
        HDF_JUSTIFYMASK = 3,
        HDF_LEFT = 0,
        HDF_OWNERDRAW = 0x8000,
        HDF_RIGHT = 1,
        HDF_RTLREADING = 4,
        HDF_SORTDOWN = 0x200,
        HDF_SORTUP = 0x400,
        HDF_STRING = 0x4000
    }
}

