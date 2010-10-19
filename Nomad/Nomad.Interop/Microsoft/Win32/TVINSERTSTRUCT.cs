namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TVINSERTSTRUCT
    {
        public IntPtr hParent;
        public IntPtr hInsertAfter;
        public TVITEM item;
    }
}

