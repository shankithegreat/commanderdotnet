namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SHARDAPPIDINFOLINK
    {
        public IShellLinkW psl;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszAppID;
    }
}

