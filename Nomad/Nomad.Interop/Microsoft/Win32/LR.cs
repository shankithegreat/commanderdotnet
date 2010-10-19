namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LR : uint
    {
        LR_COPYDELETEORG = 8,
        LR_COPYFROMRESOURCE = 0x4000,
        LR_COPYRETURNORG = 4,
        LR_CREATEDIBSECTION = 0x2000,
        LR_DEFAULTCOLOR = 0,
        LR_DEFAULTSIZE = 0x40,
        LR_LOADFROMFILE = 0x10,
        LR_LOADMAP3DCOLORS = 0x1000,
        LR_LOADTRANSPARENT = 0x20,
        LR_MONOCHROME = 1,
        LR_SHARED = 0x8000,
        LR_VGACOLOR = 0x80
    }
}

