namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DVASPECTINFO
    {
        public uint cb;
        private DVASPECTINFOFLAG dwFlags;
    }
}

