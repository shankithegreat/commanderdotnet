namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVIF : uint
    {
        LVIF_COLFMT = 0x10000,
        LVIF_COLUMNS = 0x200,
        LVIF_DI_SETITEM = 0x1000,
        LVIF_GROUPID = 0x100,
        LVIF_IMAGE = 2,
        LVIF_INDENT = 0x10,
        LVIF_NORECOMPUTE = 0x800,
        LVIF_PARAM = 4,
        LVIF_STATE = 8,
        LVIF_TEXT = 1
    }
}

