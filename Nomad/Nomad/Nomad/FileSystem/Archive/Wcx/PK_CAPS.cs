namespace Nomad.FileSystem.Archive.Wcx
{
    using System;

    [Flags]
    public enum PK_CAPS
    {
        PK_CAPS_BY_CONTENT = 0x40,
        PK_CAPS_DELETE = 8,
        PK_CAPS_ENCRYPT = 0x200,
        PK_CAPS_HIDE = 0x100,
        PK_CAPS_MEMPACK = 0x20,
        PK_CAPS_MODIFY = 2,
        PK_CAPS_MULTIPLE = 4,
        PK_CAPS_NEW = 1,
        PK_CAPS_OPTIONS = 0x10,
        PK_CAPS_SEARCHTEXT = 0x80
    }
}

