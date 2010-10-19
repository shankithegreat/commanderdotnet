namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum GIL_OUT : uint
    {
        GIL_DONTCACHE = 0x10,
        GIL_NOTFILENAME = 8,
        GIL_PERCLASS = 4,
        GIL_PERINSTANCE = 2,
        GIL_SIMULATEDOC = 1
    }
}

