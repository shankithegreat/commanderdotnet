namespace Nomad.Workers
{
    using System;

    [Flags]
    public enum CopyWorkerOptions
    {
        AsyncCopy = 4,
        AutoAsyncCopy = 8,
        CheckFreeSpace = 0x10,
        ClearROFromCD = 0x20,
        CopyACL = 0x40,
        CopyFolderTime = 0x100,
        CopyItemTime = 0x80,
        DeleteSource = 1,
        SkipEmptyFolders = 2,
        UseSystemCopy = 0x200
    }
}

