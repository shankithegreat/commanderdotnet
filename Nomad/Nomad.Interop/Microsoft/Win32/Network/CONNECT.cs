namespace Microsoft.Win32.Network
{
    using System;

    [Flags]
    public enum CONNECT : uint
    {
        CONNECT_CURRENT_MEDIA = 0x200,
        CONNECT_DEFERRED = 0x400,
        CONNECT_INTERACTIVE = 8,
        CONNECT_LOCALDRIVE = 0x100,
        CONNECT_NEED_DRIVE = 0x20,
        CONNECT_PROMPT = 0x10,
        CONNECT_REDIRECT = 0x80,
        CONNECT_REFCOUNT = 0x40,
        CONNECT_RESERVED = 0xff000000,
        CONNECT_TEMPORARY = 4,
        CONNECT_UPDATE_PROFILE = 1,
        CONNECT_UPDATE_RECENT = 2
    }
}

