namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct RM_PROCESS_INFO
    {
        public RM_UNIQUE_PROCESS Process;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string strAppName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
        public string strServiceShortName;
        public RM_APP_TYPE ApplicationType;
        public RM_APP_STATUS AppStatus;
        public uint TSSessionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bRestartable;
    }
}

