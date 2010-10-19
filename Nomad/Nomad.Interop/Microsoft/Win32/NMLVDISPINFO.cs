namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVDISPINFO
    {
        public NMHDR hdr;
        public LVITEM item;
        public static unsafe LVIF GetMask(IntPtr ptr)
        {
            return ((void*) ptr).item.mask;
        }

        public static unsafe int GetItemIndex(IntPtr ptr)
        {
            return ((void*) ptr).item.iItem;
        }

        public static unsafe LVIS GetState(IntPtr ptr)
        {
            return ((void*) ptr).item.state;
        }

        public static unsafe LVIS GetStateMask(IntPtr ptr)
        {
            return ((void*) ptr).item.stateMask;
        }

        public static unsafe void SetState(IntPtr ptr, LVIS value)
        {
            ((void*) ptr).item.state = value;
        }
    }
}

