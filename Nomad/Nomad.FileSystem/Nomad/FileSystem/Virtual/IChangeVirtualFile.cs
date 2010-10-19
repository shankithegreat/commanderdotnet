namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;
    using System.IO;

    public interface IChangeVirtualFile : IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset);

        bool CanSeek { get; }
    }
}

