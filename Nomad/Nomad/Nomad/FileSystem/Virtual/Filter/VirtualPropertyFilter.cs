namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class VirtualPropertyFilter : BasicFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>
    {
        private BasicFilter FFilter;
        private int FPropertyId;

        public VirtualPropertyFilter()
        {
        }

        public VirtualPropertyFilter(int propertyId, BasicFilter filter)
        {
            this.PropertyId = propertyId;
            this.Filter = filter;
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public override bool EqualTo(object obj)
        {
            VirtualPropertyFilter filter = obj as VirtualPropertyFilter;
            return ((((filter != null) && (this.PropertyId == filter.PropertyId)) && (this.Filter != null)) && this.Filter.EqualTo(filter.Filter));
        }

        private static Type GetFilterForType(Type propertyType)
        {
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Byte:
                    return typeof(IntegralFilter<byte>);

                case TypeCode.Int16:
                    return typeof(IntegralFilter<short>);

                case TypeCode.UInt16:
                    return typeof(IntegralFilter<ushort>);

                case TypeCode.Int32:
                    return typeof(IntegralFilter<int>);

                case TypeCode.UInt32:
                    return typeof(IntegralFilter<uint>);

                case TypeCode.Int64:
                    return typeof(IntegralFilter<long>);

                case TypeCode.DateTime:
                    return typeof(DateFilter);

                case TypeCode.String:
                    return typeof(StringFilter);
            }
            if (propertyType != typeof(Version))
            {
                throw new ArgumentException("No filter for this property type.");
            }
            return typeof(SimpleFilter<Version>);
        }

        public bool IsMatch(IVirtualItem item)
        {
            ValueFilter filter = this.Filter as ValueFilter;
            return ((filter != null) && filter.MatchValue(item[this.PropertyId]));
        }

        [XmlElement("DateFilter", typeof(DateFilter)), XmlElement("StringFilter", typeof(StringFilter)), XmlElement("VersionFilter", typeof(SimpleFilter<Version>)), XmlElement("Int64Filter", typeof(IntegralFilter<long>)), XmlElement("UInt32Filter", typeof(IntegralFilter<uint>)), XmlElement("Int32Filter", typeof(IntegralFilter<int>)), XmlElement("ByteFilter", typeof(IntegralFilter<byte>))]
        public BasicFilter Filter
        {
            get
            {
                return this.FFilter;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (this.FFilter != value)
                {
                    Type filterForType = GetFilterForType(VirtualProperty.Get(this.PropertyId).PropertyType);
                    if (value.GetType() != filterForType)
                    {
                        throw new ArgumentException("Invalid filter value for this property.");
                    }
                    this.FFilter = value;
                }
            }
        }

        public int PropertyId
        {
            get
            {
                return this.FPropertyId;
            }
            set
            {
                if (this.FPropertyId != value)
                {
                    VirtualProperty property = VirtualProperty.Get(value);
                    if (property == null)
                    {
                        throw new ArgumentException("Unknown property");
                    }
                    this.FPropertyId = value;
                    Type filterForType = GetFilterForType(property.PropertyType);
                    if ((this.FFilter == null) || (this.FFilter.GetType() != filterForType))
                    {
                        this.FFilter = (BasicFilter) Activator.CreateInstance(filterForType);
                    }
                }
            }
        }
    }
}

