namespace Nomad.Commons.IO
{
    using System;

    public enum STGTY : byte
    {
        STGTY_INVALID = 0,
        STGTY_LOCKBYTES = 3,
        STGTY_PROPERTY = 4,
        STGTY_ROOT = 5,
        STGTY_STORAGE = 1,
        STGTY_STREAM = 2
    }
}

