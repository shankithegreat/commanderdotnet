namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum HOTKEYF : byte
    {
        HOTKEYF_ALT = 4,
        HOTKEYF_CONTROL = 2,
        HOTKEYF_EXT = 8,
        HOTKEYF_SHIFT = 1
    }
}

