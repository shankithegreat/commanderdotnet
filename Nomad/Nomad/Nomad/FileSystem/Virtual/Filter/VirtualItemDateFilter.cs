namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.IO;

    [Serializable]
    public class VirtualItemDateFilter : DateFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        [DefaultValue(typeof(ItemDateTimePart), "LastWriteTime")]
        public ItemDateTimePart DatePart;

        public VirtualItemDateFilter()
        {
            this.DatePart = ItemDateTimePart.LastWriteTime;
        }

        public VirtualItemDateFilter(DateComparision Comparision, DateTime Date) : base(Comparision, Date)
        {
            this.DatePart = ItemDateTimePart.LastWriteTime;
        }

        public VirtualItemDateFilter(int NotOlderThan, DateUnit Measure) : base(NotOlderThan, Measure)
        {
            this.DatePart = ItemDateTimePart.LastWriteTime;
        }

        public VirtualItemDateFilter(DateComparision Comparision, DateTime FromDate, DateTime ToDate) : base(Comparision, FromDate, ToDate)
        {
            this.DatePart = ItemDateTimePart.LastWriteTime;
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public override bool EqualTo(object obj)
        {
            VirtualItemDateFilter filter = obj as VirtualItemDateFilter;
            return (((filter != null) && base.EqualTo(obj)) && (filter.DatePart == this.DatePart));
        }

        public bool IsMatch(IVirtualItem item)
        {
            object obj2;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            switch (this.DatePart)
            {
                case ItemDateTimePart.LastAccessTime:
                    obj2 = item[9];
                    break;

                case ItemDateTimePart.CreationTime:
                    obj2 = item[7];
                    break;

                case ItemDateTimePart.LastWriteTime:
                    obj2 = item[8];
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
            return ((obj2 != null) && base.MatchDate((DateTime) obj2));
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            switch (this.DatePart)
            {
                case ItemDateTimePart.LastAccessTime:
                    return base.MatchDate(item.LastAccessTime);

                case ItemDateTimePart.CreationTime:
                    return base.MatchDate(item.CreationTime);

                case ItemDateTimePart.LastWriteTime:
                    return base.MatchDate(item.LastWriteTime);
            }
            throw new InvalidEnumArgumentException();
        }
    }
}

