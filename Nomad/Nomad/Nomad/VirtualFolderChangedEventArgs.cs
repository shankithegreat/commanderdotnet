namespace Nomad
{
    using Nomad.FileSystem.Virtual;
    using System;

    public class VirtualFolderChangedEventArgs : EventArgs
    {
        public readonly IVirtualFolder CurrentFolder;
        public readonly IVirtualFolder PreviousFolder;

        public VirtualFolderChangedEventArgs(IVirtualFolder oldFolder, IVirtualFolder newFolder)
        {
            this.PreviousFolder = oldFolder;
            this.CurrentFolder = newFolder;
        }
    }
}

