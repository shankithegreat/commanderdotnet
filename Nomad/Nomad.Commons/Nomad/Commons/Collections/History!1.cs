namespace Nomad.Commons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class History<T> : IEnumerable<T>, IEnumerable, ICloneable
    {
        private IStack<T> FBackHistory;
        private T FCurrent;
        private bool FCurrentExits;
        private IStack<T> FForwardHistory;
        private int Version;

        public History()
        {
            this.FCurrentExits = false;
        }

        public History(T current)
        {
            this.FCurrentExits = false;
            this.FCurrent = current;
            this.FCurrentExits = true;
        }

        public T Back()
        {
            if ((this.FBackHistory == null) || (this.FBackHistory.Count <= 0))
            {
                throw new InvalidOperationException();
            }
            IStack<T> stack = this.FForwardHistory ?? (this.FForwardHistory = new GeneralStack<T>());
            stack.Push(this.Current);
            this.FCurrent = this.FBackHistory.Pop();
            this.Version++;
            return this.FCurrent;
        }

        public void Clear()
        {
            if (this.FBackHistory != null)
            {
                this.FBackHistory.Clear();
            }
            if (this.FForwardHistory != null)
            {
                this.FForwardHistory.Clear();
            }
            this.FCurrent = default(T);
            this.FCurrentExits = false;
            this.Version++;
        }

        public object Clone()
        {
            History<T> history = (History<T>) base.MemberwiseClone();
            history.FBackHistory = History<T>.CloneHistory(this.FBackHistory);
            history.FForwardHistory = History<T>.CloneHistory(this.FForwardHistory);
            return history;
        }

        private static IStack<T> CloneHistory(IStack<T> source)
        {
            if (source == null)
            {
                return null;
            }
            RoundStack<T> stack = source as RoundStack<T>;
            if (stack != null)
            {
                IStack<T> stack2 = new RoundStack<T>(stack.Capacity);
                if (source.Count > 0)
                {
                    stack2.PushCollection<T>(source.Reverse<T>());
                }
                return stack2;
            }
            return new GeneralStack<T>(source.Reverse<T>());
        }

        public T Forward()
        {
            if ((this.FForwardHistory == null) || (this.FForwardHistory.Count <= 0))
            {
                throw new InvalidOperationException();
            }
            IStack<T> stack = this.FBackHistory ?? (this.FBackHistory = new GeneralStack<T>());
            stack.Push(this.Current);
            this.FCurrent = this.FForwardHistory.Pop();
            this.Version++;
            return this.FCurrent;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new <GetEnumerator>d__0<T>(0) { <>4__this = this };
        }

        private static int GetHistoryDepth(IStack<T> history)
        {
            RoundStack<T> stack = history as RoundStack<T>;
            if (stack != null)
            {
                return stack.Capacity;
            }
            return 0;
        }

        public T Move(int depth)
        {
            T local;
            int num;
            if (depth > 0)
            {
                local = this.Forward();
                for (num = 1; num < depth; num++)
                {
                    local = this.Forward();
                }
                return local;
            }
            if (depth >= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            local = this.Back();
            for (num = -1; num > depth; num--)
            {
                local = this.Back();
            }
            return local;
        }

        public T PeekBack()
        {
            if ((this.FBackHistory != null) && (this.FBackHistory.Count > 0))
            {
                return this.FBackHistory.Peek();
            }
            return default(T);
        }

        public T PeekForward()
        {
            if ((this.FForwardHistory != null) && (this.FForwardHistory.Count > 0))
            {
                return this.FForwardHistory.Peek();
            }
            return default(T);
        }

        private static void SetHistoryDepth(ref IStack<T> history, int newHistoryDepth)
        {
            IStack<T> collection = history;
            if (newHistoryDepth > 0)
            {
                history = new RoundStack<T>(newHistoryDepth);
                if ((collection != null) && (collection.Count > 0))
                {
                    history.PushCollection<T>(collection);
                }
            }
            else if ((collection != null) && (collection.Count > 0))
            {
                history = new GeneralStack<T>(collection);
            }
            else
            {
                history = null;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int BackCount
        {
            get
            {
                return ((this.FBackHistory != null) ? this.FBackHistory.Count : 0);
            }
        }

        public IEnumerable<T> BackHistory
        {
            get
            {
                return ((this.FBackHistory != null) ? ((IEnumerable<T>) this.FBackHistory) : ((IEnumerable<T>) new EmptyEnumerable<T>()));
            }
        }

        public T Current
        {
            get
            {
                if (!this.FCurrentExits)
                {
                    throw new InvalidOperationException();
                }
                return this.FCurrent;
            }
            set
            {
                if (this.FCurrentExits)
                {
                    IStack<T> stack = this.FBackHistory ?? (this.FBackHistory = new GeneralStack<T>());
                    stack.Push(this.FCurrent);
                }
                this.FCurrent = value;
                this.FCurrentExits = true;
                if (this.FForwardHistory != null)
                {
                    this.FForwardHistory.Clear();
                }
                this.Version++;
            }
        }

        public int ForwardCount
        {
            get
            {
                return ((this.FForwardHistory != null) ? this.FForwardHistory.Count : 0);
            }
        }

        public IEnumerable<T> ForwardHistory
        {
            get
            {
                return ((this.FForwardHistory != null) ? ((IEnumerable<T>) this.FForwardHistory) : ((IEnumerable<T>) new EmptyEnumerable<T>()));
            }
        }

        public bool HasCurrent
        {
            get
            {
                return this.FCurrentExits;
            }
        }

        public int MaxBackDepth
        {
            get
            {
                return History<T>.GetHistoryDepth(this.FBackHistory);
            }
            set
            {
                if (History<T>.GetHistoryDepth(this.FBackHistory) != value)
                {
                    History<T>.SetHistoryDepth(ref this.FBackHistory, value);
                    this.Version++;
                }
            }
        }

        public int MaxForwardDepth
        {
            get
            {
                return History<T>.GetHistoryDepth(this.FForwardHistory);
            }
            set
            {
                if (History<T>.GetHistoryDepth(this.FForwardHistory) != value)
                {
                    History<T>.SetHistoryDepth(ref this.FForwardHistory, value);
                    this.Version++;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<T>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private T <>2__current;
            public History<T> <>4__this;
            public IEnumerator<T> <>7__wrap4;
            public IEnumerator<T> <>7__wrap6;
            public T <NextValue>5__2;
            public T <NextValue>5__3;
            public int <RememberVersion>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private void <>m__Finally7()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap6 != null)
                {
                    this.<>7__wrap6.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<RememberVersion>5__1 = this.<>4__this.Version;
                            if (!this.<>4__this.FCurrentExits)
                            {
                                goto Label_008B;
                            }
                            this.<>2__current = this.<>4__this.FCurrent;
                            this.<>1__state = 1;
                            return true;

                        case 1:
                            break;

                        case 3:
                            goto Label_0109;

                        case 5:
                            goto Label_01A2;

                        default:
                            goto Label_01C1;
                    }
                    this.<>1__state = -1;
                Label_008B:
                    if (this.<>4__this.FBackHistory != null)
                    {
                        this.<>7__wrap4 = this.<>4__this.FBackHistory.GetEnumerator();
                        this.<>1__state = 2;
                        while (this.<>7__wrap4.MoveNext())
                        {
                            this.<NextValue>5__2 = this.<>7__wrap4.Current;
                            if (this.<>4__this.Version != this.<RememberVersion>5__1)
                            {
                                throw new InvalidOperationException();
                            }
                            this.<>2__current = this.<NextValue>5__2;
                            this.<>1__state = 3;
                            return true;
                        Label_0109:
                            this.<>1__state = 2;
                        }
                        this.<>m__Finally5();
                    }
                    if (this.<>4__this.FForwardHistory != null)
                    {
                        this.<>7__wrap6 = this.<>4__this.FForwardHistory.GetEnumerator();
                        this.<>1__state = 4;
                        while (this.<>7__wrap6.MoveNext())
                        {
                            this.<NextValue>5__3 = this.<>7__wrap6.Current;
                            if (this.<>4__this.Version != this.<RememberVersion>5__1)
                            {
                                throw new InvalidOperationException();
                            }
                            this.<>2__current = this.<NextValue>5__3;
                            this.<>1__state = 5;
                            return true;
                        Label_01A2:
                            this.<>1__state = 4;
                        }
                        this.<>m__Finally7();
                    }
                Label_01C1:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
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
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
                        break;

                    case 4:
                    case 5:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally7();
                        }
                        break;
                }
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

