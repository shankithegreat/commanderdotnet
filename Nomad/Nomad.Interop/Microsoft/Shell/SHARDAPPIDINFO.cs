namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHARDAPPIDINFO
    {
        public IShellItem psi;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszAppID;
    }
}

