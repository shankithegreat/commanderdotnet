namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class RoundStack<T> : IStack<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private int BottomIndex;
        private T[] Items;
        private int TopIndex;
        private int Version;

        public RoundStack(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.Items = new T[capacity + 1];
        }

        public void Clear()
        {
            if (this.TopIndex != this.BottomIndex)
            {
                for (int i = this.Items.Length - 1; i >= 0; i--)
                {
                    this.Items[i] = default(T);
                }
                this.TopIndex = this.BottomIndex;
                this.Version++;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int topIndex = this.TopIndex;
            while (topIndex != this.BottomIndex)
            {
                if (--topIndex < 0)
                {
                    topIndex = this.Items.Length - 1;
                }
                array[arrayIndex++] = this.Items[topIndex];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new <GetEnumerator>d__0<T>(0) { <>4__this = this };
        }

        public T Peek()
        {
            if (this.TopIndex == this.BottomIndex)
            {
                throw new InvalidOperationException();
            }
            return this.Items[(this.TopIndex == 0) ? (this.Items.Length - 1) : (this.TopIndex - 1)];
        }

        public T Pop()
        {
            if (this.TopIndex == this.BottomIndex)
            {
                throw new InvalidOperationException();
            }
            this.Version++;
            if (--this.TopIndex < 0)
            {
                this.TopIndex = this.Items.Length - 1;
            }
            T local = this.Items[this.TopIndex];
            this.Items[this.TopIndex] = default(T);
            return local;
        }

        public void Push(T item)
        {
            this.Version++;
            this.Items[this.TopIndex] = item;
            if (++this.TopIndex > (this.Items.Length - 1))
            {
                this.TopIndex = 0;
            }
            if (this.TopIndex == this.BottomIndex)
            {
                if (++this.BottomIndex > (this.Items.Length - 1))
                {
                    this.BottomIndex = 0;
                }
                this.Items[this.TopIndex] = default(T);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            this.CopyTo(array, 0);
            return array;
        }

        public int Capacity
        {
            get
            {
                return (this.Items.Length - 1);
            }
        }

        public int Count
        {
            get
            {
                int num = this.TopIndex - this.BottomIndex;
                if (num < 0)
                {
                    num += this.Items.Length;
                }
                return num;
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

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<T>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private T <>2__current;
            public RoundStack<T> <>4__this;
            public int <PeekIndex>5__2;
            public int <RememberVersion>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<RememberVersion>5__1 = this.<>4__this.Version;
                        this.<PeekIndex>5__2 = this.<>4__this.TopIndex;
                        while (this.<PeekIndex>5__2 != this.<>4__this.BottomIndex)
                        {
                            if (this.<RememberVersion>5__1 != this.<>4__this.Version)
                            {
                                throw new InvalidOperationException();
                            }
                            if (--this.<PeekIndex>5__2 < 0)
                            {
                                this.<PeekIndex>5__2 = this.<>4__this.Items.Length - 1;
                            }
                            this.<>2__current = this.<>4__this.Items[this.<PeekIndex>5__2];
                            this.<>1__state = 1;
                            return true;
                        Label_00C4:
                            this.<>1__state = -1;
                        }
                        break;

                    case 1:
                        goto Label_00C4;
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

