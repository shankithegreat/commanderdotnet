namespace Microsoft.Win32.Network
{
    using System;

    public enum STYPE : uint
    {
        STYPE_DEVICE = 2,
        STYPE_DISKTREE = 0,
        STYPE_IPC = 3,
        STYPE_PRINTQ = 1,
        STYPE_SPECIAL = 0x80000000,
        STYPE_TEMPORARY = 0x40000000
    }
}

