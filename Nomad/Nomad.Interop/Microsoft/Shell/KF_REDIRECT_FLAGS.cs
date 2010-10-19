namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum KF_REDIRECT_FLAGS
    {
        KF_REDIRECT_CHECK_ONLY = 0x10,
        KF_REDIRECT_COPY_CONTENTS = 0x200,
        KF_REDIRECT_COPY_SOURCE_DACL = 2,
        KF_REDIRECT_DEL_SOURCE_CONTENTS = 0x400,
        KF_REDIRECT_EXCLUDE_ALL_KNOWN_SUBFOLDERS = 0x800,
        KF_REDIRECT_OWNER_USER = 4,
        KF_REDIRECT_PIN = 0x80,
        KF_REDIRECT_SET_OWNER_EXPLICIT = 8,
        KF_REDIRECT_UNPIN = 0x40,
        KF_REDIRECT_USER_EXCLUSIVE = 1,
        KF_REDIRECT_WITH_UI = 0x20
    }
}

