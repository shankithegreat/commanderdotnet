namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct SHCOLUMNID
    {
        public const uint PID_STG_STORAGETYPE = 4;
        public const uint PID_STG_NAME = 10;
        public const uint PID_STG_SIZE = 12;
        public const uint PID_STG_ATTRIBUTES = 13;
        public const uint PID_STG_WRITETIME = 14;
        public const uint PID_STG_CREATETIME = 15;
        public const uint PID_STG_ACCESSTIME = 0x10;
        public const uint PID_STG_ALLOCSIZE = 0x12;
        public const uint PID_VOLUME_FREE = 2;
        public const uint PID_VOLUME_CAPACITY = 3;
        public const uint PID_VOLUME_FILESYSTEM = 4;
        public Guid fmtid;
        public uint pid;
        public static readonly Guid PropertySetStorage;
        public static readonly Guid PropertySetShellDetails;
        public static readonly Guid PropertySetDisplaced;
        public static readonly Guid PropertySetMiscellaneous;
        public static readonly Guid PropertySetVolume;
        public static readonly Guid PropertySetQuery;
        public static readonly Guid PropertySetSummaryInformation;
        public SHCOLUMNID(Guid fmt, uint id)
        {
            this.fmtid = fmt;
            this.pid = id;
        }

        static SHCOLUMNID()
        {
            PropertySetStorage = new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC");
            PropertySetShellDetails = new Guid("28636AA6-953D-11D2-B5D6-00C04FD918D0");
            PropertySetDisplaced = new Guid("9B174B33-40FF-11D2-A27E-00C04FC30871");
            PropertySetMiscellaneous = new Guid("9B174B34-40FF-11D2-A27E-00C04FC30871");
            PropertySetVolume = new Guid("9B174B35-40FF-11D2-A27E-00C04FC30871");
            PropertySetQuery = new Guid("49691C90-7E17-101A-A91C-08002B2ECDA9");
            PropertySetSummaryInformation = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
        }
    }
}

