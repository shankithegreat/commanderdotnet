namespace Microsoft.Win32.Network
{
    using System;

    public enum RESOURCETYPE : uint
    {
        RESOURCETYPE_ANY = 0,
        RESOURCETYPE_DISK = 1,
        RESOURCETYPE_PRINT = 2,
        RESOURCETYPE_RESERVED = 8,
        RESOURCETYPE_UNKNOWN = 0xffffffff
    }
}

