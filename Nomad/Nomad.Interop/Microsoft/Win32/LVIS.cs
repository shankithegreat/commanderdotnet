namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVIS
    {
        LVIS_ACTIVATING = 0x20,
        LVIS_CUT = 4,
        LVIS_DROPHILITED = 8,
        LVIS_FOCUSED = 1,
        LVIS_GLOW = 0x10,
        LVIS_OVERLAYMASK = 0xf00,
        LVIS_SELECTED = 2,
        LVIS_STATEIMAGEMASK = 0xf000
    }
}

