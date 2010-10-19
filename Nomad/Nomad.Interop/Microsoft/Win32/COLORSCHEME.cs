namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORSCHEME
    {
        public uint dwSize;
        public int clrBtnHighlight;
        public int clrBtnShadow;
    }
}

