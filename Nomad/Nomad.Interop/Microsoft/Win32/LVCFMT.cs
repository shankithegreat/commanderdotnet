namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVCFMT
    {
        LVCFMT_BITMAP_ON_RIGHT = 0x1000,
        LVCFMT_CENTER = 2,
        LVCFMT_COL_HAS_IMAGES = 0x8000,
        LVCFMT_IMAGE = 0x800,
        LVCFMT_JUSTIFYMASK = 3,
        LVCFMT_LEFT = 0,
        LVCFMT_RIGHT = 1
    }
}

