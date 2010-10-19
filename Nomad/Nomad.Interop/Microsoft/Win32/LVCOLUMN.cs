namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct LVCOLUMN
    {
        public LVCF mask;
        public LVCFMT fmt;
        public int cx;
        public IntPtr pszText;
        public int cchTextMax;
        public int iSubItem;
        public int iImage;
        public int iOrder;
    }
}

