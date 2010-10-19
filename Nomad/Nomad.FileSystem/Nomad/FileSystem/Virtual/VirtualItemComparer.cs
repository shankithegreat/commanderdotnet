namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.ComponentModel;

    [Serializable, TypeConverter(typeof(VirtualItemComparerConverter))]
    public class VirtualItemComparer : VirtualComparer<IVirtualItem>
    {
        public static readonly VirtualItemComparer DefaultSort = new VirtualItemComparer(1, ListSortDirection.Ascending);

        public VirtualItemComparer(VirtualComparer<IVirtualItem> comparer, NameComparison comparison) : base(comparer, comparison)
        {
        }

        public VirtualItemComparer(VirtualComparer<IVirtualItem> comparer, ListSortDirection direction) : base(comparer, direction)
        {
        }

        public VirtualItemComparer(VirtualComparer<IVirtualItem> comparer, int propertyId) : base(comparer, propertyId)
        {
        }

        public VirtualItemComparer(int propertyId, ListSortDirection direction) : base(propertyId, direction)
        {
        }

        public VirtualItemComparer(int propertyId, ListSortDirection direction, NameComparison comparison) : base(propertyId, direction, comparison)
        {
        }
    }
}

