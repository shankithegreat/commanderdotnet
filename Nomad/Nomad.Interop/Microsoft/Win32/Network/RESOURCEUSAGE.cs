namespace Microsoft.Win32.Network
{
    using System;

    [Flags]
    public enum RESOURCEUSAGE : uint
    {
        RESOURCEUSAGE_ALL = 0x13,
        RESOURCEUSAGE_ATTACHED = 0x10,
        RESOURCEUSAGE_CONNECTABLE = 1,
        RESOURCEUSAGE_CONTAINER = 2,
        RESOURCEUSAGE_NOLOCALDEVICE = 4,
        RESOURCEUSAGE_RESERVED = 0x80000000,
        RESOURCEUSAGE_SIBLING = 8
    }
}

