namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum GMEM : uint
    {
        GHND = 0x42,
        GMEM_FIXED = 0,
        GMEM_MOVEABLE = 2,
        GMEM_ZEROINIT = 0x40,
        GPTR = 0x40
    }
}

