namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad.Commons;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    public class VirtualItemNameListFilter : BasicFilter, IVirtualItemFilter, IEquatable<IVirtualItemFilter>, IFileSystemInfoFilter, ISerializable
    {
        public NameListCondition Condition;
        private const string EntryCondition = "Condition";
        private const string EntryNames = "Names";
        private Dictionary<string, int> FNames;

        public VirtualItemNameListFilter()
        {
            this.Condition = NameListCondition.InList;
        }

        public VirtualItemNameListFilter(IEnumerable<string> names)
        {
            this.Condition = NameListCondition.InList;
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            if (names is ICollection)
            {
                this.FNames = new Dictionary<string, int>(((ICollection) names).Count, StringComparer.InvariantCultureIgnoreCase);
            }
            else
            {
                this.FNames = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            }
            foreach (string str in names)
            {
                this.FNames.Add(str, 0);
            }
        }

        public VirtualItemNameListFilter(string[] names)
        {
            this.Condition = NameListCondition.InList;
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            this.FNames = new Dictionary<string, int>(names.Length, StringComparer.InvariantCultureIgnoreCase);
            foreach (string str in names)
            {
                this.FNames.Add(str, 0);
            }
        }

        public VirtualItemNameListFilter(NameListCondition condition, IEnumerable<string> names) : this(names)
        {
            this.Condition = condition;
        }

        public VirtualItemNameListFilter(NameListCondition condition, string[] names) : this(names)
        {
            this.Condition = condition;
        }

        protected VirtualItemNameListFilter(SerializationInfo info, StreamingContext context) : this((NameListCondition) info.GetInt32("Condition"), (string[]) info.GetValue("Names", typeof(string[])))
        {
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return this.EqualTo(other);
        }

        public override bool EqualTo(object obj)
        {
            VirtualItemNameListFilter filter = obj as VirtualItemNameListFilter;
            bool flag = (filter != null) && (filter.FNames.Count == this.FNames.Count);
            if (flag)
            {
                foreach (string str in this.FNames.Keys)
                {
                    flag = filter.FNames.ContainsKey(str);
                    if (!flag)
                    {
                        return flag;
                    }
                }
            }
            return flag;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Condition", (int) this.Condition);
            info.AddValue("Names", this.Names);
        }

        public bool IsMatch(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            if (this.Condition == NameListCondition.InList)
            {
                return this.FNames.ContainsKey(item.Name);
            }
            return !this.FNames.ContainsKey(item.Name);
        }

        public bool IsMatch(FileSystemInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            if (this.Condition == NameListCondition.InList)
            {
                return this.FNames.ContainsKey(item.Name);
            }
            return !this.FNames.ContainsKey(item.Name);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Name");
            if (this.Condition == NameListCondition.NotInList)
            {
                builder.Append(" not");
            }
            builder.Append(" in (");
            bool flag = false;
            foreach (string str in this.FNames.Keys)
            {
                if (flag)
                {
                    builder.Append(", ");
                }
                builder.Append("'");
                builder.Append(str);
                builder.Append("'");
                flag = true;
            }
            builder.Append(')');
            return builder.ToString();
        }

        public string[] Names
        {
            get
            {
                string[] array = new string[this.FNames.Keys.Count];
                this.FNames.Keys.CopyTo(array, 0);
                return array;
            }
            set
            {
                this.FNames.Clear();
                foreach (string str in value)
                {
                    this.FNames.Add(str, 0);
                }
            }
        }
    }
}

