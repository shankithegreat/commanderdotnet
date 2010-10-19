namespace Nomad.FileSystem.Archive.Common
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ArjHeader
    {
        public ushort Mark;
        public ushort HeadSize;
        public byte FirstHeadSize;
        public byte ArjVer;
        public byte ArjExtrVer;
        public byte HostOS;
        public byte Flags;
        public byte Method;
        public byte FileType;
        public byte Reserved;
        public uint ftime;
        public uint PackSize;
        public uint UnpSize;
        public uint CRC;
        public ushort FileSpec;
        public ushort AccessMode;
        public ushort HostData;
    }
}

