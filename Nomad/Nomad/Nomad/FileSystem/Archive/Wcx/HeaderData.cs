namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HeaderData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string ArcName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
        public string FileName;
        public int Flags;
        public uint PackSize;
        public uint UnpSize;
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
    }
}

