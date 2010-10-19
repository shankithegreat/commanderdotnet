namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHARDAPPIDINFOIDLIST
    {
        public IntPtr pidl;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszAppID;
    }
}

