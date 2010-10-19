namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Xml.Serialization;

    [Serializable]
    public class DateFilter : ValueFilter
    {
        public Nomad.Commons.DateComparision DateComparision;
        [DefaultValue(typeof(DateUnit), "Day")]
        public DateUnit DateMeasure;
        private DateTime FFromDate;
        private DateTime FToDate;
        [DefaultValue(1)]
        public int NotOlderThan;

        public DateFilter()
        {
            this.DateComparision = Nomad.Commons.DateComparision.Ignore;
            this.NotOlderThan = 1;
            this.DateMeasure = DateUnit.Day;
        }

        public DateFilter(Nomad.Commons.DateComparision comparision, DateTime date)
        {
            this.DateComparision = Nomad.Commons.DateComparision.Ignore;
            this.NotOlderThan = 1;
            this.DateMeasure = DateUnit.Day;
            if ((comparision < Nomad.Commons.DateComparision.On) || (comparision > Nomad.Commons.DateComparision.After))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.DateComparision = comparision;
            this.FromDate = date;
        }

        public DateFilter(int notOlderThan, DateUnit measure)
        {
            this.DateComparision = Nomad.Commons.DateComparision.Ignore;
            this.NotOlderThan = 1;
            this.DateMeasure = DateUnit.Day;
            this.NotOlderThan = notOlderThan;
            this.DateMeasure = measure;
            this.DateComparision = Nomad.Commons.DateComparision.NotOlderThan;
        }

        public DateFilter(Nomad.Commons.DateComparision comparision, DateTime fromDate, DateTime toDate)
        {
            this.DateComparision = Nomad.Commons.DateComparision.Ignore;
            this.NotOlderThan = 1;
            this.DateMeasure = DateUnit.Day;
            if ((comparision != Nomad.Commons.DateComparision.Between) && (comparision != Nomad.Commons.DateComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.DateComparision = comparision;
            this.FromDate = fromDate;
            this.ToDate = toDate;
        }

        public override bool EqualTo(object obj)
        {
            DateFilter filter = obj as DateFilter;
            bool flag = (filter != null) && (filter.DateComparision == this.DateComparision);
            if (!flag)
            {
                return flag;
            }
            switch (this.DateComparision)
            {
                case Nomad.Commons.DateComparision.Between:
                case Nomad.Commons.DateComparision.NotBetween:
                    return ((filter.FromDate == this.FromDate) && (filter.ToDate == this.ToDate));

                case Nomad.Commons.DateComparision.NotOlderThan:
                    return ((filter.NotOlderThan == this.NotOlderThan) && (filter.DateMeasure == this.DateMeasure));
            }
            return (filter.FromDate == this.FromDate);
        }

        public bool MatchDate(DateTime Value)
        {
            Value = new DateTime(Value.Year, Value.Month, Value.Day);
            switch (this.DateComparision)
            {
                case Nomad.Commons.DateComparision.On:
                    return (Value == this.FromDate);

                case Nomad.Commons.DateComparision.Before:
                    return (Value < this.FromDate);

                case Nomad.Commons.DateComparision.After:
                    return (Value > this.FromDate);

                case Nomad.Commons.DateComparision.Between:
                    return ((Value >= this.FromDate) && (Value <= this.ToDate));

                case Nomad.Commons.DateComparision.NotBetween:
                    return ((Value < this.FromDate) || (Value > this.ToDate));

                case Nomad.Commons.DateComparision.NotOlderThan:
                {
                    DateTime date = DateTime.Now.Date;
                    switch (this.DateMeasure)
                    {
                        case DateUnit.Week:
                            return ((Value >= date.AddDays((double) (-7 * this.NotOlderThan))) && (Value <= DateTime.Now));

                        case DateUnit.Month:
                            return ((Value >= date.AddMonths(-this.NotOlderThan)) && (Value <= DateTime.Now));

                        case DateUnit.Year:
                            return ((Value >= date.AddYears(-this.NotOlderThan)) && (Value <= DateTime.Now));
                    }
                    return ((Value >= date.AddDays((double) -this.NotOlderThan)) && (Value <= DateTime.Now));
                }
            }
            return true;
        }

        public override bool MatchValue(object value)
        {
            return ((this.DateComparision == Nomad.Commons.DateComparision.Ignore) || ((value is DateTime) && this.MatchDate((DateTime) value)));
        }

        public override string ToString()
        {
            switch (this.DateComparision)
            {
                case Nomad.Commons.DateComparision.On:
                    return string.Format(CultureInfo.InvariantCulture, "{0} = {1:d}", new object[] { this.FieldName, this.FromDate });

                case Nomad.Commons.DateComparision.Before:
                    return string.Format(CultureInfo.InvariantCulture, "{0} < {1:d}", new object[] { this.FieldName, this.FromDate });

                case Nomad.Commons.DateComparision.After:
                    return string.Format(CultureInfo.InvariantCulture, "{0} > {1:d}", new object[] { this.FieldName, this.FromDate });

                case Nomad.Commons.DateComparision.Between:
                    return string.Format(CultureInfo.InvariantCulture, "{0} between {1:d} and {2:d}", new object[] { this.FieldName, this.FromDate, this.ToDate });

                case Nomad.Commons.DateComparision.NotBetween:
                    return string.Format(CultureInfo.InvariantCulture, "{0} not between {1:d} and {2:d}", new object[] { this.FieldName, this.FromDate, this.ToDate });

                case Nomad.Commons.DateComparision.NotOlderThan:
                    return string.Format(CultureInfo.InvariantCulture, "{0} not older than {1} {2}", new object[] { this.FieldName, this.NotOlderThan, this.DateMeasure });
            }
            return base.ToString();
        }

        protected virtual string FieldName
        {
            get
            {
                return "Date";
            }
        }

        [XmlElement(DataType="date")]
        public DateTime FromDate
        {
            get
            {
                return this.FFromDate;
            }
            set
            {
                this.FFromDate = new DateTime(value.Year, value.Month, value.Day);
            }
        }

        [XmlElement(DataType="date")]
        public DateTime ToDate
        {
            get
            {
                return this.FToDate;
            }
            set
            {
                this.FToDate = new DateTime(value.Year, value.Month, value.Day);
            }
        }
    }
}

