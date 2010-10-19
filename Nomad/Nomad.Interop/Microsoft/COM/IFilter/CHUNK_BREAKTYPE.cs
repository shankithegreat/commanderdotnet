namespace Microsoft.COM.IFilter
{
    using System;

    public enum CHUNK_BREAKTYPE : uint
    {
        CHUNK_EOC = 4,
        CHUNK_EOP = 3,
        CHUNK_EOS = 2,
        CHUNK_EOW = 1,
        CHUNK_NO_BREAK = 0
    }
}

