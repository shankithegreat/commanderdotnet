namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct tdateformat
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDay;
    }
}

