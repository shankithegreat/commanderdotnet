namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public class AggregatedVirtualItemFilter : BasicFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter, IChangeProgress
    {
        private bool CancellationPending;
        public AggregatedFilterCondition Condition;
        private List<IVirtualItemFilter> FFilters;

        private event EventHandler<ProgressEventArgs> FProgress;

        public event EventHandler<ProgressEventArgs> Progress
        {
            add
            {
                if (this.FProgress == null)
                {
                    this.AssignContainedFilterProgress(this.Filters, true);
                }
                this.FProgress = (EventHandler<ProgressEventArgs>) Delegate.Combine(this.FProgress, value);
            }
            remove
            {
                this.FProgress = (EventHandler<ProgressEventArgs>) Delegate.Remove(this.FProgress, value);
                if (this.FProgress == null)
                {
                    this.AssignContainedFilterProgress(this.Filters, false);
                }
            }
        }

        public AggregatedVirtualItemFilter()
        {
        }

        public AggregatedVirtualItemFilter(IEnumerable<IVirtualItemFilter> filters) : this(AggregatedFilterCondition.All, filters)
        {
        }

        public AggregatedVirtualItemFilter(params IVirtualItemFilter[] filters) : this(AggregatedFilterCondition.All, filters)
        {
        }

        public AggregatedVirtualItemFilter(AggregatedFilterCondition condition, IEnumerable<IVirtualItemFilter> filters)
        {
            this.Condition = condition;
            this.FFilters = new List<IVirtualItemFilter>(filters);
            this.Filters.Sort(new Comparison<IVirtualItemFilter>(this.CompareContentFilters));
        }

        public AggregatedVirtualItemFilter(AggregatedFilterCondition condition, params IVirtualItemFilter[] filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            this.Condition = condition;
            this.FFilters = new List<IVirtualItemFilter>(filters);
            this.Filters.Sort(new Comparison<IVirtualItemFilter>(this.CompareContentFilters));
        }

        public AggregatedVirtualItemFilter(IVirtualItemFilter filter1, IVirtualItemFilter filter2) : this(AggregatedFilterCondition.All, filter1, filter2)
        {
        }

        public AggregatedVirtualItemFilter(AggregatedFilterCondition condition, IVirtualItemFilter filter1, IVirtualItemFilter filter2)
        {
            if (filter1 == null)
            {
                throw new ArgumentNullException("filter1");
            }
            if (filter2 == null)
            {
                throw new ArgumentNullException("filter2");
            }
            this.Condition = condition;
            this.FFilters = new List<IVirtualItemFilter>(2);
            this.FFilters.Add(filter1);
            this.FFilters.Add(filter2);
            this.Filters.Sort(new Comparison<IVirtualItemFilter>(this.CompareContentFilters));
        }

        private void AssignContainedFilterProgress(IEnumerable<IVirtualItemFilter> filters, bool add)
        {
            foreach (IVirtualItemFilter filter in filters)
            {
                IChangeProgress progress = filter as IChangeProgress;
                if (progress != null)
                {
                    if (add)
                    {
                        progress.Progress += new EventHandler<ProgressEventArgs>(this.ContainedFilterProgress);
                    }
                    else
                    {
                        progress.Progress -= new EventHandler<ProgressEventArgs>(this.ContainedFilterProgress);
                    }
                }
                AggregatedVirtualItemFilter filter2 = filter as AggregatedVirtualItemFilter;
                if (filter2 != null)
                {
                    this.AssignContainedFilterProgress(filter2.Filters, add);
                }
            }
        }

        private int CompareContentFilters(IVirtualItemFilter x, IVirtualItemFilter y)
        {
            bool flag = x is CustomContentFilter;
            bool flag2 = y is CustomContentFilter;
            if (!(!flag || flag2))
            {
                return 1;
            }
            if (!(flag || !flag2))
            {
                return -1;
            }
            return 0;
        }

        private void ContainedFilterProgress(object sender, ProgressEventArgs e)
        {
            if (!this.CancellationPending)
            {
                this.CancellationPending = !this.OnProgress(0);
            }
            e.Cancel = this.CancellationPending;
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public override bool EqualTo(object other)
        {
            AggregatedVirtualItemFilter filter = other as AggregatedVirtualItemFilter;
            bool flag = ((filter != null) && (this.Condition == filter.Condition)) && (filter.Filters.Count == this.Filters.Count);
            if (flag)
            {
                List<IVirtualItemFilter> list = new List<IVirtualItemFilter>(filter.Filters);
                foreach (IVirtualItemFilter filter2 in this.Filters)
                {
                    flag = false;
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (filter2.Equals(list[i]))
                        {
                            list.RemoveAt(i);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        return flag;
                    }
                }
            }
            return flag;
        }

        public bool IsMatch(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            bool flag = this.Condition == AggregatedFilterCondition.All;
            foreach (IVirtualItemFilter filter in this.Filters)
            {
                if (!(!this.CancellationPending && this.OnProgress(0)))
                {
                    return false;
                }
                bool flag2 = filter.IsMatch(item);
                if (flag != flag2)
                {
                    flag = flag2;
                    break;
                }
            }
            return (((this.Condition == AggregatedFilterCondition.None) && !flag) || ((this.Condition != AggregatedFilterCondition.None) && flag));
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            bool flag = this.Condition == AggregatedFilterCondition.All;
            foreach (IVirtualItemFilter filter in this.Filters)
            {
                if (!(!this.CancellationPending && this.OnProgress(0)))
                {
                    return false;
                }
                IFileSystemInfoFilter filter2 = filter as IFileSystemInfoFilter;
                bool flag2 = (filter2 != null) && filter2.IsMatch(item);
                if (flag != flag2)
                {
                    flag = flag2;
                    break;
                }
            }
            return (((this.Condition == AggregatedFilterCondition.None) && !flag) || ((this.Condition != AggregatedFilterCondition.None) && flag));
        }

        protected bool OnProgress(int progressPercentage)
        {
            if (this.FProgress != null)
            {
                ProgressEventArgs e = new ProgressEventArgs(progressPercentage);
                this.FProgress(this, e);
                return !e.Cancel;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (this.Condition == AggregatedFilterCondition.None)
            {
                builder.Append("not ");
            }
            builder.Append('(');
            bool flag = false;
            foreach (IVirtualItemFilter filter in this.Filters)
            {
                if (flag)
                {
                    if (this.Condition == AggregatedFilterCondition.All)
                    {
                        builder.Append(" and ");
                    }
                    else
                    {
                        builder.Append(" or ");
                    }
                }
                builder.Append('(');
                builder.Append(filter);
                builder.Append(')');
                flag = true;
            }
            builder.Append(')');
            return builder.ToString();
        }

        [XmlIgnore]
        public List<IVirtualItemFilter> Filters
        {
            get
            {
                return this.FFilters;
            }
        }

        [XmlArrayItem("PropertyFilter", typeof(VirtualPropertyFilter)), XmlArrayItem("AggregatedFilter", typeof(AggregatedVirtualItemFilter)), XmlArrayItem("ContentFilter", typeof(VirtualItemContentFilter)), XmlArrayItem("TimeFilter", typeof(VirtualItemTimeFilter)), EditorBrowsable(EditorBrowsableState.Never), XmlArrayItem("AttributeFilter", typeof(VirtualItemAttributeFilter)), XmlArrayItem("SizeFilter", typeof(VirtualItemSizeFilter)), XmlArrayItem("DateFilter", typeof(VirtualItemDateFilter)), XmlArrayItem("NameFilter", typeof(VirtualItemNameFilter)), XmlArray("Filters"), XmlArrayItem("HexContentFilter", typeof(VirtualItemHexContentFilter)), XmlArrayItem("NameListFilter", typeof(VirtualItemNameListFilter))]
        public BasicFilter[] SerializableFilters
        {
            get
            {
                BasicFilter[] filterArray = new BasicFilter[this.Filters.Count];
                for (int i = 0; i < this.Filters.Count; i++)
                {
                    filterArray[i] = (BasicFilter) this.Filters[i];
                }
                return filterArray;
            }
            set
            {
                if (this.Filters != null)
                {
                    this.Filters.Clear();
                }
                else
                {
                    this.FFilters = new List<IVirtualItemFilter>(value.Length);
                }
                foreach (BasicFilter filter in value)
                {
                    this.Filters.Add((IVirtualItemFilter) filter);
                }
                this.Filters.Sort(new Comparison<IVirtualItemFilter>(this.CompareContentFilters));
            }
        }
    }
}

