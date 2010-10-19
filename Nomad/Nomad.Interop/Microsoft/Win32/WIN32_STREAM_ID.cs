namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WIN32_STREAM_ID
    {
        public StreamId dwStreamId;
        public StreamAttributes dwStreamAttributes;
        public long Size;
        public int dwStreamNameSize;
    }
}

