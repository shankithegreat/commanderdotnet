namespace Microsoft.Win32
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_ATTRIBUTE_TAG_INFO
    {
        public System.IO.FileAttributes FileAttributes;
        public uint ReparseTag;
    }
}

