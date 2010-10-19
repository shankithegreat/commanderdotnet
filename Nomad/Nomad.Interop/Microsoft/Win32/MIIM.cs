namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum MIIM : uint
    {
        All = 0x1ff,
        MIIM_BITMAP = 0x80,
        MIIM_CHECKMARKS = 8,
        MIIM_DATA = 0x20,
        MIIM_FTYPE = 0x100,
        MIIM_ID = 2,
        MIIM_STATE = 1,
        MIIM_STRING = 0x40,
        MIIM_SUBMENU = 4,
        MIIM_TYPE = 0x10
    }
}

