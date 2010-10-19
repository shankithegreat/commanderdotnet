namespace Microsoft
{
    using System;

    [Flags]
    public enum ApplicationRestartOptions : uint
    {
        NoCrash = 1,
        NoHang = 2,
        NoPatch = 4,
        NoReboot = 8
    }
}

