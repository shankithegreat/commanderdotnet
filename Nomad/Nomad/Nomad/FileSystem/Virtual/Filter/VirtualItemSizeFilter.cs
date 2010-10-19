namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class VirtualItemSizeFilter : SizeFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        public VirtualItemSizeFilter()
        {
        }

        public VirtualItemSizeFilter(SizeComparision comparision, long size) : base(comparision, size)
        {
        }

        public VirtualItemSizeFilter(SizeComparision comparision, long fromSize, long toSize) : base(comparision, fromSize, toSize)
        {
        }

        public VirtualItemSizeFilter(SizeComparision comparision, long fromSize, long toSize, SizeUnit unit) : base(comparision, fromSize, toSize, unit)
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
            if (item.GetPropertyAvailability(3) == PropertyAvailability.Normal)
            {
                object obj2 = item[3];
                if (obj2 != null)
                {
                    return this.MatchValue(Convert.ToInt64(obj2));
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
            return ((info != null) && this.MatchValue(info.Length));
        }

        bool IEquatable<IVirtualItemFilter>.Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }
    }
}

