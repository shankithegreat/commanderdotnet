namespace Microsoft.COM.IFilter
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct STAT_CHUNK
    {
        public int idChunk;
        public CHUNK_BREAKTYPE breakType;
        public CHUNKSTATE flags;
        public int locale;
        public FULLPROPSPEC attribute;
        public int idChunkSource;
        public int cwcStartSource;
        public int cwcLenSource;
    }
}

