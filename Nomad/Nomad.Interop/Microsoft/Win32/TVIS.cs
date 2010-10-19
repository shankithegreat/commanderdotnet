namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum TVIS : uint
    {
        TVIS_BOLD = 0x10,
        TVIS_CUT = 4,
        TVIS_DROPHILITED = 8,
        TVIS_EXPANDED = 0x20,
        TVIS_EXPANDEDONCE = 0x40,
        TVIS_EXPANDPARTIAL = 0x80,
        TVIS_OVERLAYMASK = 0xf00,
        TVIS_SELECTED = 2,
        TVIS_STATEIMAGEMASK = 0xf000,
        TVIS_USERMASK = 0xf000
    }
}

