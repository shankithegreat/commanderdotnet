namespace Microsoft.COM
{
    using System;

    [Flags]
    public enum FD : uint
    {
        FD_ACCESSTIME = 0x10,
        FD_ATTRIBUTES = 4,
        FD_CLSID = 1,
        FD_CREATETIME = 8,
        FD_FILESIZE = 0x40,
        FD_LINKUI = 0x8000,
        FD_SIZEPOINT = 2,
        FD_WRITESTIME = 0x20
    }
}

