namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using System;

    public sealed class FilterHelper
    {
        public static bool FilterEquals(IVirtualItemFilter filter1, IVirtualItemFilter filter2)
        {
            return (((filter1 == null) && (filter2 == null)) || (((filter1 != null) && (filter2 != null)) && filter1.Equals(filter2)));
        }

        public static bool HasContentFilter(IVirtualItemFilter filter)
        {
            if (filter is CustomContentFilter)
            {
                return true;
            }
            AggregatedVirtualItemFilter filter2 = filter as AggregatedVirtualItemFilter;
            if (filter2 != null)
            {
                foreach (IVirtualItemFilter filter3 in filter2.Filters)
                {
                    if (HasContentFilter(filter3))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

