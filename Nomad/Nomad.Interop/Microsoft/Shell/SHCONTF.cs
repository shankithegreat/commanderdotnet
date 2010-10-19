namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SHCONTF : uint
    {
        SHCONTF_FOLDERS = 0x20,
        SHCONTF_INCLUDEHIDDEN = 0x80,
        SHCONTF_INIT_ON_FIRST_NEXT = 0x100,
        SHCONTF_NETPRINTERSRCH = 0x200,
        SHCONTF_NONFOLDERS = 0x40,
        SHCONTF_SHAREABLE = 0x400,
        SHCONTF_STORAGE = 0x800
    }
}

