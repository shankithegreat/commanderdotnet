namespace Nomad.Workers
{
    using System;

    public enum ArchiveUpdateMode
    {
        CreateNew,
        OverwriteAll,
        SkipAll,
        RefreshOld
    }
}

