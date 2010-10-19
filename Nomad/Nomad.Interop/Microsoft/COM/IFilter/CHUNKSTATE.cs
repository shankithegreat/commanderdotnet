namespace Microsoft.COM.IFilter
{
    using System;

    [Flags]
    public enum CHUNKSTATE : uint
    {
        CHUNK_FILTER_OWNED_VALUE = 4,
        CHUNK_TEXT = 1,
        CHUNK_VALUE = 2
    }
}

