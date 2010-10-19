namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct EXTRASEARCH
    {
        public Guid guidSearch;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=80)]
        public string wszFriendlyName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x800)]
        public string wszUrl;
    }
}

