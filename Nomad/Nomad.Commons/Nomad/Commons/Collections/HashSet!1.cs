namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private Dictionary<T, int> HashMap;

        public HashSet()
        {
            this.HashMap = new Dictionary<T, int>();
        }

        public HashSet(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }
            ICollection<T> is2 = collection as ICollection<T>;
            if (is2 != null)
            {
                this.HashMap = new Dictionary<T, int>(is2.Count);
            }
            else
            {
                this.HashMap = new Dictionary<T, int>();
            }
            foreach (T local in collection)
            {
                this.HashMap[local] = 0;
            }
        }

        public HashSet(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.HashMap = new Dictionary<T, int>(comparer);
        }

        public HashSet(int capacity)
        {
            this.HashMap = new Dictionary<T, int>(capacity);
        }

        public HashSet(int capacity, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.HashMap = new Dictionary<T, int>(capacity, comparer);
        }

        public bool Add(T item)
        {
            if (this.HashMap.ContainsKey(item))
            {
                return false;
            }
            this.HashMap.Add(item, 0);
            return true;
        }

        public void Clear()
        {
            this.HashMap.Clear();
        }

        public bool Contains(T item)
        {
            return this.HashMap.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.HashMap.Keys.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.HashMap.Keys.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return this.HashMap.Remove(item);
        }

        public int RemoveWhere(Predicate<T> match)
        {
            List<T> list = new List<T>();
            foreach (T local in this.HashMap.Keys)
            {
                if (match(local))
                {
                    list.Add(local);
                }
            }
            foreach (T local in list)
            {
                this.HashMap.Remove(local);
            }
            return list.Count;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            int num = 0;
            foreach (T local in other)
            {
                if (!this.HashMap.ContainsKey(local))
                {
                    return false;
                }
                num++;
            }
            return (num == this.HashMap.Count);
        }

        void ICollection<T>.Add(T item)
        {
            this.HashMap[item] = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.HashMap.Keys.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.HashMap.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}

