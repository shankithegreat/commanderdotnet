namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVNI
    {
        LVNI_ABOVE = 0x100,
        LVNI_ALL = 0,
        LVNI_BELOW = 0x200,
        LVNI_CUT = 4,
        LVNI_DIRECTIONMASK = 0xf00,
        LVNI_DROPHILITED = 8,
        LVNI_FOCUSED = 1,
        LVNI_PREVIOUS = 0x20,
        LVNI_SAMEGROUPONLY = 0x80,
        LVNI_SELECTED = 2,
        LVNI_STATEMASK = 15,
        LVNI_TOLEFT = 0x400,
        LVNI_TORIGHT = 0x800,
        LVNI_VISIBLEONLY = 0x40,
        LVNI_VISIBLEORDER = 0x10
    }
}

