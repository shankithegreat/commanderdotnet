namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class VirtualItemContentFilter : ContentFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        public VirtualItemContentFilter()
        {
        }

        public VirtualItemContentFilter(string text) : base(text)
        {
        }

        public VirtualItemContentFilter(string text, ContentFilterOptions options) : base(text, options)
        {
        }

        public VirtualItemContentFilter(ContentComparision comparision, string text, ContentFilterOptions options) : base(comparision, text, options)
        {
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public bool IsMatch(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            IChangeVirtualFile file = item as IChangeVirtualFile;
            if (file != null)
            {
                using (Stream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.Asynchronous | FileOptions.SequentialScan, 0L))
                {
                    return this.MatchStream(stream, item.Name);
                }
            }
            return false;
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            FileInfo info = item as FileInfo;
            if (info != null)
            {
                using (Stream stream = info.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return this.MatchStream(stream, item.Name);
                }
            }
            return false;
        }
    }
}

