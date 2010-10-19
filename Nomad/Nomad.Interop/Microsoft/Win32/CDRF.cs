namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum CDRF
    {
        CDRF_DODEFAULT = 0,
        CDRF_DOERASE = 8,
        CDRF_NEWFONT = 2,
        CDRF_NOTIFYITEMDRAW = 0x20,
        CDRF_NOTIFYPOSTERASE = 0x40,
        CDRF_NOTIFYPOSTPAINT = 0x10,
        CDRF_NOTIFYSUBITEMDRAW = 0x20,
        CDRF_SKIPDEFAULT = 4,
        CDRF_SKIPPOSTPAINT = 0x100
    }
}

