namespace Microsoft.Win32.IOCTL
{
    using System;

    public enum METHOD : byte
    {
        METHOD_BUFFERED = 0,
        METHOD_IN_DIRECT = 1,
        METHOD_NEITHER = 3,
        METHOD_OUT_DIRECT = 2
    }
}

