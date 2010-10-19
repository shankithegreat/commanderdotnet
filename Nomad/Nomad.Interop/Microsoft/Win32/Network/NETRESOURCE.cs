namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct NETRESOURCE
    {
        public RESOURCE dwScope;
        public RESOURCETYPE dwType;
        public RESOURCEDISPLAYTYPE dwDisplayType;
        public RESOURCEUSAGE dwUsage;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpLocalName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpRemoteName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpComment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpProvider;
    }
}

