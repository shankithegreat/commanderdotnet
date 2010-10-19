namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SimpleFilter<T> : ValueFilter
    {
        private SimpleComparision FValueComparision;

        public SimpleFilter()
        {
            this.FValueComparision = SimpleComparision.Ignore;
        }

        public SimpleFilter(SimpleComparision comparision, T value)
        {
            this.FValueComparision = SimpleComparision.Ignore;
            if ((comparision == SimpleComparision.Between) || (comparision == SimpleComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.ValueComparision = comparision;
            this.FromValue = value;
        }

        public SimpleFilter(SimpleComparision comparision, T fromValue, T toValue)
        {
            this.FValueComparision = SimpleComparision.Ignore;
            if ((comparision != SimpleComparision.Between) || (comparision != SimpleComparision.NotBetween))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.ValueComparision = comparision;
            this.FromValue = fromValue;
            this.ToValue = toValue;
        }

        public override bool EqualTo(object obj)
        {
            SimpleFilter<T> filter = obj as SimpleFilter<T>;
            if (filter == null)
            {
                return false;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            bool flag = (filter.ValueComparision == this.ValueComparision) && comparer.Equals(filter.FromValue, this.FromValue);
            if (flag && ((this.ValueComparision == SimpleComparision.Between) || (this.ValueComparision == SimpleComparision.NotBetween)))
            {
                flag = comparer.Equals(filter.ToValue, this.ToValue);
            }
            return flag;
        }

        public virtual bool MatchValue(T value)
        {
            return this.MatchValue(value, this.ValueComparision, this.FromValue, this.ToValue);
        }

        public override bool MatchValue(object value)
        {
            return ((this.ValueComparision == SimpleComparision.Ignore) || ((value is T) && this.MatchValue((T) value)));
        }

        protected bool MatchValue(T value, SimpleComparision comparision, T fromValue, T toValue)
        {
            Comparer<T> comparer = Comparer<T>.Default;
            switch (comparision)
            {
                case SimpleComparision.Equals:
                    return (comparer.Compare(value, fromValue) == 0);

                case SimpleComparision.Smaller:
                    return (comparer.Compare(value, fromValue) < 0);

                case SimpleComparision.Larger:
                    return (comparer.Compare(value, fromValue) > 0);

                case SimpleComparision.Between:
                    return ((comparer.Compare(value, fromValue) >= 0) && (comparer.Compare(value, toValue) <= 0));

                case SimpleComparision.NotBetween:
                    return ((comparer.Compare(value, fromValue) < 0) || (comparer.Compare(value, toValue) > 0));

                case SimpleComparision.NotEquals:
                    return (comparer.Compare(value, fromValue) != 0);
            }
            return true;
        }

        public T FromValue
        {
            [CompilerGenerated]
            get
            {
                return this.<FromValue>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FromValue>k__BackingField = value;
            }
        }

        public T ToValue
        {
            [CompilerGenerated]
            get
            {
                return this.<ToValue>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ToValue>k__BackingField = value;
            }
        }

        public SimpleComparision ValueComparision
        {
            get
            {
                return this.FValueComparision;
            }
            set
            {
                this.FValueComparision = value;
            }
        }
    }
}

