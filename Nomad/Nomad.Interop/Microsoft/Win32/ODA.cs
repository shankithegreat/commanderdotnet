namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum ODA : uint
    {
        ODA_DRAWENTIRE = 1,
        ODA_FOCUS = 4,
        ODA_SELECT = 2
    }
}

