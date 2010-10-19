namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum DLGC
    {
        DLGC_BUTTON = 0x2000,
        DLGC_DEFPUSHBUTTON = 0x10,
        DLGC_HASSETSEL = 8,
        DLGC_RADIOBUTTON = 0x40,
        DLGC_STATIC = 0x100,
        DLGC_UNDEFPUSHBUTTON = 0x20,
        DLGC_WANTALLKEYS = 4,
        DLGC_WANTARROWS = 1,
        DLGC_WANTCHARS = 0x80,
        DLGC_WANTMESSAGE = 4,
        DLGC_WANTTAB = 2
    }
}

