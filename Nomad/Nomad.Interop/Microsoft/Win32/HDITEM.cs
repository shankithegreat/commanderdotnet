namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct HDITEM
    {
        public HDI mask;
        public int cxy;
        public IntPtr pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public HDF fmt;
        public IntPtr lParam;
        public int iImage;
        public int iOrder;
        public uint type;
        public IntPtr pvFilter;
        public static unsafe HDI GetMask(IntPtr ptr)
        {
            return ((void*) ptr).mask;
        }

        public static unsafe HDF GetFormat(IntPtr ptr)
        {
            return ((void*) ptr).fmt;
        }

        public static unsafe void SetMask(IntPtr ptr, HDI mask)
        {
            ((void*) ptr).mask = mask;
        }

        public static unsafe void SetFormat(IntPtr ptr, HDF fmt)
        {
            ((void*) ptr).fmt = fmt;
        }
    }
}

