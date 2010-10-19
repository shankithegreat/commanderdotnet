namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public sealed class LOGPALETTE
    {
        public ushort palVersion;
        public ushort palNumEntries;
    }
}

