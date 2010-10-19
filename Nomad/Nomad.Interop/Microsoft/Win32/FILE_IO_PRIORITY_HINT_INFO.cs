namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_IO_PRIORITY_HINT_INFO
    {
        public PRIORITY_HINT PriorityHint;
    }
}

