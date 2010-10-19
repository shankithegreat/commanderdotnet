namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum CacheState
    {
        HasContent = 2,
        HasFolders = 8,
        HasItems = 4,
        Unavailable = 1,
        Unknown = 0
    }
}

