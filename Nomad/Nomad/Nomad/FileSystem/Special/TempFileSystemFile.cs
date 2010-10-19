namespace Nomad.FileSystem.Special
{
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Virtual;
    using System;

    internal class TempFileSystemFile : FileSystemFile
    {
        private string _Name;

        public TempFileSystemFile(string fullName, string name, IVirtualFolder parent) : base(fullName, parent)
        {
            this._Name = name;
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

