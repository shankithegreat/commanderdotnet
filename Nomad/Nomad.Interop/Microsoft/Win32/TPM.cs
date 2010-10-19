namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum TPM : uint
    {
        TPM_BOTTOMALIGN = 0x20,
        TPM_CENTERALIGN = 4,
        TPM_HORIZONTAL = 0,
        TPM_HORNEGANIMATION = 0x800,
        TPM_HORPOSANIMATION = 0x400,
        TPM_LAYOUTRTL = 0x8000,
        TPM_LEFTALIGN = 0,
        TPM_LEFTBUTTON = 0,
        TPM_NOANIMATION = 0x4000,
        TPM_NONOTIFY = 0x80,
        TPM_RECURSE = 1,
        TPM_RETURNCMD = 0x100,
        TPM_RIGHTALIGN = 8,
        TPM_RIGHTBUTTON = 2,
        TPM_TOPALIGN = 0,
        TPM_VCENTERALIGN = 0x10,
        TPM_VERNEGANIMATION = 0x2000,
        TPM_VERPOSANIMATION = 0x1000,
        TPM_VERTICAL = 0x40
    }
}

