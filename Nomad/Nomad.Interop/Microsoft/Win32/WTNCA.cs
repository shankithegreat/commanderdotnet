namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum WTNCA : uint
    {
        WTNCA_NODRAWCAPTION = 1,
        WTNCA_NODRAWICON = 2,
        WTNCA_NOMIRRORHELP = 8,
        WTNCA_NOSYSMENU = 4
    }
}

