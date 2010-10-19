namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TVITEMEX
    {
        public TVIF mask;
        public IntPtr hItem;
        public TVIS state;
        public TVIS stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public int iSelectedImage;
        public int cChildren;
        public IntPtr lParam;
        public int iIntegral;
        public TVIS_EX uStateEx;
        public IntPtr hwnd;
        public int iExpandedImage;
    }
}

