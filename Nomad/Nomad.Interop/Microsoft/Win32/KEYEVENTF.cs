namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum KEYEVENTF : uint
    {
        KEYEVENTF_EXTENDEDKEY = 1,
        KEYEVENTF_KEYUP = 2,
        KEYEVENTF_SCANCODE = 8,
        KEYEVENTF_UNICODE = 4
    }
}

