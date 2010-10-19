namespace Nomad.Workers
{
    using System;

    public enum PackStage
    {
        NotStarted,
        CalculatingSize,
        ReadingExistingArchive,
        MovingExistingItems,
        PackingNewItems,
        Relocating,
        Finished
    }
}

