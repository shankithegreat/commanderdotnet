namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum TVIF : uint
    {
        TVIF_CHILDREN = 0x40,
        TVIF_DI_SETITEM = 0x1000,
        TVIF_EXPANDEDIMAGE = 0x200,
        TVIF_HANDLE = 0x10,
        TVIF_IMAGE = 2,
        TVIF_INTEGRAL = 0x80,
        TVIF_PARAM = 4,
        TVIF_SELECTEDIMAGE = 0x20,
        TVIF_STATE = 8,
        TVIF_STATEEX = 0x100,
        TVIF_TEXT = 1
    }
}

