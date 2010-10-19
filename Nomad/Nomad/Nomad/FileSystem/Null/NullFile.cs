namespace Nomad.FileSystem.Null
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    public class NullFile : NullItem, IChangeVirtualFile, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        internal NullFile(string name) : base(name)
        {
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            if ((mode != FileMode.Create) && (mode != FileMode.CreateNew))
            {
                throw new ArgumentOutOfRangeException("mode");
            }
            if (access != FileAccess.Write)
            {
                throw new ArgumentOutOfRangeException("access");
            }
            return Stream.Null;
        }

        public bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public string Extension
        {
            get
            {
                return (string) base[1];
            }
        }
    }
}

