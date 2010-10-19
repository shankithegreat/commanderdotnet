namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVGA : uint
    {
        LVGA_FOOTER_CENTER = 0x10,
        LVGA_FOOTER_LEFT = 8,
        LVGA_FOOTER_RIGHT = 0x20,
        LVGA_HEADER_CENTER = 2,
        LVGA_HEADER_LEFT = 1,
        LVGA_HEADER_RIGHT = 4
    }
}

