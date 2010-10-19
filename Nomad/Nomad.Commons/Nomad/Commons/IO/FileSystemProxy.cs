namespace Nomad.Commons.IO
{
    using Microsoft.IO;
    using Microsoft.Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    public class FileSystemProxy : IFileProxy, IDirectoryProxy, IFileSystemProxy
    {
        private static FileSystemProxy _Default;

        public void CreateHardLink(string fileName, string targetFileName)
        {
            Microsoft.IO.File.CreateHardLink(fileName, targetFileName);
        }

        public void CreateJunctionPoint(string emptyFolderPath, string targetFolderPath)
        {
            ReparsePoint.Create(emptyFolderPath, targetFolderPath);
        }

        public void CreateSymbolicLink(string fileName, string targetFileName, bool targetIsDirectory)
        {
            Microsoft.IO.File.CreateSymbolicLink(fileName, targetFileName, targetIsDirectory);
        }

        public IEnumerable<AlternateDataStreamInfo> EnumerateAlternateStreams(string fileName)
        {
            return AlternateDataStreamInfo.GetStreams(fileName);
        }

        public AlternateDataStreamInfo GetAlternateStream(string fileName, string streamName)
        {
            return new AlternateDataStreamInfo(fileName, streamName);
        }

        public void MoveToRecycleBin(string fileName)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT {
                wFunc = FO.FO_DELETE,
                fFlags = FOF.FOF_SILENT | FOF.FOF_ALLOWUNDO | FOF.FOF_NOCONFIRMATION,
                pFrom = fileName + '\0'
            };
            int error = Shell32.SHFileOperation(ref lpFileOp);
            if (error != 0)
            {
                switch (error)
                {
                    case 0x20:
                    {
                        Exception exception = new Win32Exception(error);
                        throw new FileInUseException(exception.Message, fileName);
                    }
                    case 120:
                        throw new UnauthorizedAccessException();
                }
                throw new Win32IOException(error);
            }
        }

        bool IDirectoryProxy.Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        DirectoryInfo IDirectoryProxy.Get(string path)
        {
            return new DirectoryInfo(path);
        }

        void IDirectoryProxy.SetCompressedState(string directoryPath, bool compress)
        {
            Microsoft.IO.Directory.SetCompressedState(directoryPath, compress);
        }

        void IDirectoryProxy.SetCreationTime(string path, DateTime creationTime)
        {
            Microsoft.IO.Directory.SetCreationTime(path, creationTime);
        }

        void IDirectoryProxy.SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            Microsoft.IO.Directory.SetLastAccessTime(path, lastAccessTime);
        }

        void IDirectoryProxy.SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            Microsoft.IO.Directory.SetLastWriteTime(path, lastWriteTime);
        }

        void IFileProxy.Delete(string fileName)
        {
            System.IO.File.Delete(fileName);
        }

        bool IFileProxy.Exists(string fileName)
        {
            return System.IO.File.Exists(fileName);
        }

        FileInfo IFileProxy.Get(string fileName)
        {
            return new FileInfo(fileName);
        }

        void IFileProxy.SetCompressedState(string fileName, bool compress)
        {
            Microsoft.IO.File.SetCompressedState(fileName, compress);
        }

        void IFileProxy.SetCreationTime(string fileName, DateTime creationTime)
        {
            Microsoft.IO.File.SetCreationTime(fileName, creationTime);
        }

        void IFileProxy.SetLastAccessTime(string fileName, DateTime lastAccessTime)
        {
            Microsoft.IO.File.SetLastAccessTime(fileName, lastAccessTime);
        }

        void IFileProxy.SetLastWriteTime(string fileName, DateTime lastWriteTime)
        {
            Microsoft.IO.File.SetLastWriteTime(fileName, lastWriteTime);
        }

        public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return System.IO.File.Open(path, mode, access, share);
        }

        public static FileSystemProxy Default
        {
            get
            {
                if (_Default == null)
                {
                }
                return (_Default = new FileSystemProxy());
            }
        }
    }
}

