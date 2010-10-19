namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum CMF : uint
    {
        CMF_CANRENAME = 0x10,
        CMF_DEFAULTONLY = 1,
        CMF_EXPLORE = 4,
        CMF_EXTENDEDVERBS = 0x100,
        CMF_INCLUDESTATIC = 0x40,
        CMF_NODEFAULT = 0x20,
        CMF_NORMAL = 0,
        CMF_NOVERBS = 8,
        CMF_RESERVED = 0xffff0000,
        CMF_VERBSONLY = 2
    }
}

