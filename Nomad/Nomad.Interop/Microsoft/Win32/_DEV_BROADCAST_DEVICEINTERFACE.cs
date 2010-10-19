namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=1)]
    public struct _DEV_BROADCAST_DEVICEINTERFACE
    {
        public int dbcc_size;
        public DBT_DEVTYP dbcc_devicetype;
        public uint dbcc_reserved;
        public Guid dbcc_classguid;
        public ushort test;
    }
}

