namespace Microsoft.Win32.Network
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    [SuppressUnmanagedCodeSecurity]
    public class Winnetwk
    {
        public const string MprDll = "mpr.dll";

        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetAddConnection2([In] ref NETRESOURCE lpNetResource, [MarshalAs(UnmanagedType.LPTStr)] string lpPassword, [MarshalAs(UnmanagedType.LPTStr)] string lpUsername, CONNECT dwFlags);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetAddConnection3(IntPtr hwndOwner, [In] ref NETRESOURCE lpNetResource, [MarshalAs(UnmanagedType.LPTStr)] string lpPassword, [MarshalAs(UnmanagedType.LPTStr)] string lpUsername, CONNECT dwFlags);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetCancelConnection([MarshalAs(UnmanagedType.LPTStr)] string lpName, [MarshalAs(UnmanagedType.Bool)] bool fForce);
        [DllImport("mpr.dll")]
        public static extern int WNetConnectionDialog(IntPtr hwnd, RESOURCETYPE dwType);
        [DllImport("mpr.dll")]
        public static extern int WNetDisconnectDialog(IntPtr hwnd, RESOURCETYPE dwType);
        [DllImport("mpr.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int WNetEnumResource(SafeNetEnumHandle hEnum, ref uint lpcCount, IntPtr lpBuffer, ref int lpBufferSize);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetGetLastError(out int lpError, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpErrorBuf, int nErrorBufSize, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpNameBuf, int nNameBufSize);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetGetNetworkInformation([MarshalAs(UnmanagedType.LPTStr)] string lpProvider, ref NETINFOSTRUCT lpNetInfoStruct);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetGetResourceInformation([In] ref NETRESOURCE lpNetResource, IntPtr lpBuffer, ref int lpcbBuffer, [MarshalAs(UnmanagedType.LPTStr)] out string lplpSystem);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetGetResourceParent([In] ref NETRESOURCE lpNetResource, IntPtr lpBuffer, ref int lpcbBuffer);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetGetUniversalName([MarshalAs(UnmanagedType.LPTStr)] string lpLocalPath, NAME_INFO dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetOpenEnum(RESOURCE dwScope, RESOURCETYPE dwType, RESOURCEUSAGE dwUsage, [In] ref NETRESOURCE lpNetResource, out SafeNetEnumHandle lphEnum);
        [DllImport("mpr.dll", CharSet=CharSet.Auto)]
        public static extern int WNetOpenEnum(RESOURCE dwScope, RESOURCETYPE dwType, RESOURCEUSAGE dwUsage, IntPtr lpNetResource, out SafeNetEnumHandle lphEnum);
    }
}

