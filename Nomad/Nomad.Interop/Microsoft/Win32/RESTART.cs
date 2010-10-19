namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum RESTART : uint
    {
        RESTART_NO_CRASH = 1,
        RESTART_NO_HANG = 2,
        RESTART_NO_PATCH = 4,
        RESTART_NO_REBOOT = 8
    }
}

