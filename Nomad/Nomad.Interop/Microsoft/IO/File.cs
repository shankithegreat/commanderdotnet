namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Win32;
    using Microsoft.Win32.IOCTL;
    using Microsoft.Win32.SafeHandles;
    using Microsoft.Win32.Security;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class File
    {
        public static void CreateHardLink(string fileName, string targetFileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException("String is empty", "fileName");
            }
            if (targetFileName == null)
            {
                throw new ArgumentNullException("targetFileName");
            }
            if (targetFileName == string.Empty)
            {
                throw new ArgumentException("String is empty", "targetFileName");
            }
            if (!Windows.CreateHardLink(fileName, targetFileName, IntPtr.Zero))
            {
                throw IoHelper.GetIOException();
            }
        }

        public static void CreateSymbolicLink(string fileName, string targetFileName, bool targetIsDirectory)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException("String is empty", "fileName");
            }
            if (targetFileName == null)
            {
                throw new ArgumentNullException("targetFileName");
            }
            if (targetFileName == string.Empty)
            {
                throw new ArgumentException("String is empty", "targetFileName");
            }
            if (!OS.IsWinVista)
            {
                throw new PlatformNotSupportedException();
            }
            Microsoft.Win32.Security.Security.ChangeCurrentProcessPrivilege("SeCreateSymbolicLinkPrivilege", true);
            SYMBOLIC_LINK dwFlags = targetIsDirectory ? SYMBOLIC_LINK.SYMBOLIC_LINK_FLAG_DIRECTORY : SYMBOLIC_LINK.SYMBOLIC_LINK_FLAG_FILE;
            if (!Windows.CreateSymbolicLink(fileName, targetFileName, dwFlags))
            {
                throw IoHelper.GetIOException();
            }
        }

        public static long GetCompressedFileSize(string fileName)
        {
            uint num;
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException("fileName is empty");
            }
            uint compressedFileSize = Windows.GetCompressedFileSize(fileName, out num);
            if (compressedFileSize == uint.MaxValue)
            {
                int nativeErrorCode = Marshal.GetLastWin32Error();
                if (nativeErrorCode != 0)
                {
                    throw IoHelper.GetIOException(nativeErrorCode);
                }
            }
            return (long) ((num << 0x20) | compressedFileSize);
        }

        public static CompressionFormat GetCompressedState(SafeFileHandle handle)
        {
            CompressionFormat format;
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            if (handle.IsClosed || handle.IsInvalid)
            {
                throw new ArgumentException("handle is closed or invalid");
            }
            IntPtr lpOutBuffer = Marshal.AllocHGlobal(2);
            try
            {
                uint num;
                if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_GET_COMPRESSION, IntPtr.Zero, 0, lpOutBuffer, 2, out num, IntPtr.Zero))
                {
                    throw IoHelper.GetIOException();
                }
                format = (CompressionFormat) ((ushort) Marshal.ReadInt16(lpOutBuffer));
            }
            finally
            {
                Marshal.FreeHGlobal(lpOutBuffer);
            }
            return format;
        }

        public static CompressionFormat GetCompressedState(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException("fileName is empty");
            }
            using (SafeFileHandle handle = OpenReadAttributes(fileName))
            {
                return GetCompressedState(handle);
            }
        }

        public static long GetFileSize(SafeFileHandle handle)
        {
            uint num2;
            if (handle == null)
            {
                throw new ArgumentNullException();
            }
            if (handle.IsInvalid)
            {
                throw new ArgumentException();
            }
            if (OS.IsWin2k)
            {
                long num;
                if (!Windows.GetFileSizeEx(handle, out num))
                {
                    throw IoHelper.GetIOException();
                }
                return num;
            }
            uint fileSize = Windows.GetFileSize(handle, out num2);
            if (fileSize == uint.MaxValue)
            {
                int nativeErrorCode = Marshal.GetLastWin32Error();
                if (nativeErrorCode != 0)
                {
                    throw IoHelper.GetIOException(nativeErrorCode);
                }
            }
            return (long) ((num2 << 0x20) | fileSize);
        }

        public static SafeFileHandle Open(string fileName, FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            SafeFileHandle handle = Windows.CreateFile(fileName, access, share, IntPtr.Zero, mode, options, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                throw IoHelper.GetIOException();
            }
            return handle;
        }

        public static SafeFileHandle OpenReadAttributes(string fileName)
        {
            return Open(fileName, FileMode.Open, 0, FileShare.ReadWrite, FileOptions.None);
        }

        public static void SetCompressedState(SafeFileHandle handle, CompressionFormat format)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }
            if (handle.IsClosed || handle.IsInvalid)
            {
                throw new ArgumentException("handle is closed or invalid");
            }
            IntPtr ptr = Marshal.AllocHGlobal(2);
            try
            {
                uint num;
                Marshal.WriteInt16(ptr, (short) format);
                if (!Microsoft.Win32.IOCTL.IOCTL.DeviceIoControl(handle, FSCTL.FSCTL_SET_COMPRESSION, ptr, 2, IntPtr.Zero, 0, out num, IntPtr.Zero))
                {
                    throw IoHelper.GetIOException();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void SetCompressedState(SafeFileHandle handle, bool compressed)
        {
            SetCompressedState(handle, compressed ? CompressionFormat.Default : CompressionFormat.None);
        }

        public static void SetCompressedState(string fileName, CompressionFormat format)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException("fileName is empty");
            }
            using (FileStream stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                SetCompressedState(stream.SafeFileHandle, format);
            }
        }

        public static void SetCompressedState(string fileName, bool compressed)
        {
            SetCompressedState(fileName, compressed ? CompressionFormat.Default : CompressionFormat.None);
        }

        public static void SetCreationTime(string fileName, DateTime creationTime)
        {
            DateTime? lastAccessTime = null;
            SetFileTimes(fileName, new DateTime?(creationTime), lastAccessTime, null);
        }

        internal static void SetFileTimes(SafeFileHandle handle, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr = IntPtr.Zero;
            IntPtr ptr3 = IntPtr.Zero;
            try
            {
                if (creationTime.HasValue)
                {
                    zero = Marshal.AllocHGlobal(8);
                    Marshal.WriteInt64(zero, creationTime.Value.ToFileTimeUtc());
                }
                if (lastAccessTime.HasValue)
                {
                    ptr = Marshal.AllocHGlobal(8);
                    Marshal.WriteInt64(ptr, lastAccessTime.Value.ToFileTimeUtc());
                }
                if (lastWriteTime.HasValue)
                {
                    ptr3 = Marshal.AllocHGlobal(8);
                    Marshal.WriteInt64(ptr3, lastWriteTime.Value.ToFileTimeUtc());
                }
                if (!Windows.SetFileTime(handle, zero, ptr, ptr3))
                {
                    throw IoHelper.GetIOException();
                }
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
                if (ptr3 != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr3);
                }
            }
        }

        private static void SetFileTimes(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime)
        {
            using (SafeFileHandle handle = Open(fileName, FileMode.Open, 0x100, FileShare.Delete | FileShare.Write, FileOptions.None))
            {
                SetFileTimes(handle, creationTime, lastAccessTime, lastWriteTime);
            }
        }

        public static void SetLastAccessTime(string fileName, DateTime lastAccessTime)
        {
            DateTime? creationTime = null;
            SetFileTimes(fileName, creationTime, new DateTime?(lastAccessTime), null);
        }

        public static void SetLastWriteTime(string fileName, DateTime lastWriteTime)
        {
            DateTime? creationTime = null;
            SetFileTimes(fileName, creationTime, null, new DateTime?(lastWriteTime));
        }
    }
}

