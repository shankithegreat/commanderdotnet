namespace Nomad.FileSystem.Archive.Wcx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct OpenArchiveData
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string ArcName;
        public PK_OM OpenMode;
        public int OpenResult;
        public IntPtr CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
    }
}

