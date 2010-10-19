namespace Microsoft.Win32
{
    using System;

    public enum KLF : uint
    {
        KLF_ACTIVATE = 1,
        KLF_NOTELLSHELL = 0x80,
        KLF_REORDER = 8,
        KLF_REPLACELANG = 0x10,
        KLF_RESET = 0x40000000,
        KLF_SETFORPROCESS = 0x100,
        KLF_SHIFTLOCK = 0x10000,
        KLF_SUBSTITUTE_OK = 2
    }
}

