namespace Nomad.FileSystem.Special
{
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    internal class TempFileSystemFolder : FileSystemFolder
    {
        private string _Name;

        public TempFileSystemFolder(DirectoryInfo info, string name, IVirtualFolder parent) : base(info, parent)
        {
            this._Name = name;
        }

        public override void Delete(bool sendToBin)
        {
            base.FolderInfo.Delete(true);
        }

        public override bool CanSendToBin
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get
            {
                return this._Name;
            }
        }
    }
}

