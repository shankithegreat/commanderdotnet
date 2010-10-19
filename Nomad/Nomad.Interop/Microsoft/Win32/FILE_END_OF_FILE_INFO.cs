namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_END_OF_FILE_INFO
    {
        public long EndOfFile;
    }
}

