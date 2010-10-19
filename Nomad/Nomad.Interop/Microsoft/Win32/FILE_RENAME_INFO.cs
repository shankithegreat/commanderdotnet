namespace Microsoft.Win32
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_RENAME_INFO
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool ReplaceIfExists;
        public SafeFileHandle RootDirectory;
        public uint FileNameLength;
    }
}

