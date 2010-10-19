namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public IntPtr idFrom;
        public int code;
        public static unsafe int GetNotifyCode(IntPtr ptr)
        {
            return ((void*) ptr).code;
        }
    }
}

