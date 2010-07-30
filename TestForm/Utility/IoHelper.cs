using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Commander.Utility
{
    public static class IoExtensions
    {
        public static DriveInfo GetDrive(this DirectoryInfo directory)
        {
            return new DriveInfo(directory.Root.FullName);
        }

        public static bool Equals(DriveInfo a, DriveInfo b)
        {
            return a == b || (a != null && b != null && a.Name == b.Name);
        }

        public static bool Equals(FileSystemInfo a, FileSystemInfo b)
        {
            return a == b || (a != null && b != null && a.FullName == b.FullName);
        }

        public static long GetDirectorySize(this DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            try
            {
                foreach (FileInfo file in d.GetFiles())
                {
                    size += file.Length;
                }

            }
            catch (System.UnauthorizedAccessException)
            {
            }
            // Add subdirectory sizes.
            try
            {
                foreach (DirectoryInfo directory in d.GetDirectories())
                {
                    size += GetDirectorySize(directory);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
            }

            return (size);
        }

    }
}
