namespace Nomad.Commons.IO
{
    using Microsoft;
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class PathHelper
    {
        private static Dictionary<string, bool> ExecutableExtMap;
        public static readonly string UncPrefix = new string(Path.DirectorySeparatorChar, 2);

        public static IEnumerable<string> ApplyFileOrder(IEnumerable<string> files, string fileOrderPath)
        {
            if (!File.Exists(fileOrderPath))
            {
                return files;
            }
            List<string> list = new List<string>(files);
            Dictionary<string, int> OrderMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            using (TextReader reader = File.OpenText(fileOrderPath))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    if (!OrderMap.ContainsKey(str))
                    {
                        OrderMap.Add(str, OrderMap.Count);
                    }
                }
            }
            list.Sort(delegate (string file1, string file2) {
                int num;
                int num2;
                if (!OrderMap.TryGetValue(Path.GetFileName(file1), out num))
                {
                    num = -1;
                }
                if (!OrderMap.TryGetValue(Path.GetFileName(file2), out num2))
                {
                    num2 = -1;
                }
                if ((num >= 0) && (num2 >= 0))
                {
                    return num - num2;
                }
                if (num >= 0)
                {
                    return -1;
                }
                if (num2 >= 0)
                {
                    return 1;
                }
                return 0;
            });
            return list;
        }

        public static string ExcludeTrailingDirectorySeparator(string folderPath)
        {
            if (HasTrailingDirectorySeparator(folderPath))
            {
                return folderPath.Substring(0, folderPath.Length - 1);
            }
            return folderPath;
        }

        public static PathType GetPathType(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length == 0)
            {
                return ~PathType.Unknown;
            }
            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return ~PathType.Unknown;
            }
            PathType unknown = PathType.Unknown;
            if (path.IndexOf(Uri.SchemeDelimiter, StringComparison.Ordinal) > 0)
            {
                unknown |= PathType.Uri;
                try
                {
                    Uri uri = new Uri(path);
                    if (path.StartsWith(Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
                    {
                        path = uri.LocalPath;
                    }
                    else
                    {
                        if (!uri.AbsolutePath.Equals("/", StringComparison.Ordinal))
                        {
                            unknown |= uri.AbsolutePath.EndsWith('/') ? 2 : 4;
                        }
                        return unknown;
                    }
                }
                catch
                {
                    return ~PathType.Unknown;
                }
            }
            if (path.StartsWith(UncPrefix, StringComparison.Ordinal))
            {
                string str = path.Substring(UncPrefix.Length);
                if ((path.Length == 2) || (str.IndexOf(Path.VolumeSeparatorChar) >= 0))
                {
                    return ~PathType.Unknown;
                }
                int index = path.IndexOf(Path.DirectorySeparatorChar, 2);
                if ((index < 0) || (index == (path.Length - 1)))
                {
                    return (unknown | PathType.NetworkServer);
                }
                unknown |= PathType.NetworkShare;
                index = path.IndexOf(Path.DirectorySeparatorChar, index + 1);
                if ((index < 0) || (index == (path.Length - 1)))
                {
                    return unknown;
                }
            }
            else
            {
                if (path.StartsWith(Path.DirectorySeparatorChar))
                {
                    unknown |= PathType.Relative | PathType.Volume;
                }
                else if (Path.IsPathRooted(path))
                {
                    unknown |= PathType.Volume;
                    try
                    {
                        if (path.Equals(Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase) && (path.Length < 4))
                        {
                            return unknown;
                        }
                    }
                    catch (ArgumentException)
                    {
                        return ~PathType.Unknown;
                    }
                }
                else
                {
                    unknown = PathType.Relative;
                }
                if ((path.Length > 2) && (path.IndexOf(Path.VolumeSeparatorChar, 2) > 0))
                {
                    return ~PathType.Unknown;
                }
            }
            return (unknown | (path.EndsWith(Path.DirectorySeparatorChar) ? 2 : 4));
        }

        public static bool HasTrailingDirectorySeparator(string folderPath)
        {
            if (!string.IsNullOrEmpty(folderPath))
            {
                char ch = folderPath[folderPath.Length - 1];
                return ((ch == Path.DirectorySeparatorChar) || (ch == Path.AltDirectorySeparatorChar));
            }
            return false;
        }

        public static string IncludeTrailingDirectorySeparator(string folderPath)
        {
            if (!(string.IsNullOrEmpty(folderPath) || HasTrailingDirectorySeparator(folderPath)))
            {
                return (folderPath + Path.DirectorySeparatorChar);
            }
            return folderPath;
        }

        public static bool IsExecutableFile(string filePath)
        {
            bool flag;
            if (ExecutableExtMap == null)
            {
                ExecutableExtMap = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
                ExecutableExtMap.Add(".exe", true);
                ExecutableExtMap.Add(".com", true);
                ExecutableExtMap.Add(".bat", true);
                if (OS.IsWinNT)
                {
                    ExecutableExtMap.Add(".cmd", true);
                }
                foreach (string str in StringHelper.SplitString(Environment.GetEnvironmentVariable("pathext"), new char[] { ';' }))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        ExecutableExtMap[str] = true;
                    }
                }
            }
            return (ExecutableExtMap.TryGetValue(Path.GetExtension(filePath), out flag) && flag);
        }

        public static bool IsRootPath(string path)
        {
            return string.Equals(Path.GetPathRoot(path), path, StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeInvalidFileName(string fileName)
        {
            return NormalizeInvalidFileName(fileName, Path.GetInvalidFileNameChars());
        }

        private static string NormalizeInvalidFileName(string fileName, char[] invalidChars)
        {
            StringBuilder builder = null;
            for (int i = 0; i < fileName.Length; i++)
            {
                if (Array.IndexOf<char>(invalidChars, fileName[i]) >= 0)
                {
                    if (builder == null)
                    {
                        builder = new StringBuilder(fileName);
                    }
                    builder[i] = '_';
                }
            }
            if (builder != null)
            {
                return builder.ToString();
            }
            return fileName;
        }

        public static string NormalizeInvalidPath(string path)
        {
            return NormalizeInvalidFileName(path, Path.GetInvalidPathChars());
        }
    }
}

