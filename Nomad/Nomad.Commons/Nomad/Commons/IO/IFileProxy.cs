namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public interface IFileProxy : IFileSystemProxy
    {
        void CreateHardLink(string fileName, string targetFileName);
        void CreateSymbolicLink(string fileName, string targetFileName, bool targetIsDirectory);
        void Delete(string fileName);
        bool Exists(string fileName);
        FileInfo Get(string fileName);
        Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
        void SetCompressedState(string fileName, bool compress);
        void SetCreationTime(string fileName, DateTime creationTime);
        void SetLastAccessTime(string fileName, DateTime lastAccessTime);
        void SetLastWriteTime(string fileName, DateTime lastWriteTime);
    }
}

