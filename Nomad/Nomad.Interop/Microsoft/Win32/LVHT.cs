namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVHT : uint
    {
        LVHT_ABOVE = 8,
        LVHT_BELOW = 0x10,
        LVHT_EX_FOOTER = 0x8000000,
        LVHT_EX_GROUP = 0xf3000000,
        LVHT_EX_GROUP_BACKGROUND = 0x80000000,
        LVHT_EX_GROUP_COLLAPSE = 0x40000000,
        LVHT_EX_GROUP_FOOTER = 0x20000000,
        LVHT_EX_GROUP_HEADER = 0x10000000,
        LVHT_EX_GROUP_STATEICON = 0x1000000,
        LVHT_EX_GROUP_SUBSETLINK = 0x2000000,
        LVHT_EX_ONCONTENTS = 0x4000000,
        LVHT_NOWHERE = 1,
        LVHT_ONITEM = 14,
        LVHT_ONITEMICON = 2,
        LVHT_ONITEMLABEL = 4,
        LVHT_ONITEMSTATEICON = 8,
        LVHT_TOLEFT = 0x40,
        LVHT_TORIGHT = 0x20
    }
}

