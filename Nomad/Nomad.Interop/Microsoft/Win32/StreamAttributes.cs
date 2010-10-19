namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum StreamAttributes : uint
    {
        STREAM_CONTAINS_PROPERTIES = 4,
        STREAM_CONTAINS_SECURITY = 2,
        STREAM_MODIFIED_WHEN_READ = 1,
        STREAM_NORMAL_ATTRIBUTE = 0,
        STREAM_SPARSE_ATTRIBUTE = 8
    }
}

