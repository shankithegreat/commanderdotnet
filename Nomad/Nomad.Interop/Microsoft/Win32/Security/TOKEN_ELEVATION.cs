namespace Microsoft.Win32.Security
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TOKEN_ELEVATION
    {
        [MarshalAs(UnmanagedType.Bool)]
        public bool TokenIsElevated;
    }
}

