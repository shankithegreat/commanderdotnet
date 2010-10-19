namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.IO;

    [Serializable]
    public class VirtualItemTimeFilter : TimeFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter
    {
        [DefaultValue(typeof(ItemDateTimePart), "LastWriteTime")]
        public ItemDateTimePart TimePart;

        public VirtualItemTimeFilter()
        {
            this.TimePart = ItemDateTimePart.LastWriteTime;
        }

        public VirtualItemTimeFilter(TimeComparision comparision, TimeSpan time) : base(comparision, time)
        {
            this.TimePart = ItemDateTimePart.LastWriteTime;
        }

        public VirtualItemTimeFilter(TimeComparision comparision, TimeSpan fromTime, TimeSpan toTime) : base(comparision, fromTime, toTime)
        {
            this.TimePart = ItemDateTimePart.LastWriteTime;
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public override bool EqualTo(object obj)
        {
            VirtualItemTimeFilter filter = obj as VirtualItemTimeFilter;
            return (((filter != null) && base.EqualTo(obj)) && (filter.TimePart == this.TimePart));
        }

        public bool IsMatch(IVirtualItem item)
        {
            object obj2;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            switch (this.TimePart)
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
            if (obj2 != null)
            {
                DateTime time = (DateTime) obj2;
                return base.MatchTime(time.TimeOfDay);
            }
            return false;
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            switch (this.TimePart)
            {
                case ItemDateTimePart.LastAccessTime:
                    return base.MatchTime(item.LastAccessTime.TimeOfDay);

                case ItemDateTimePart.CreationTime:
                    return base.MatchTime(item.CreationTime.TimeOfDay);

                case ItemDateTimePart.LastWriteTime:
                    return base.MatchTime(item.LastWriteTime.TimeOfDay);
            }
            throw new InvalidEnumArgumentException();
        }
    }
}

