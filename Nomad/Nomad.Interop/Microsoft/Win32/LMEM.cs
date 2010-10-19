namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LMEM : uint
    {
        LHND = 0x42,
        LMEM_FIXED = 0,
        LMEM_MOVEABLE = 2,
        LMEM_ZEROINIT = 0x40,
        LPTR = 0x40,
        NONZEROLHND = 2,
        NONZEROLPTR = 0
    }
}

