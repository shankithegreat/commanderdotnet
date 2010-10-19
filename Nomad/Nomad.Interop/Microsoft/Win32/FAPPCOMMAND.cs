namespace Microsoft.Win32
{
    using System;

    public enum FAPPCOMMAND : ushort
    {
        FAPPCOMMAND_KEY = 0,
        FAPPCOMMAND_MASK = 0xf000,
        FAPPCOMMAND_MOUSE = 0x8000,
        FAPPCOMMAND_OEM = 0x1000
    }
}

