namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PREVENT_MEDIA_REMOVAL
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool PreventMediaRemoval;
    }
}

