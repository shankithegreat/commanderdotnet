namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum TTF : uint
    {
        TTF_ABSOLUTE = 0x80,
        TTF_CENTERTIP = 2,
        TTF_DI_SETITEM = 0x8000,
        TTF_IDISHWND = 1,
        TTF_PARSELINKS = 0x1000,
        TTF_RTLREADING = 4,
        TTF_SUBCLASS = 0x10,
        TTF_TRACK = 0x20,
        TTF_TRANSPARENT = 0x100
    }
}

