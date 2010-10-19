namespace Microsoft.IO
{
    using Microsoft.Win32;
    using System;
    using System.IO;

    [Serializable]
    public class ReadOnlyFileSystemInfo
    {
        private DateTime FCreationTime;
        private FileAttributes FFileAttributes;
        private string FFullName;
        private DateTime FLastAccessTime;
        private DateTime FLastWriteTime;
        private string FName;
        private long FSize;

        public ReadOnlyFileSystemInfo(string fullName)
        {
            Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA win_file_attribute_data;
            if (fullName == null)
            {
                throw new ArgumentNullException();
            }
            if (fullName == string.Empty)
            {
                throw new ArgumentException();
            }
            if (!Windows.GetFileAttributesEx(IoHelper.ConvertFileName(fullName), GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out win_file_attribute_data))
            {
                throw IoHelper.GetIOException();
            }
            this.Initialize(ref win_file_attribute_data);
        }

        internal ReadOnlyFileSystemInfo(string fullName, ref Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA fileData)
        {
            this.FFullName = fullName;
            this.Initialize(ref fileData);
        }

        internal ReadOnlyFileSystemInfo(string directory, Microsoft.Win32.WIN32_FIND_DATA findData)
        {
            this.FFullName = Path.Combine(directory, findData.cFileName);
            this.FFileAttributes = findData.dwFileAttributes;
            this.FCreationTime = IoHelper.FileTimeToDateTime(findData.ftCreationTime);
            this.FLastAccessTime = IoHelper.FileTimeToDateTime(findData.ftLastAccessTime);
            this.FLastWriteTime = IoHelper.FileTimeToDateTime(findData.ftLastWriteTime);
            this.FSize = (findData.nFileSizeHigh << 0x20) + findData.nFileSizeLow;
        }

        private void Initialize(ref Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA fileData)
        {
            this.FFileAttributes = fileData.dwFileAttributes;
            this.FCreationTime = IoHelper.FileTimeToDateTime(fileData.ftCreationTime);
            this.FLastAccessTime = IoHelper.FileTimeToDateTime(fileData.ftLastAccessTime);
            this.FLastWriteTime = IoHelper.FileTimeToDateTime(fileData.ftLastWriteTime);
            this.FSize = (fileData.nFileSizeHigh << 0x20) + fileData.nFileSizeLow;
        }

        public FileSystemInfo ToFileSystemInfo()
        {
            FileSystemInfo info;
            if (this.IsDirectory)
            {
                info = new DirectoryInfo(this.FFullName);
            }
            else
            {
                info = new FileInfo(this.FFullName);
            }
            Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA fileAttributeData = new Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA {
                dwFileAttributes = this.FFileAttributes,
                ftCreationTime = IoHelper.DateTimeToFileTime(this.FCreationTime),
                ftLastAccessTime = IoHelper.DateTimeToFileTime(this.FLastAccessTime),
                ftLastWriteTime = IoHelper.DateTimeToFileTime(this.FLastWriteTime),
                nFileSizeHigh = (uint) (this.FSize >> 0x20),
                nFileSizeLow = (uint) (((ulong) this.FSize) & 0xffffffffL)
            };
            IoHelper.InitializeFileSystemInfo(info, ref fileAttributeData);
            return info;
        }

        public FileAttributes Attributes
        {
            get
            {
                return this.FFileAttributes;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.FCreationTime;
            }
        }

        public string Extension
        {
            get
            {
                return (this.IsDirectory ? string.Empty : Path.GetExtension(this.FFullName));
            }
        }

        public string FullName
        {
            get
            {
                return this.FFullName;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return ((this.FFileAttributes & FileAttributes.Directory) > 0);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((this.FFileAttributes & FileAttributes.ReadOnly) > 0);
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                return this.FLastAccessTime;
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                return this.FLastWriteTime;
            }
        }

        public long Length
        {
            get
            {
                return this.FSize;
            }
        }

        public string Name
        {
            get
            {
                if (this.FName == null)
                {
                    this.FName = Path.GetFileName(this.FFullName);
                }
                return this.FName;
            }
        }
    }
}

