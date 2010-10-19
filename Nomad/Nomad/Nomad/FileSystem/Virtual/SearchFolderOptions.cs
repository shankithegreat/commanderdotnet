namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum SearchFolderOptions
    {
        AsyncSearch = 0x20,
        AutoAsyncSearch = 0x40,
        DetectChanges = 0x10,
        ExpandAggregatedRoot = 0x80,
        ProcessArchives = 2,
        ProcessSubfolders = 1,
        SkipReparsePoints = 8,
        SkipUnmatchedSubfolders = 4
    }
}

