namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum CustomizeFolderParts
    {
        All = 0xff,
        Colors = 0x40,
        Columns = 1,
        Filter = 4,
        Icon = 2,
        ListColumnCount = 0x80,
        Sort = 8,
        ThumbnailSize = 0x20,
        View = 0x10
    }
}

