namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public static class Lm
    {
        public const int MAX_PREFERRED_LENGTH = -1;
        public const string Netapi32Dll = "Netapi32.dll";
        public const string SvrapiDll = "Svrapi.dll";

        [DllImport("Netapi32.dll")]
        public static extern int NetApiBufferFree(IntPtr lpBuffer);
        [DllImport("Netapi32.dll")]
        public static extern int NetShareCheck([MarshalAs(UnmanagedType.LPWStr)] string ServerName, [MarshalAs(UnmanagedType.LPWStr)] string Device, out int Type);
        [DllImport("Svrapi.dll")]
        public static extern int NetShareEnum([MarshalAs(UnmanagedType.LPStr)] string lpServerName, int dwLevel, IntPtr lpBuffer, ushort cbBuffer, out ushort entriesRead, out ushort totalEntries);
        [DllImport("Netapi32.dll")]
        public static extern int NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, out IntPtr bufptr, int prefmaxlen, out int entriesread, out int totalentries, ref int resume_handle);
    }
}

