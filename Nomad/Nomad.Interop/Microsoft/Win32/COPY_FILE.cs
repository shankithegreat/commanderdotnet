namespace Microsoft.Win32
{
    using System;

    public enum COPY_FILE : uint
    {
        COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 8,
        COPY_FILE_COPY_SYMLINK = 0x800,
        COPY_FILE_FAIL_IF_EXISTS = 1,
        COPY_FILE_OPEN_SOURCE_FOR_WRITE = 4,
        COPY_FILE_RESTARTABLE = 2
    }
}

