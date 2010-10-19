namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum CDIS
    {
        CDIS_CHECKED = 8,
        CDIS_DEFAULT = 0x20,
        CDIS_DISABLED = 4,
        CDIS_DROPHILITED = 0x1000,
        CDIS_FOCUS = 0x10,
        CDIS_GRAYED = 2,
        CDIS_HOT = 0x40,
        CDIS_INDETERMINATE = 0x100,
        CDIS_MARKED = 0x80,
        CDIS_NEARHOT = 0x400,
        CDIS_OTHERSIDEHOT = 0x800,
        CDIS_SELECTED = 1,
        CDIS_SHOWKEYBOARDCUES = 0x200
    }
}

