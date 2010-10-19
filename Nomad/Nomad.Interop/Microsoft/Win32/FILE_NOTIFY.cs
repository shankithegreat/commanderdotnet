namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum FILE_NOTIFY : uint
    {
        FILE_NOTIFY_CHANGE_ATTRIBUTES = 4,
        FILE_NOTIFY_CHANGE_CREATION = 0x40,
        FILE_NOTIFY_CHANGE_DIR_NAME = 2,
        FILE_NOTIFY_CHANGE_FILE_NAME = 1,
        FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x20,
        FILE_NOTIFY_CHANGE_LAST_WRITE = 0x10,
        FILE_NOTIFY_CHANGE_SECURITY = 0x100,
        FILE_NOTIFY_CHANGE_SIZE = 8
    }
}

