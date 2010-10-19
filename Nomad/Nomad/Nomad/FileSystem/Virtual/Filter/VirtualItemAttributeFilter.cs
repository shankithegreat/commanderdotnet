namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class VirtualItemAttributeFilter : AttributeFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        public VirtualItemAttributeFilter()
        {
        }

        public VirtualItemAttributeFilter(FileAttributes Include) : base(Include)
        {
        }

        public VirtualItemAttributeFilter(FileAttributes Include, FileAttributes Exclude) : base(Include, Exclude)
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
                throw new ArgumentNullException("item");
            }
            return base.MatchAttributes(item.Attributes);
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return base.MatchAttributes(item.Attributes);
        }
    }
}

