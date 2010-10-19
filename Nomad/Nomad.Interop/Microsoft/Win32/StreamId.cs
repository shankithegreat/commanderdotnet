namespace Microsoft.Win32
{
    using System;

    public enum StreamId : uint
    {
        BACKUP_ALTERNATE_DATA = 4,
        BACKUP_DATA = 1,
        BACKUP_EA_DATA = 2,
        BACKUP_INVALID = 0,
        BACKUP_LINK = 5,
        BACKUP_OBJECT_ID = 7,
        BACKUP_PROPERTY_DATA = 6,
        BACKUP_REPARSE_DATA = 8,
        BACKUP_SECURITY_DATA = 3,
        BACKUP_SPARSE_BLOCK = 9,
        BACKUP_TXFS_DATA = 10
    }
}

