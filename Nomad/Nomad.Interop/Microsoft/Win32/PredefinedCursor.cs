namespace Microsoft.Win32
{
    using System;

    public enum PredefinedCursor : uint
    {
        IDC_APPSTARTING = 0x7f8a,
        IDC_ARROW = 0x7f00,
        IDC_CROSS = 0x7f03,
        IDC_HAND = 0x7f89,
        IDC_HELP = 0x7f8b,
        IDC_IBEAM = 0x7f01,
        [Obsolete("use IDC_ARROW")]
        IDC_ICON = 0x7f81,
        IDC_NO = 0x7f88,
        [Obsolete("use IDC_SIZEALL")]
        IDC_SIZE = 0x7f80,
        IDC_SIZEALL = 0x7f86,
        IDC_SIZENESW = 0x7f83,
        IDC_SIZENS = 0x7f85,
        IDC_SIZENWSE = 0x7f82,
        IDC_SIZEWE = 0x7f84,
        IDC_UPARROW = 0x7f04,
        IDC_WAIT = 0x7f02
    }
}

