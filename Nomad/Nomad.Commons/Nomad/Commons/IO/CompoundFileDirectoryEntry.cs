namespace Nomad.Commons.IO
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct CompoundFileDirectoryEntry
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x20)]
        public string _ab;
        public ushort _cb;
        public STGTY _mse;
        public DECOLOR _bflags;
        public uint _sidLeftSib;
        public uint _sidRightSib;
        public uint _sidChild;
        public Guid _clsId;
        public uint _dwUserFlags;
        public System.Runtime.InteropServices.ComTypes.FILETIME _Createtime;
        public System.Runtime.InteropServices.ComTypes.FILETIME _Modifytime;
        public uint _sectStart;
        public uint _ulSizeLow;
        public uint _ulSizeHigh;
    }
}

