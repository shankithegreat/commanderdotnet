namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SLR : uint
    {
        SLR_ANY_MATCH = 2,
        SLR_INVOKE_MSI = 0x80,
        SLR_NO_UI = 1,
        SLR_NOLINKINFO = 0x40,
        SLR_NOSEARCH = 0x10,
        SLR_NOTRACK = 0x20,
        SLR_NOUPDATE = 8,
        SLR_UPDATE = 4
    }
}

