namespace Nomad.Configuration
{
    using System;

    [Flags]
    public enum PanelLayoutEntry
    {
        All = 0x7f,
        Columns = 8,
        FolderBarOrientation = 2,
        FolderBarVisible = 1,
        ListColumnCount = 0x40,
        None = 0,
        ThumbnailSize = 0x20,
        ToolbarsVisible = 0x10,
        View = 4
    }
}

