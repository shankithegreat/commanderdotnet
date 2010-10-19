namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Reflection;

    [DebuggerDisplay("Count = {Count}")]
    public class HashList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IStructuralEquatable
    {
        private const int Cutover = 9;
        private IEqualityComparer<T> HashComparer;
        private Dictionary<T, int> HashMap;
        private List<T> PlainList;

        public HashList()
        {
            this.PlainList = new List<T>();
        }

        public HashList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList = new List<T>(collection);
        }

        public HashList(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList = new List<T>();
            this.HashComparer = comparer;
        }

        public HashList(int capacity)
        {
            this.PlainList = new List<T>(capacity);
        }

        public HashList(int capacity, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList = new List<T>(capacity);
            this.HashComparer = comparer;
        }

        public void Add(T item)
        {
            if (this.HashMap != null)
            {
                this.HashMap.Add(item, this.PlainList.Count);
            }
            else if (item == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList.Add(item);
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return this.PlainList.AsReadOnly();
        }

        public void Clear()
        {
            this.PlainList.Clear();
            this.HashMap = null;
        }

        public bool Contains(T item)
        {
            if ((this.HashMap == null) && (this.PlainList.Count < 9))
            {
                return (this.SimpleIndexOf(item) >= 0);
            }
            this.HashMapNeeded();
            return this.HashMap.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.PlainList.CopyTo(array, arrayIndex);
        }

        public void Exchange(int index1, int index2)
        {
            T local = this.PlainList[index1];
            T local2 = this.PlainList[index2];
            this.PlainList[index1] = local2;
            this.PlainList[index2] = local;
            if (this.HashMap != null)
            {
                this.HashMap[local] = index2;
                this.HashMap[local2] = index1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.PlainList.GetEnumerator();
        }

        private void HashMapNeeded()
        {
            if (this.HashMap == null)
            {
                if (this.HashComparer != null)
                {
                    this.HashMap = new Dictionary<T, int>(this.PlainList.Capacity, this.HashComparer);
                }
                else
                {
                    this.HashMap = new Dictionary<T, int>(this.PlainList.Capacity);
                }
                for (int i = 0; i < this.PlainList.Count; i++)
                {
                    this.HashMap.Add(this.PlainList[i], i);
                }
            }
        }

        public int IndexOf(T item)
        {
            int num;
            if ((this.HashMap == null) && (this.PlainList.Count < 9))
            {
                return this.SimpleIndexOf(item);
            }
            this.HashMapNeeded();
            if (this.HashMap.TryGetValue(item, out num))
            {
                return num;
            }
            return -1;
        }

        public void Initialize()
        {
            if ((this.HashMap == null) && (this.PlainList.Count >= 9))
            {
                this.HashMapNeeded();
            }
        }

        public void Insert(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList.Insert(index, item);
            this.HashMap = null;
        }

        public bool Remove(T item)
        {
            if (this.HashMap != null)
            {
                int num;
                if (this.HashMap.TryGetValue(item, out num))
                {
                    this.RemoveAt(num);
                    return true;
                }
                return false;
            }
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            return this.PlainList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.PlainList.RemoveAt(index);
            this.HashMap = null;
        }

        protected int SimpleIndexOf(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            if (this.HashComparer == null)
            {
                return this.PlainList.IndexOf(item);
            }
            for (int i = this.PlainList.Count - 1; i >= 0; i--)
            {
                if (this.HashComparer.Equals(this.PlainList[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Sort()
        {
            this.PlainList.Sort();
            this.HashMap = null;
        }

        public void Sort(IComparer<T> comparer)
        {
            this.PlainList.Sort(comparer);
            this.HashMap = null;
        }

        public void Sort(Comparison<T> comparison)
        {
            this.PlainList.Sort(comparison);
            this.HashMap = null;
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            this.PlainList.Sort(index, count, comparer);
            this.HashMap = null;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.PlainList.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.PlainList.GetEnumerator();
        }

        int IList.Add(object value)
        {
            T key = (T) value;
            int count = this.PlainList.Count;
            if (this.HashMap != null)
            {
                this.HashMap.Add(key, count);
            }
            else if (key == null)
            {
                throw new ArgumentNullException();
            }
            this.PlainList.Add(key);
            return count;
        }

        bool IList.Contains(object value)
        {
            return this.Contains((T) value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T) value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T) value);
        }

        void IList.Remove(object value)
        {
            this.Remove((T) value);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            HashList<T> list = other as HashList<T>;
            if ((list == null) || (list.PlainList.Count != this.PlainList.Count))
            {
                return false;
            }
            for (int i = 0; i < this.PlainList.Count; i++)
            {
                if (!comparer.Equals(this.PlainList[i], list.PlainList[i]))
                {
                    return false;
                }
            }
            return true;
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

        public T[] ToArray()
        {
            return this.PlainList.ToArray();
        }

        public void TrimExcess(bool full)
        {
            this.PlainList.TrimExcess();
            if ((this.HashMap != null) && full)
            {
                this.HashMap = null;
            }
        }

        public int Count
        {
            get
            {
                return this.PlainList.Count;
            }
        }

        public bool IsReadOnly
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
                return this.PlainList[index];
            }
            set
            {
                this.PlainList[index] = value;
                if (this.HashMap != null)
                {
                    this.HashMap[value] = index;
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.PlainList[index];
            }
            set
            {
                this.PlainList[index] = (T) value;
            }
        }
    }
}

