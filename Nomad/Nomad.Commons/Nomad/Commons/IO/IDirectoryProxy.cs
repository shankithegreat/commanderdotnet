namespace Nomad.Commons.IO
{
    using System;
    using System.IO;

    public interface IDirectoryProxy : IFileSystemProxy
    {
        void CreateJunctionPoint(string emptyFolderPath, string targetFolderPath);
        bool Exists(string path);
        DirectoryInfo Get(string path);
        void SetCompressedState(string directoryPath, bool compress);
        void SetCreationTime(string path, DateTime creationTime);
        void SetLastAccessTime(string path, DateTime lastAccessTime);
        void SetLastWriteTime(string path, DateTime lastWriteTime);
    }
}

