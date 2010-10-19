namespace Nomad.FileSystem.Virtual
{
    using System;

    [Flags]
    public enum PathView
    {
        ShowActiveState = 4,
        ShowDriveMenuOnHover = 8,
        ShowFolderIcon = 0x20,
        ShowIconForEveryFolder = 2,
        ShowNormalRootName = 0,
        ShowShortRootName = 1,
        VistaLikeBreadcrumb = 0x10
    }
}

