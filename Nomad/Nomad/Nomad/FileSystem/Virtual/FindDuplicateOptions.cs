namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum FindDuplicateOptions
    {
        SameContent = 4,
        SameName = 1,
        SameSize = 2
    }
}

