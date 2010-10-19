namespace TagLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class ListBase<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T> where T: IComparable<T>
    {
        private List<T> data;

        public ListBase()
        {
            this.data = new List<T>();
        }

        public ListBase(ListBase<T> list)
        {
            this.data = new List<T>();
            if (list != null)
            {
                this.Add(list);
            }
        }

        public ListBase(params T[] list)
        {
            this.data = new List<T>();
            if (list != null)
            {
                this.Add(list);
            }
        }

        public void Add(T item)
        {
            this.data.Add(item);
        }

        public void Add(ListBase<T> list)
        {
            if (list != null)
            {
                this.data.AddRange(list);
            }
        }

        public void Add(IEnumerable<T> list)
        {
            if (list != null)
            {
                this.data.AddRange(list);
            }
        }

        public void Add(T[] list)
        {
            if (list != null)
            {
                this.data.AddRange(list);
            }
        }

        public void Clear()
        {
            this.data.Clear();
        }

        public bool Contains(T item)
        {
            return this.data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.data.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.data.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.data.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.data.RemoveAt(index);
        }

        public void SortedInsert(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            this.SortedInsert(item, false);
        }

        public virtual void SortedInsert(T item, bool unique)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = 0;
            while (index < this.data.Count)
            {
                if ((item.CompareTo(this.data[index]) == 0) && unique)
                {
                    return;
                }
                if (item.CompareTo(this.data[index]) <= 0)
                {
                    break;
                }
                index++;
            }
            this.Insert(index, item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public T[] ToArray()
        {
            return this.data.ToArray();
        }

        public override string ToString()
        {
            return this.ToString(", ");
        }

        public string ToString(string separator)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                if (i != 0)
                {
                    builder.Append(separator);
                }
                builder.Append(this[i].ToString());
            }
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.Count == 0);
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.data[index];
            }
            set
            {
                this.data[index] = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

