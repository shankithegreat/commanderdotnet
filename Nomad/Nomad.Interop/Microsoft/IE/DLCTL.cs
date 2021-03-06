﻿namespace Microsoft.IE
{
    using System;

    [Flags]
    public enum DLCTL : uint
    {
        DLCTL_BGSOUNDS = 0x40,
        DLCTL_DLIMAGES = 0x10,
        DLCTL_DOWNLOADONLY = 0x800,
        DLCTL_FORCEOFFLINE = 0x10000000,
        DLCTL_NO_BEHAVIORS = 0x8000,
        DLCTL_NO_CLIENTPULL = 0x20000000,
        DLCTL_NO_DLACTIVEXCTLS = 0x400,
        DLCTL_NO_FRAMEDOWNLOAD = 0x1000,
        DLCTL_NO_JAVA = 0x100,
        DLCTL_NO_METACHARSET = 0x10000,
        DLCTL_NO_RUNACTIVEXCTLS = 0x200,
        DLCTL_NO_SCRIPTS = 0x80,
        DLCTL_NOFRAMES = 0x80000,
        DLCTL_OFFLINE = 0x80000000,
        DLCTL_OFFLINEIFNOTCONNECTED = 0x80000000,
        DLCTL_PRAGMA_NO_CACHE = 0x4000,
        DLCTL_RESYNCHRONIZE = 0x2000,
        DLCTL_SILENT = 0x40000000,
        DLCTL_URL_ENCODING_DISABLE_UTF8 = 0x20000,
        DLCTL_URL_ENCODING_ENABLE_UTF8 = 0x40000,
        DLCTL_VIDEOS = 0x20
    }
}

