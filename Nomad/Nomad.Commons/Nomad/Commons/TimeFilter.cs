namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class TimeFilter : ValueFilter
    {
        private TimeSpan FFromTime;
        private TimeSpan FToTime;
        public Nomad.Commons.TimeComparision TimeComparision;

        public TimeFilter()
        {
            this.TimeComparision = Nomad.Commons.TimeComparision.Ignore;
        }

        public TimeFilter(Nomad.Commons.TimeComparision comparision, TimeSpan time)
        {
            this.TimeComparision = Nomad.Commons.TimeComparision.Ignore;
            if ((comparision == Nomad.Commons.TimeComparision.Between) || (comparision == Nomad.Commons.TimeComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.TimeComparision = comparision;
            this.FromTime = time;
        }

        public TimeFilter(Nomad.Commons.TimeComparision comparision, TimeSpan fromTime, TimeSpan toTime)
        {
            this.TimeComparision = Nomad.Commons.TimeComparision.Ignore;
            if ((comparision != Nomad.Commons.TimeComparision.Between) && (comparision != Nomad.Commons.TimeComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.TimeComparision = comparision;
            this.FromTime = fromTime;
            this.ToTime = toTime;
        }

        public override bool EqualTo(object obj)
        {
            TimeFilter filter = obj as TimeFilter;
            bool flag = ((filter != null) && (filter.TimeComparision == this.TimeComparision)) && (filter.FromTime == this.FromTime);
            if (flag && ((this.TimeComparision == Nomad.Commons.TimeComparision.Between) || (this.TimeComparision == Nomad.Commons.TimeComparision.NotBetween)))
            {
                flag = filter.ToTime == this.ToTime;
            }
            return flag;
        }

        public bool MatchTime(TimeSpan value)
        {
            switch (this.TimeComparision)
            {
                case Nomad.Commons.TimeComparision.At:
                    return ((value >= this.FromTime) && (value <= this.FromTime));

                case Nomad.Commons.TimeComparision.Before:
                    return (value < this.FromTime);

                case Nomad.Commons.TimeComparision.After:
                    return (value > this.FromTime);

                case Nomad.Commons.TimeComparision.Between:
                    return ((value >= this.FromTime) && (value <= this.ToTime));

                case Nomad.Commons.TimeComparision.NotBetween:
                    return ((value < this.FromTime) || (value > this.ToTime));

                case Nomad.Commons.TimeComparision.HoursOf1:
                    return ((value >= this.FromTime.Add(new TimeSpan(1, 0, 0))) && (value <= this.FromTime.Add(new TimeSpan(-1, 0, 0))));

                case Nomad.Commons.TimeComparision.HoursOf6:
                    return ((value >= this.FromTime.Add(new TimeSpan(6, 0, 0))) && (value <= this.FromTime.Add(new TimeSpan(-6, 0, 0))));
            }
            return true;
        }

        public override bool MatchValue(object value)
        {
            return ((this.TimeComparision == Nomad.Commons.TimeComparision.Ignore) || ((value is TimeSpan) && this.MatchTime((TimeSpan) value)));
        }

        [XmlIgnore]
        public TimeSpan FromTime
        {
            get
            {
                return this.FFromTime;
            }
            set
            {
                this.FFromTime = new TimeSpan(value.Hours, value.Minutes, 0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), XmlElement("FromTime", DataType="time")]
        public DateTime SerializableFromTime
        {
            get
            {
                return new DateTime(this.FromTime.Ticks);
            }
            set
            {
                this.FromTime = new TimeSpan(value.Hour, value.Minute, 0);
            }
        }

        [XmlElement("ToTime", DataType="time"), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SerializableToTime
        {
            get
            {
                return new DateTime(this.ToTime.Ticks);
            }
            set
            {
                this.ToTime = new TimeSpan(value.Hour, value.Minute, 0);
            }
        }

        [XmlIgnore]
        public TimeSpan ToTime
        {
            get
            {
                return this.FToTime;
            }
            set
            {
                this.FToTime = new TimeSpan(value.Hours, value.Minutes, 0);
            }
        }
    }
}

