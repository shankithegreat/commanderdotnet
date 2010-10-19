namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
    {
        private IDictionary<TKey, TValue> Source;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> source)
        {
            this.Source = source;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.Source.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.Source.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.Source.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection) this.Source).CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary) this.Source).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary) this.Source).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Source.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.Source.TryGetValue(key, out value);
        }

        public int Count
        {
            get
            {
                return this.Source.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.Source[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.Source.Keys;
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
                return this;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary) this.Source)[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return ((IDictionary) this.Source).Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return ((IDictionary) this.Source).Values;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this.Source.Values;
            }
        }
    }
}

