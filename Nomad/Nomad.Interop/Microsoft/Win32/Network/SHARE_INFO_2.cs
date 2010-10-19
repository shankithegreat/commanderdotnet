namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHARE_INFO_2
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetName;
        public STYPE ShareType;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        public int Permissions;
        public int MaxUsers;
        public int CurrentUsers;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
    }
}

