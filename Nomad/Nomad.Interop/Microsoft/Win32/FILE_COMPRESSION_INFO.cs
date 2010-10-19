namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_COMPRESSION_INFO
    {
        public long CompressedFileSize;
        public ushort CompressionFormat;
        public byte CompressionUnitShift;
        public byte ChunkShift;
        public byte ClusterShift;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
        public byte[] Reserved;
    }
}

