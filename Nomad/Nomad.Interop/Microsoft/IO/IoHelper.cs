namespace Microsoft.IO
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    internal static class IoHelper
    {
        private static FieldInfo Field_FileSystemInfo_data;
        private static FieldInfo Field_FileSystemInfo_dataInitialized;
        private static Type Type_WIN32_FILE_ATTRIBUTE_DATA = Type.GetType("Microsoft.Win32.Win32Native+WIN32_FILE_ATTRIBUTE_DATA,mscorlib");

        static IoHelper()
        {
            Type type = typeof(FileSystemInfo);
            Field_FileSystemInfo_data = type.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
            Field_FileSystemInfo_dataInitialized = type.GetField("_dataInitialised", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        internal static string ConvertFileName(string fileName)
        {
            if ((Environment.OSVersion.Platform == PlatformID.Win32NT) && (fileName.Length > 260))
            {
                if (fileName.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
                {
                    return (@"\\?\UNC" + fileName.Substring(1));
                }
                return (@"\\?\" + fileName);
            }
            return fileName;
        }

        internal static System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFileTime(DateTime time)
        {
            long num = time.ToFileTimeUtc();
            return new System.Runtime.InteropServices.ComTypes.FILETIME { dwHighDateTime = (int) (num >> 0x20), dwLowDateTime = (int) (((ulong) num) & 0xffffffffL) };
        }

        internal static DateTime FileTimeToDateTime(System.Runtime.InteropServices.ComTypes.FILETIME time)
        {
            return DateTime.FromFileTimeUtc((time.dwHighDateTime << 0x20) | ((long) ((ulong) time.dwLowDateTime)));
        }

        internal static Exception GetIOException()
        {
            return GetWin32IOException(new Win32Exception());
        }

        internal static Exception GetIOException(int nativeErrorCode)
        {
            return GetWin32IOException(new Win32Exception(nativeErrorCode));
        }

        private static Exception GetWin32IOException(Win32Exception Win32Error)
        {
            switch (Win32Error.NativeErrorCode)
            {
                case 2:
                    return new FileNotFoundException(Win32Error.Message, Win32Error);

                case 3:
                    return new DirectoryNotFoundException(Win32Error.Message, Win32Error);

                case 5:
                case 0x522:
                    return new UnauthorizedAccessException(Win32Error.Message, Win32Error);

                case 0x15:
                    return new DeviceNotReadyException(Win32Error.Message, Win32Error);
            }
            return new Win32IOException(Win32Error);
        }

        internal static void InitializeFileSystemInfo(FileSystemInfo info, Microsoft.Win32.WIN32_FIND_DATA findData)
        {
            Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA fileAttributeData = new Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA {
                dwFileAttributes = findData.dwFileAttributes,
                ftCreationTime = findData.ftCreationTime,
                ftLastAccessTime = findData.ftLastAccessTime,
                ftLastWriteTime = findData.ftLastWriteTime,
                nFileSizeHigh = findData.nFileSizeHigh,
                nFileSizeLow = findData.nFileSizeLow
            };
            InitializeFileSystemInfo(info, ref fileAttributeData);
        }

        internal static void InitializeFileSystemInfo(FileSystemInfo info, ref Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA fileAttributeData)
        {
            object obj2;
            GCHandle handle = GCHandle.Alloc((Microsoft.Win32.WIN32_FILE_ATTRIBUTE_DATA) fileAttributeData, GCHandleType.Pinned);
            try
            {
                obj2 = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), Type_WIN32_FILE_ATTRIBUTE_DATA);
            }
            finally
            {
                handle.Free();
            }
            Field_FileSystemInfo_data.SetValue(info, obj2);
            Field_FileSystemInfo_dataInitialized.SetValue(info, 0);
        }

        internal static string StripTrailingPathSeparator(string filePath)
        {
            int length = filePath.Length - 1;
            if ((filePath[length] == Path.DirectorySeparatorChar) || (filePath[length] == Path.AltDirectorySeparatorChar))
            {
                return filePath.Substring(0, length);
            }
            return filePath;
        }
    }
}

