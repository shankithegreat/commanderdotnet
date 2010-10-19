namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct _DEV_BROADCAST_HANDLE
    {
        public int dbch_size;
        public DBT_DEVTYP dbch_devicetype;
        public uint dbch_reserved;
        public IntPtr dbch_handle;
        public IntPtr dbch_hdevnotify;
        public Guid dbch_eventguid;
        public int dbch_nameoffset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public byte[] dbch_data;
    }
}

