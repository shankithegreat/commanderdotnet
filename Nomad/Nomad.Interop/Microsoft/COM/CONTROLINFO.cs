namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CONTROLINFO
    {
        public uint cb;
        public IntPtr hAccel;
        public ushort cAccel;
        public uint dwFlags;
    }
}

