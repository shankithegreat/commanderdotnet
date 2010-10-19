namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum CMIC : uint
    {
        ASYNCOK = 0x100000,
        CONTROL_DOWN = 0x40000000,
        FLAG_LOG_USAGE = 0x4000000,
        FLAG_NO_UI = 0x400,
        HOTKEY = 0x20,
        ICON = 0x10,
        NO_CONSOLE = 0x8000,
        NOZONECHECKS = 0x800000,
        PTINVOKE = 0x20000000,
        SHIFT_DOWN = 0x10000000,
        UNICODE = 0x4000
    }
}

