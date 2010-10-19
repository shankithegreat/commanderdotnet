namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        public int dbch_size;
        public DBT_DEVTYP dbch_devicetype;
        public uint dbch_reserved;
        public static unsafe DBT_DEVTYP GetDeviceType(IntPtr ptr)
        {
            return ((void*) ptr).dbcv_devicetype;
        }
    }
}

