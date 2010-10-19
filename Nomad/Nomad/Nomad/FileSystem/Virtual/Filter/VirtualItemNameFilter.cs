namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;

    [Serializable]
    public class VirtualItemNameFilter : NameFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        public VirtualItemNameFilter()
        {
        }

        public VirtualItemNameFilter(string Pattern) : base(Pattern)
        {
        }

        public VirtualItemNameFilter(NamePatternComparision Comparision, string Pattern) : base(Comparision, Pattern)
        {
        }

        public VirtualItemNameFilter(NamePatternCondition Condition, string Pattern) : base(Condition, Pattern)
        {
        }

        public VirtualItemNameFilter(NamePatternCondition Condition, NamePatternComparision Comparision, string Pattern) : base(Condition, Comparision, Pattern)
        {
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return (((other != null) && (other.GetType() == typeof(VirtualItemNameFilter))) && this.EqualTo(other));
        }

        public bool IsMatch(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return base.MatchName(item.Name);
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return base.MatchName(item.Name);
        }
    }
}

