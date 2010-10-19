namespace Microsoft.Win32
{
    using System;

    public enum DESIRED_ACCESS : uint
    {
        FILE_ADD_FILE = 2,
        FILE_ADD_SUBDIRECTORY = 4,
        FILE_APPEND_DATA = 4,
        FILE_CREATE_PIPE_INSTANCE = 4,
        FILE_DELETE_CHILD = 0x40,
        FILE_EXECUTE = 0x20,
        FILE_LIST_DIRECTORY = 1,
        FILE_READ_ATTRIBUTES = 0x80,
        FILE_READ_DATA = 1,
        FILE_READ_EA = 8,
        FILE_TRAVERSE = 0x20,
        FILE_WRITE_ATTRIBUTES = 0x100,
        FILE_WRITE_DATA = 2,
        FILE_WRITE_EA = 0x10,
        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000
    }
}

