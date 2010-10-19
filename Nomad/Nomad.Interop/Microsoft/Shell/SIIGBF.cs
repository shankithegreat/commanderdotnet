namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SIIGBF
    {
        SIIGBF_BIGGERSIZEOK = 1,
        SIIGBF_ICONONLY = 4,
        SIIGBF_INCACHEONLY = 0x10,
        SIIGBF_MEMORYONLY = 2,
        SIIGBF_RESIZETOFIT = 0,
        SIIGBF_THUMBNAILONLY = 8
    }
}

