namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class OneWayList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private int _Count;
        private OneWayNode<T> _First;
        private OneWayNode<T> _Last;

        public OneWayList()
        {
        }

        public OneWayList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }
            foreach (T local in collection)
            {
                this.AddLast(local);
            }
        }

        public void Add(T item)
        {
            this.AddLast(item);
        }

        public OneWayNode<T> AddFirst(T value)
        {
            if (this._First == null)
            {
                this._First = new OneWayNode<T>();
                this._Last = this._First;
            }
            else
            {
                OneWayNode<T> node = new OneWayNode<T> {
                    _Next = this._First
                };
                this._First = node;
            }
            this._First.Value = value;
            this._Count++;
            return this._First;
        }

        public OneWayNode<T> AddLast(T value)
        {
            if (this._Last == null)
            {
                this._Last = new OneWayNode<T>();
                this._First = this._Last;
            }
            else
            {
                OneWayNode<T> node = new OneWayNode<T>();
                this._Last._Next = node;
                this._Last = node;
            }
            this._Last.Value = value;
            this._Count++;
            return this._Last;
        }

        public void Clear()
        {
            this._First = null;
            this._Last = null;
            this._Count = 0;
        }

        public bool Contains(T item)
        {
            return this.Any<T>(delegate (T x) {
                return EqualityComparer<T>.Default.Equals(x, item);
            });
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T local in this)
            {
                array[arrayIndex++] = local;
            }
        }

        public void CopyTo(Array array, int index)
        {
            foreach (object obj2 in (IEnumerable) this)
            {
                array.SetValue(obj2, index++);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new <GetEnumerator>d__3<T>(0) { <>4__this = this };
        }

        public bool Remove(T item)
        {
            OneWayNode<T> node = null;
            for (OneWayNode<T> node2 = this._First; node2 != null; node2 = node2.Next)
            {
                if (EqualityComparer<T>.Default.Equals(node2.Value, item))
                {
                    if (node == null)
                    {
                        this._First = node2.Next;
                        if (this._First == null)
                        {
                            this._Last = null;
                        }
                    }
                    else
                    {
                        node._Next = node2.Next;
                        if (node2.Next == null)
                        {
                            this._Last = node;
                        }
                    }
                    this._Count--;
                    return true;
                }
                node = node2;
            }
            return false;
        }

        public int RemoveAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException();
            }
            int num = this._Count;
            OneWayNode<T> node = null;
            for (OneWayNode<T> node2 = this._First; node2 != null; node2 = node2.Next)
            {
                if (match(node2.Value))
                {
                    if (node == null)
                    {
                        this._First = node2.Next;
                    }
                    else
                    {
                        node._Next = node2.Next;
                    }
                    this._Count--;
                }
                else
                {
                    node = node2;
                }
            }
            this._Last = node;
            return (num - this._Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._Count;
            }
        }

        public OneWayNode<T> First
        {
            get
            {
                return this._First;
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

        public OneWayNode<T> Last
        {
            get
            {
                return this._Last;
            }
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__3 : IEnumerator<T>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private T <>2__current;
            public OneWayList<T> <>4__this;
            public OneWayNode<T> <CurrentNode>5__4;

            [DebuggerHidden]
            public <GetEnumerator>d__3(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<CurrentNode>5__4 = this.<>4__this._First;
                        while (this.<CurrentNode>5__4 != null)
                        {
                            this.<>2__current = this.<CurrentNode>5__4.Value;
                            this.<>1__state = 1;
                            return true;
                        Label_0055:
                            this.<>1__state = -1;
                            this.<CurrentNode>5__4 = this.<CurrentNode>5__4.Next;
                        }
                        break;

                    case 1:
                        goto Label_0055;
                }
                return false;
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            T IEnumerator<T>.Current
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

