namespace Microsoft.Shell
{
    using System;

    public enum WTS_FLAGS : uint
    {
        WTS_EXTRACT = 0,
        WTS_EXTRACTDONOTCACHE = 0x20,
        WTS_FASTEXTRACT = 2,
        WTS_FORCEEXTRACTION = 4,
        WTS_INCACHEONLY = 1,
        WTS_SLOWRECLAIM = 8
    }
}

