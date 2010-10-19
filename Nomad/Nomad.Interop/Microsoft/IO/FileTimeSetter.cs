namespace Microsoft.IO
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;
    using System.Runtime.InteropServices.ComTypes;

    public class FileTimeSetter : MarshalByRefObject, IDisposable
    {
        private string FFileName;
        private SafeFileHandle FHandle;
        private bool FIsFolder;
        private bool HasOldTimes;
        private DateTime? NewCreationTime;
        private DateTime? NewLastAccessTime;
        private DateTime? NewLastWriteTime;
        private DateTime OldCreationTime;
        private DateTime OldLastAccessTime;
        private DateTime OldLastWriteTime;

        public FileTimeSetter(SafeFileHandle handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException();
            }
            this.FHandle = handle;
        }

        public FileTimeSetter(string fileName, bool isFolder)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException();
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException();
            }
            this.FFileName = fileName;
            this.FIsFolder = isFolder;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            this.Set();
            if (this.FHandle != null)
            {
                this.FHandle.Close();
            }
            this.FHandle = null;
            this.FFileName = null;
        }

        ~FileTimeSetter()
        {
            this.Dispose(false);
        }

        private void OldTimesNeeded()
        {
            if (!this.HasOldTimes)
            {
                FILETIME ftCreationTime;
                FILETIME ftLastAccessTime;
                FILETIME ftLastWriteTime;
                if (this.FHandle != null)
                {
                    if (!Windows.GetFileTime(this.FHandle, out ftCreationTime, out ftLastAccessTime, out ftLastWriteTime))
                    {
                        throw IoHelper.GetIOException();
                    }
                }
                else
                {
                    Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA win_file_attribute_data;
                    if (this.FFileName == null)
                    {
                        throw new ObjectDisposedException("FileTimeSetter");
                    }
                    if (!Windows.GetFileAttributesEx(IoHelper.ConvertFileName(this.FFileName), GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out win_file_attribute_data))
                    {
                        throw IoHelper.GetIOException();
                    }
                    ftCreationTime = win_file_attribute_data.ftCreationTime;
                    ftLastAccessTime = win_file_attribute_data.ftLastAccessTime;
                    ftLastWriteTime = win_file_attribute_data.ftLastWriteTime;
                }
                this.OldCreationTime = IoHelper.FileTimeToDateTime(ftCreationTime);
                this.OldLastAccessTime = IoHelper.FileTimeToDateTime(ftLastAccessTime);
                this.OldLastWriteTime = IoHelper.FileTimeToDateTime(ftLastWriteTime);
                this.HasOldTimes = true;
            }
        }

        public void ResetModified()
        {
            this.NewCreationTime = null;
            this.NewLastAccessTime = null;
            this.NewLastWriteTime = null;
        }

        public bool Set()
        {
            if (!this.IsModified)
            {
                return false;
            }
            if (this.FHandle != null)
            {
                Microsoft.IO.File.SetFileTimes(this.FHandle, this.NewCreationTime, this.NewLastAccessTime, this.NewLastWriteTime);
            }
            else
            {
                if (this.FFileName == null)
                {
                    throw new ObjectDisposedException("FileTimeSetter");
                }
                using (SafeFileHandle handle = Windows.CreateFile(IoHelper.ConvertFileName(this.FFileName), 0x100, FileShare.Delete | FileShare.Write, IntPtr.Zero, FileMode.Open, this.FIsFolder ? ((FileOptions) 0x2000000) : FileOptions.None, IntPtr.Zero))
                {
                    if (handle.IsInvalid)
                    {
                        throw IoHelper.GetIOException();
                    }
                    Microsoft.IO.File.SetFileTimes(handle, this.NewCreationTime, this.NewLastAccessTime, this.NewLastWriteTime);
                }
            }
            this.ResetModified();
            this.HasOldTimes = false;
            return true;
        }

        public DateTime CreationTime
        {
            get
            {
                if (this.NewCreationTime.HasValue)
                {
                    return this.NewCreationTime.Value;
                }
                this.OldTimesNeeded();
                return this.OldCreationTime;
            }
            set
            {
                this.NewCreationTime = new DateTime?(value);
            }
        }

        public bool HasNewCreationTime
        {
            get
            {
                return this.NewCreationTime.HasValue;
            }
        }

        public bool HasNewLastAccessTime
        {
            get
            {
                return this.NewLastAccessTime.HasValue;
            }
        }

        public bool HasNewLastWriteTime
        {
            get
            {
                return this.NewLastWriteTime.HasValue;
            }
        }

        public bool IsModified
        {
            get
            {
                return ((this.NewCreationTime.HasValue || this.NewLastAccessTime.HasValue) || this.NewLastWriteTime.HasValue);
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                if (this.NewLastAccessTime.HasValue)
                {
                    return this.NewLastAccessTime.Value;
                }
                this.OldTimesNeeded();
                return this.OldLastAccessTime;
            }
            set
            {
                this.NewLastAccessTime = new DateTime?(value);
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                if (this.NewLastWriteTime.HasValue)
                {
                    return this.NewLastWriteTime.Value;
                }
                this.OldTimesNeeded();
                return this.OldLastWriteTime;
            }
            set
            {
                this.NewLastWriteTime = new DateTime?(value);
            }
        }
    }
}

