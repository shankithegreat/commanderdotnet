namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum HHT : uint
    {
        HHT_ABOVE = 0x100,
        HHT_BELOW = 0x200,
        HHT_NOWHERE = 1,
        HHT_ONDIVIDER = 4,
        HHT_ONDIVOPEN = 8,
        HHT_ONFILTER = 0x10,
        HHT_ONFILTERBUTTON = 0x20,
        HHT_ONHEADER = 2,
        HHT_TOLEFT = 0x800,
        HHT_TORIGHT = 0x400
    }
}

