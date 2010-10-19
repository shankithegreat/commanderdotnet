namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HeaderDataEx
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x400)]
        public string ArcName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x400)]
        public string FileName;
        public int Flags;
        public uint PackSize;
        public uint PackSizeHigh;
        public uint UnpSize;
        public uint UnpSizeHigh;
        public int HostOS;
        public int FileCRC;
        public int FileTime;
        public int UnpVer;
        public int Method;
        public FileAttributes FileAttr;
        public IntPtr CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, SizeConst=0x400)]
        public byte[] Reserved;
    }
}

