namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [XmlRoot("KeyValueList")]
    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IStructuralEquatable, IXmlSerializable
    {
        private IEqualityComparer<TKey> FEqualityComparer;
        private KeyCollection<TKey, TValue> FKeys;
        private ValueCollection<TKey, TValue> FValues;

        public KeyValueList()
        {
            this.FEqualityComparer = EqualityComparer<TKey>.Default;
        }

        public KeyValueList(IEqualityComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.FEqualityComparer = comparer;
        }

        public KeyValueList(int capacity) : base(capacity)
        {
            this.FEqualityComparer = EqualityComparer<TKey>.Default;
        }

        public KeyValueList(int capacity, IEqualityComparer<TKey> comparer) : base(capacity)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            this.FEqualityComparer = comparer;
        }

        public void Add(KeyValuePair<TKey, TValue> value)
        {
            if (this.ContainsKey(value.Key))
            {
                throw new ArgumentException();
            }
            base.Add(value);
        }

        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                throw new ArgumentException();
            }
            this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> value)
        {
            ICollection<KeyValueList<TKey, TValue>> is2 = value as ICollection<KeyValueList<TKey, TValue>>;
            if ((is2 != null) && (base.Capacity < (base.Count + is2.Count)))
            {
                base.Capacity = base.Count + is2.Count;
            }
            foreach (KeyValuePair<TKey, TValue> pair in value)
            {
                this.Add(pair);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return (base.FindIndex(delegate (KeyValuePair<TKey, TValue> pair) {
                return ((KeyValueList<TKey, TValue>) this).KeysEqual(pair.Key, key);
            }) >= 0);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void Insert(int index, KeyValuePair<TKey, TValue> value)
        {
            if (this.ContainsKey(value.Key))
            {
                throw new ArgumentException();
            }
            base.Insert(index, value);
        }

        public void InsertRange(int index, IEnumerable<KeyValuePair<TKey, TValue>> value)
        {
            throw new NotSupportedException();
        }

        private bool KeysEqual(TKey x, TKey y)
        {
            if (x is IEquatable<TKey>)
            {
                return ((IEquatable<TKey>) x).Equals(y);
            }
            return this.FEqualityComparer.Equals(x, y);
        }

        public void ReadXml(XmlReader reader)
        {
            this.DeserializeData<TKey, TValue>(reader);
        }

        public bool Remove(TKey key)
        {
            return (base.RemoveAll(delegate (KeyValuePair<TKey, TValue> pair) {
                return ((KeyValueList<TKey, TValue>) this).KeysEqual(pair.Key, key);
            }) > 0);
        }

        public void Sort()
        {
            this.Sort(Comparer<TKey>.Default);
        }

        public void Sort(IComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            base.Sort(delegate (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) {
                return comparer.Compare(x.Key, y.Key);
            });
        }

        public void Sort(Comparison<TKey> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException();
            }
            base.Sort(delegate (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) {
                return comparison(x.Key, x.Key);
            });
        }

        public void Sort(int index, int count, IComparer<KeyValuePair<TKey, TValue>> comparer)
        {
            throw new NotSupportedException();
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            KeyValueList<TKey, TValue> list = other as KeyValueList<TKey, TValue>;
            if ((list == null) || (list.Count != base.Count))
            {
                return false;
            }
            for (int i = 0; i < base.Count; i++)
            {
                KeyValuePair<TKey, TValue> pair = base[i];
                KeyValuePair<TKey, TValue> pair2 = list[i];
                if (!(comparer.Equals(pair.Key, pair2.Key) && comparer.Equals(pair.Value, pair2.Value)))
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

        public bool TryGetValue(TKey key, out TValue value)
        {
            int num = base.FindIndex(delegate (KeyValuePair<TKey, TValue> pair) {
                return ((KeyValueList<TKey, TValue>) this).KeysEqual(pair.Key, key);
            });
            if (num >= 0)
            {
                KeyValuePair<TKey, TValue> pair = base[num];
                value = pair.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public void WriteXml(XmlWriter writer)
        {
            this.SerializeData<TKey, TValue>(writer);
        }

        public TValue this[TKey key]
        {
            get
            {
                int num = base.FindIndex(delegate (KeyValuePair<TKey, TValue> pair) {
                    return ((KeyValueList<TKey, TValue>) this).KeysEqual(pair.Key, key);
                });
                if (num < 0)
                {
                    throw new ArgumentException();
                }
                KeyValuePair<TKey, TValue> pair = base[num];
                return pair.Value;
            }
            set
            {
                int num = base.FindIndex(delegate (KeyValuePair<TKey, TValue> pair) {
                    return ((KeyValueList<TKey, TValue>) this).KeysEqual(pair.Key, key);
                });
                if (num < 0)
                {
                    base.Add(new KeyValuePair<TKey, TValue>(key, value));
                }
                else
                {
                    base[num] = new KeyValuePair<TKey, TValue>(key, value);
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return (this.FKeys ?? (this.FKeys = new KeyCollection<TKey, TValue>((KeyValueList<TKey, TValue>) this)));
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return (this.FValues ?? (this.FValues = new ValueCollection<TKey, TValue>((KeyValueList<TKey, TValue>) this)));
            }
        }

        public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable
        {
            private KeyValueList<TKey, TValue> FOwner;

            public KeyCollection(KeyValueList<TKey, TValue> owner)
            {
                this.FOwner = owner;
            }

            public void Add(TKey item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(TKey item)
            {
                return this.FOwner.ContainsKey(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException();
                }
                foreach (TKey local in this)
                {
                    array[arrayIndex++] = local;
                }
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return new <GetEnumerator>d__15<TKey, TValue>(0) { <>4__this = this };
            }

            public bool Remove(TKey item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this.FOwner.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            [CompilerGenerated]
            private sealed class <GetEnumerator>d__15 : IEnumerator<TKey>, IEnumerator, IDisposable
            {
                private int <>1__state;
                private TKey <>2__current;
                public KeyValueList<TKey, TValue>.KeyCollection <>4__this;
                public List<KeyValuePair<TKey, TValue>>.Enumerator <>7__wrap17;
                public KeyValuePair<TKey, TValue> <NextPair>5__16;

                [DebuggerHidden]
                public <GetEnumerator>d__15(int <>1__state)
                {
                    this.<>1__state = <>1__state;
                }

                private void <>m__Finally18()
                {
                    this.<>1__state = -1;
                    this.<>7__wrap17.Dispose();
                }

                private bool MoveNext()
                {
                    try
                    {
                        switch (this.<>1__state)
                        {
                            case 0:
                                this.<>1__state = -1;
                                this.<>7__wrap17 = this.<>4__this.FOwner.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap17.MoveNext())
                                {
                                    this.<NextPair>5__16 = this.<>7__wrap17.Current;
                                    this.<>2__current = this.<NextPair>5__16.Key;
                                    this.<>1__state = 2;
                                    return true;
                                Label_0079:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally18();
                                break;

                            case 2:
                                goto Label_0079;
                        }
                        return false;
                    }
                    fault
                    {
                        this.System.IDisposable.Dispose();
                    }
                }

                [DebuggerHidden]
                void IEnumerator.Reset()
                {
                    throw new NotSupportedException();
                }

                void IDisposable.Dispose()
                {
                    switch (this.<>1__state)
                    {
                        case 1:
                        case 2:
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally18();
                            }
                            break;
                    }
                }

                TKey IEnumerator<TKey>.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }

                object IEnumerator.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }
            }
        }

        public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable
        {
            private KeyValueList<TKey, TValue> FOwner;

            public ValueCollection(KeyValueList<TKey, TValue> owner)
            {
                this.FOwner = owner;
            }

            public void Add(TValue item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(TValue item)
            {
                return (this.FOwner.FindIndex(delegate (KeyValuePair<TKey, TValue> pair) {
                    return pair.Value.Equals(item);
                }) > 0);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException();
                }
                foreach (TValue local in this)
                {
                    array[arrayIndex++] = local;
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return new <GetEnumerator>d__1d<TKey, TValue>(0) { <>4__this = this };
            }

            public bool Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this.FOwner.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            [CompilerGenerated]
            private sealed class <GetEnumerator>d__1d : IEnumerator<TValue>, IEnumerator, IDisposable
            {
                private int <>1__state;
                private TValue <>2__current;
                public KeyValueList<TKey, TValue>.ValueCollection <>4__this;
                public List<KeyValuePair<TKey, TValue>>.Enumerator <>7__wrap1f;
                public KeyValuePair<TKey, TValue> <NextPair>5__1e;

                [DebuggerHidden]
                public <GetEnumerator>d__1d(int <>1__state)
                {
                    this.<>1__state = <>1__state;
                }

                private void <>m__Finally20()
                {
                    this.<>1__state = -1;
                    this.<>7__wrap1f.Dispose();
                }

                private bool MoveNext()
                {
                    try
                    {
                        switch (this.<>1__state)
                        {
                            case 0:
                                this.<>1__state = -1;
                                this.<>7__wrap1f = this.<>4__this.FOwner.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap1f.MoveNext())
                                {
                                    this.<NextPair>5__1e = this.<>7__wrap1f.Current;
                                    this.<>2__current = this.<NextPair>5__1e.Value;
                                    this.<>1__state = 2;
                                    return true;
                                Label_0079:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finally20();
                                break;

                            case 2:
                                goto Label_0079;
                        }
                        return false;
                    }
                    fault
                    {
                        this.System.IDisposable.Dispose();
                    }
                }

                [DebuggerHidden]
                void IEnumerator.Reset()
                {
                    throw new NotSupportedException();
                }

                void IDisposable.Dispose()
                {
                    switch (this.<>1__state)
                    {
                        case 1:
                        case 2:
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally20();
                            }
                            break;
                    }
                }

                TValue IEnumerator<TValue>.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }

                object IEnumerator.Current
                {
                    [DebuggerHidden]
                    get
                    {
                        return this.<>2__current;
                    }
                }
            }
        }
    }
}

