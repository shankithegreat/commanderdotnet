﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ShellDll
{
    public static class ShellFolder
    {
        public static FileSystemInfo GetFileSystemInfo(string path)
        {
            if (File.Exists(path))
            {
                return new FileInfo(path);
            }
            else
            {
                return new DirectoryInfo(path);
            }
        }

        public static DirectoryInfo GetParentDirectory(FileSystemInfo item)
        {
            if (item is FileInfo)
            {
                FileInfo file = (FileInfo)item;
                return file.Directory;
            }
            else
            {
                DirectoryInfo directory = (DirectoryInfo)item;
                return directory.Parent;
            }
        }

        public static string GetParentDirectoryPath(FileSystemInfo item)
        {
            DirectoryInfo parentDirectory = GetParentDirectory(item);
            if (parentDirectory == null)
            {
                return SpecialFolderPath.MyComputer;
            }

            return parentDirectory.FullName;
        }

        public static string GetParentDirectoryPath(string path)
        {
            return GetParentDirectoryPath(GetFileSystemInfo(path));
        }

        public static IntPtr GetPathPIDL(string path)
        {
            if (path.StartsWith("::{"))
            {
                return GetPathPIDL(null, path);
            }
            else
            {
                if (path.EndsWith(@"\") && !path.EndsWith(@":\"))
                {
                    path = path.Remove(path.Length - 1);
                }
                string parentDirectory = Path.GetDirectoryName(path);
                if (parentDirectory == null)
                {
                    parentDirectory = SpecialFolderPath.MyComputer;
                }
                string name = Path.GetFileName(path);
                if (string.IsNullOrEmpty(name))
                {
                    name = path;
                }

                return GetPathPIDL(parentDirectory, name);
            }
        }

        public static IntPtr GetPathPIDL(FileSystemInfo item)
        {
            string parentDirectory = GetParentDirectoryPath(item);

            string name = item.Name;

            return GetPathPIDL(parentDirectory, name);
        }

        public static IntPtr GetPathPIDL(string parentDirectory, string name)
        {
            IShellFolder parentFolder = (!string.IsNullOrEmpty(parentDirectory) ? GetShellFolder(parentDirectory) : GetDesktopFolder());
            if (parentFolder != null)
            {
                uint pchEaten = 0;
                SFGAO attributes = 0;
                IntPtr pidl;
                parentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, name, ref pchEaten, out pidl, ref attributes);

                return pidl;
            }

            return IntPtr.Zero;
        }

        public static IntPtr[] GetPIDLs(params FileSystemInfo[] list)
        {
            List<IntPtr> pidls = new List<IntPtr>(list.Length);

            foreach (FileSystemInfo item in list)
            {
                pidls.Add(GetPathPIDL(item.FullName));
            }

            return pidls.ToArray();
        }

        public static IntPtr[] GetPIDLs(params string[] pathList)
        {
            List<IntPtr> pidls = new List<IntPtr>(pathList.Length);

            foreach (string path in pathList)
            {
                pidls.Add(GetPathPIDL(path));
            }

            return pidls.ToArray();
        }

        public static IntPtr GetShellFolderIntPtr(string path)
        {
            IShellFolder desktopFolder = GetDesktopFolder();

            // Get PIDL            
            uint pchEaten = 0;
            IntPtr pidl;
            SFGAO pdwAttributes = 0;
            desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, path, ref pchEaten, out pidl, ref pdwAttributes);

            // Get IShellFolder
            IntPtr shellFolder;
            int result = desktopFolder.BindToObject(pidl, IntPtr.Zero, ref ShellGuids.IShellFolder, out shellFolder);
            Marshal.FreeCoTaskMem(pidl);
            if (result != 0)
            {
                return IntPtr.Zero;
            }

            return shellFolder;
        }

        public static IShellFolder GetShellFolder(string path)
        {
            IntPtr shellFolder = GetShellFolderIntPtr(path);

            return (IShellFolder)Marshal.GetTypedObjectForIUnknown(shellFolder, typeof(IShellFolder));
        }

        public static IShellFolder GetParentShellFolder(FileSystemInfo item)
        {
            string parentDirectory = GetParentDirectoryPath(item);
            IShellFolder parentShellFolder = GetShellFolder(parentDirectory);

            return parentShellFolder;
        }

        public static IShellFolder GetParentShellFolder(string path)
        {
            return GetParentShellFolder(GetFileSystemInfo(path));
        }

        public static IShellFolder GetDesktopFolder()
        {
            IntPtr p = IntPtr.Zero;
            ShellApi.SHGetDesktopFolder(out p);

            IShellFolder result = (IShellFolder)Marshal.GetTypedObjectForIUnknown(p, typeof(IShellFolder));
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }
    }
}