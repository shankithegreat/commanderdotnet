namespace Microsoft.Win32.Security
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }
}

