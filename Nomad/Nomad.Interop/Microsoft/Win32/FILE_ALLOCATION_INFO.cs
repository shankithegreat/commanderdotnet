namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_ALLOCATION_INFO
    {
        public long AllocationSize;
    }
}

