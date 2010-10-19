namespace Microsoft.COM.IFilter
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public struct PROPSPEC
    {
        [FieldOffset(4)]
        public IntPtr lpwstr;
        [FieldOffset(4)]
        public int propid;
        [FieldOffset(0)]
        public int ulKind;
    }
}

