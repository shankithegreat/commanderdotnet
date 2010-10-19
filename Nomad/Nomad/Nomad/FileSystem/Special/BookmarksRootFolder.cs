namespace Nomad.FileSystem.Special
{
    using Nomad;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Drawing;

    internal class BookmarksRootFolder : CustomFileSystemFolder, ICreateVirtualFile, ICreateVirtualFolder
    {
        public BookmarksRootFolder() : base(SettingsManager.SpecialFolders.Bookmarks, null)
        {
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            if (defaultIcon)
            {
                return ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, size);
            }
            return ImageProvider.Default.GetFolderIcon(base.FullName, size);
        }
    }
}

