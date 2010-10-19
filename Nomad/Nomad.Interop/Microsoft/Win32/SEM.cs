namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum SEM : uint
    {
        SEM_DEFAULT = 0,
        SEM_FAILCRITICALERRORS = 1,
        SEM_NOALIGNMENTFAULTEXCEPT = 4,
        SEM_NOGPFAULTERRORBOX = 2,
        SEM_NOOPENFILEERRORBOX = 0x8000
    }
}

