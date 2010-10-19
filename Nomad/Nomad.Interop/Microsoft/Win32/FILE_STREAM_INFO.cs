namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_STREAM_INFO
    {
        public uint NextEntryOffset;
        public uint StreamNameLength;
        public long StreamSize;
        public long StreamAllocationSize;
    }
}

