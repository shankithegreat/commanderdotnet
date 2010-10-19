namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct share_info_1
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
        public string NetName;
        private byte Padding;
        private ushort bShareType;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;
        public STYPE ShareType
        {
            get
            {
                return (STYPE) this.bShareType;
            }
        }
    }
}

