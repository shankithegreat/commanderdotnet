namespace Nomad.FileSystem.Virtual.Filter
{
    using System;
    using System.IO;

    public interface IFileSystemInfoFilter
    {
        bool IsMatch(FileSystemInfo item);
    }
}

