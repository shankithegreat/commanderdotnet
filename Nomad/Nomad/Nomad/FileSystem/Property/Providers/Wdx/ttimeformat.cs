namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ttimeformat
    {
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
    }
}

