namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct UNIVERSAL_NAME_INFO
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpUniversalName;
    }
}

