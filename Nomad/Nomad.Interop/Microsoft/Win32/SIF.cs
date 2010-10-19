namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum SIF
    {
        SIF_ALL = 0x17,
        SIF_DISABLENOSCROLL = 8,
        SIF_PAGE = 2,
        SIF_POS = 4,
        SIF_RANGE = 1,
        SIF_TRACKPOS = 0x10
    }
}

