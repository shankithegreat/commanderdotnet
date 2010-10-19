namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum HDI : uint
    {
        HDI_BITMAP = 0x10,
        HDI_DI_SETITEM = 0x40,
        HDI_FILTER = 0x100,
        HDI_FORMAT = 4,
        HDI_HEIGHT = 1,
        HDI_IMAGE = 0x20,
        HDI_LPARAM = 8,
        HDI_ORDER = 0x80,
        HDI_TEXT = 2,
        HDI_WIDTH = 1
    }
}

