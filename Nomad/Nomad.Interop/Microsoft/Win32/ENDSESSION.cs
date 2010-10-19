namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum ENDSESSION : uint
    {
        ENDSESSION_CLOSEAPP = 1,
        ENDSESSION_CRITICAL = 0x40000000,
        ENDSESSION_LOGOFF = 0x80000000
    }
}

