namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class SizeFilter : IntegralFilter<long>
    {
        private Nomad.Commons.SizeComparision FSizeComparision;
        [DefaultValue(typeof(Nomad.Commons.SizeUnit), "Byte")]
        public Nomad.Commons.SizeUnit SizeUnit;

        public SizeFilter()
        {
            this.FSizeComparision = Nomad.Commons.SizeComparision.Ignore;
            this.SizeUnit = Nomad.Commons.SizeUnit.Byte;
        }

        public SizeFilter(Nomad.Commons.SizeComparision comparision, long size)
        {
            this.FSizeComparision = Nomad.Commons.SizeComparision.Ignore;
            this.SizeUnit = Nomad.Commons.SizeUnit.Byte;
            if ((comparision == Nomad.Commons.SizeComparision.Between) || (comparision == Nomad.Commons.SizeComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.SizeComparision = comparision;
            base.FromValue = size;
        }

        public SizeFilter(Nomad.Commons.SizeComparision comparision, long fromSize, long toSize)
        {
            this.FSizeComparision = Nomad.Commons.SizeComparision.Ignore;
            this.SizeUnit = Nomad.Commons.SizeUnit.Byte;
            if ((comparision != Nomad.Commons.SizeComparision.Between) || (comparision != Nomad.Commons.SizeComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.SizeComparision = comparision;
            base.FromValue = fromSize;
            base.ToValue = toSize;
        }

        public SizeFilter(Nomad.Commons.SizeComparision comparision, long fromSize, long toSize, Nomad.Commons.SizeUnit unit) : this(comparision, fromSize, toSize)
        {
            this.SizeUnit = unit;
        }

        public override bool EqualTo(object obj)
        {
            SizeFilter filter = obj as SizeFilter;
            return ((((filter != null) && base.EqualTo(obj)) && (filter.SizeComparision == this.SizeComparision)) && (filter.SizeUnit == this.SizeUnit));
        }

        public override bool MatchValue(long value)
        {
            if (this.SizeComparision == Nomad.Commons.SizeComparision.Ignore)
            {
                return true;
            }
            long fromValue = this.SizeInBytes(base.FromValue);
            long toValue = this.SizeInBytes(base.ToValue);
            switch (this.SizeComparision)
            {
                case Nomad.Commons.SizeComparision.PercentOf25:
                case Nomad.Commons.SizeComparision.PercentOf50:
                {
                    long num3 = fromValue >> ((this.SizeComparision == 6) ? 2 : 1);
                    return ((value >= (fromValue - num3)) && (value <= (fromValue + num3)));
                }
            }
            return base.MatchValue(value, (SimpleComparision) this.SizeComparision, fromValue, toValue);
        }

        private long SizeInBytes(long size)
        {
            switch (this.SizeUnit)
            {
                case Nomad.Commons.SizeUnit.KiloByte:
                    return (size * 0x400L);

                case Nomad.Commons.SizeUnit.MegaByte:
                    return ((size * 0x400L) * 0x400L);
            }
            return size;
        }

        public Nomad.Commons.SizeComparision SizeComparision
        {
            get
            {
                return this.FSizeComparision;
            }
            set
            {
                this.FSizeComparision = value;
                switch (value)
                {
                    case Nomad.Commons.SizeComparision.PercentOf25:
                    case Nomad.Commons.SizeComparision.PercentOf50:
                        this.ValueComparision = SimpleComparision.Equals;
                        break;

                    default:
                        this.ValueComparision = (SimpleComparision) value;
                        break;
                }
            }
        }

        [XmlIgnore, EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SimpleComparision ValueComparision
        {
            get
            {
                return base.ValueComparision;
            }
            set
            {
                base.ValueComparision = value;
            }
        }
    }
}

