namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum MOD : uint
    {
        MOD_ALT = 1,
        MOD_CONTROL = 2,
        MOD_SHIFT = 4,
        MOD_WIN = 8
    }
}

