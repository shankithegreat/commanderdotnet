namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVCF : uint
    {
        LVCF_FMT = 1,
        LVCF_IMAGE = 0x10,
        LVCF_ORDER = 0x20,
        LVCF_SUBITEM = 8,
        LVCF_TEXT = 4,
        LVCF_WIDTH = 2
    }
}

