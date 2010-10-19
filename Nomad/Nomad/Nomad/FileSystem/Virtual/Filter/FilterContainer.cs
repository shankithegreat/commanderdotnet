namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    public class FilterContainer : IFilterContainter, IEquatable<IFilterContainter>
    {
        private IVirtualItemFilter FFilter;

        public FilterContainer()
        {
        }

        public FilterContainer(IVirtualItemFilter filter)
        {
            this.FFilter = filter;
        }

        public virtual bool Equals(IFilterContainter other)
        {
            return ((other != null) && FilterHelper.FilterEquals(this.Filter, other.Filter));
        }

        [XmlIgnore]
        public IVirtualItemFilter Filter
        {
            get
            {
                return this.FFilter;
            }
            set
            {
                this.FFilter = value;
            }
        }

        [XmlElement("AggregatedFilter", typeof(AggregatedVirtualItemFilter)), XmlElement("NameFilter", typeof(VirtualItemNameFilter)), XmlElement("PropertyFilter", typeof(VirtualPropertyFilter)), EditorBrowsable(EditorBrowsableState.Never), XmlElement("ContentFilter", typeof(VirtualItemContentFilter)), XmlElement("NameListFilter", typeof(VirtualItemNameListFilter)), XmlElement("HexContentFilter", typeof(VirtualItemHexContentFilter)), XmlElement("AttributeFilter", typeof(VirtualItemAttributeFilter)), XmlElement("SizeFilter", typeof(VirtualItemSizeFilter)), XmlElement("DateFilter", typeof(VirtualItemDateFilter)), XmlElement("TimeFilter", typeof(VirtualItemTimeFilter))]
        public BasicFilter SerializableFilter
        {
            get
            {
                return (BasicFilter) this.FFilter;
            }
            set
            {
                this.FFilter = (IVirtualItemFilter) value;
            }
        }
    }
}

