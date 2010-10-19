namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct _DEV_BROADCAST_VOLUME
    {
        public int dbcv_size;
        public DBT_DEVTYP dbcv_devicetype;
        public uint dbcv_reserved;
        public uint dbcv_unitmask;
        public DBTF dbcv_flags;
    }
}

