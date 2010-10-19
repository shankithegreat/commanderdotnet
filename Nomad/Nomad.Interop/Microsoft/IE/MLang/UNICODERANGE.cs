namespace Microsoft.IE.MLang
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct UNICODERANGE
    {
        public ushort wcFrom;
        public ushort wcTo;
    }
}

