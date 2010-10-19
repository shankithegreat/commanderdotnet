namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class VirtualItemFullNameFilter : NameFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        private static string CachePath;
        private static object CachePathLock = new object();
        private static PathType CachePathType;

        public VirtualItemFullNameFilter()
        {
        }

        public VirtualItemFullNameFilter(string Pattern) : base(Pattern)
        {
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return (((other != null) && (other.GetType() == typeof(VirtualItemFullNameFilter))) && this.EqualTo(other));
        }

        private static PathType GetPathType(string path)
        {
            object obj2;
            lock ((obj2 = CachePathLock))
            {
                if (string.Equals(path, CachePath, StringComparison.OrdinalIgnoreCase))
                {
                    return CachePathType;
                }
            }
            PathType pathType = PathHelper.GetPathType(path);
            lock ((obj2 = CachePathLock))
            {
                CachePath = path;
                CachePathType = pathType;
            }
            return pathType;
        }

        public bool IsMatch(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return this.MatchFullName(item.FullName);
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return this.MatchFullName(item.FullName);
        }

        public bool MatchFullName(string FullName)
        {
            PathType pathType;
            if (base.NamePattern.StartsWith(Path.DirectorySeparatorChar))
            {
                pathType = GetPathType(FullName);
                if (((pathType & (PathType.NetworkShare | PathType.Volume)) > PathType.Unknown) && ((pathType & (PathType.File | PathType.Folder)) > PathType.Unknown))
                {
                    string pathRoot = Path.GetPathRoot(FullName);
                    if (!string.IsNullOrEmpty(pathRoot))
                    {
                        int length = pathRoot.Length;
                        if (pathRoot.EndsWith(Path.DirectorySeparatorChar))
                        {
                            length--;
                        }
                        FullName = FullName.Substring(length);
                    }
                }
            }
            else if (base.NamePattern.StartsWith('/'))
            {
                pathType = GetPathType(FullName);
                if (((pathType & PathType.Uri) > PathType.Unknown) && ((pathType & (PathType.File | PathType.Folder)) > PathType.Unknown))
                {
                    int startIndex = FullName.IndexOf(Uri.SchemeDelimiter) + Uri.SchemeDelimiter.Length;
                    if (startIndex > 0)
                    {
                        startIndex = FullName.IndexOf('/', startIndex);
                    }
                    if (startIndex > 0)
                    {
                        FullName = FullName.Substring(startIndex);
                    }
                }
            }
            if ((base.NameComparision == NamePatternComparision.StartsWith) && base.NamePattern.EndsWith(Path.DirectorySeparatorChar))
            {
                return ((FullName.Length > base.NamePattern.Length) && FullName.StartsWith(base.NamePattern, StringComparison.OrdinalIgnoreCase));
            }
            return base.MatchName(FullName);
        }
    }
}

