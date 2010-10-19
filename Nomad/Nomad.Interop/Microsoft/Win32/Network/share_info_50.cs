namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct share_info_50
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
        public string NetName;
        private byte bShareType;
        public ushort Flags;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Path;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
        public string PasswordRW;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
        public string PasswordRO;
        public STYPE ShareType
        {
            get
            {
                return (STYPE) this.bShareType;
            }
        }
    }
}

