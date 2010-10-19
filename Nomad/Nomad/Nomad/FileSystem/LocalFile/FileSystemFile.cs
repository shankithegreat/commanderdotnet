namespace Nomad.FileSystem.LocalFile
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class FileSystemFile : CustomFileSystemFile, IChangeVirtualItem, IPersistVirtualItem, ISetVirtualProperty, IChangeVirtualFile, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICreateVirtualLink, ICloneable
    {
        public FileSystemFile(FileInfo info) : base(info, null)
        {
        }

        public FileSystemFile(string fileName) : base(fileName, null)
        {
        }

        public FileSystemFile(FileInfo info, IVirtualFolder parent) : base(info, parent)
        {
        }

        public FileSystemFile(string fileName, IVirtualFolder parent) : base(fileName, parent)
        {
        }

        public object Clone()
        {
            return this.InternalClone();
        }

        string IChangeVirtualItem.Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                base.SetName(value);
            }
        }
    }
}

