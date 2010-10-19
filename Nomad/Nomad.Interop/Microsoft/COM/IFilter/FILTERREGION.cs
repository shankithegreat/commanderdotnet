namespace Microsoft.COM.IFilter
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FILTERREGION
    {
        public int idChunk;
        public int cwcStart;
        public int cwcExtent;
    }
}

