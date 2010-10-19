namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct SHQUERYRBINFO
    {
        public int cbSize;
        public ulong i64Size;
        public ulong i64NumItems;
    }
}

