namespace Nomad.Themes
{
    using System;

    [Flags]
    public enum TreeViewStyles
    {
        ExplorerFadeButtons = 0x200,
        ExplorerFullRowSelect = 0x80,
        ExplorerHotTrack = 0x100,
        ExplorerShowButtons = 0x40,
        ExplorerShowLines = 0x20,
        FullRowSelect = 4,
        HotTrack = 8,
        ShowButtons = 2,
        ShowLines = 1,
        UseExplorerTheme = 0x10
    }
}

