namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct PropertyKey
    {
        public const int PID_TITLE = 2;
        public Guid fmtid;
        public uint pid;
        public static readonly Guid PropertySet;
        public PropertyKey(Guid fmt, uint id)
        {
            this.fmtid = fmt;
            this.pid = id;
        }

        static PropertyKey()
        {
            PropertySet = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
        }
    }
}

