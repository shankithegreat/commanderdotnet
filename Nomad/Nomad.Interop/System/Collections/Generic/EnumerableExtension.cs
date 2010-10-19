namespace System.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EnumerableExtension
    {
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            ICollection is2 = source as ICollection;
            if (is2 != null)
            {
                return (is2.Count > 0);
            }
            ICollection<TSource> is3 = source as ICollection<TSource>;
            if (is3 != null)
            {
                return (is3.Count > 0);
            }
            foreach (TSource local in source)
            {
                return true;
            }
            return false;
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            foreach (TSource local in source)
            {
                if (predicate(local))
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            return ((source != null) ? source : ((IEnumerable<TSource>) new TSource[0]));
        }

        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
        {
            return new <Cast>d__25<TResult>(-2) { <>3__source = source };
        }

        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return new <Concat>d__0<TSource>(-2) { <>3__first = first, <>3__second = second };
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source.Contains<TSource>(value, EqualityComparer<TSource>.Default);
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            ICollection<TSource> is2 = source as ICollection<TSource>;
            if (is2 != null)
            {
                return is2.Contains(value);
            }
            foreach (TSource local in source)
            {
                if (comparer.Equals(local, value))
                {
                    return true;
                }
            }
            return false;
        }

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            ICollection is2 = source as ICollection;
            if (is2 != null)
            {
                return is2.Count;
            }
            ICollection<TSource> is3 = source as ICollection<TSource>;
            if (is3 != null)
            {
                return is3.Count;
            }
            int num = 0;
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    num++;
                }
            }
            return num;
        }

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            int num = 0;
            foreach (TSource local in source)
            {
                if (predicate(local))
                {
                    num++;
                }
            }
            return num;
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }
            throw new InvalidOperationException();
        }

        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }
            return default(TSource);
        }

        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count <= 0)
                {
                    throw new InvalidOperationException();
                }
                return list[list.Count - 1];
            }
            ICollection<TSource> is2 = source as ICollection<TSource>;
            if ((is2 != null) && (is2.Count == 0))
            {
                throw new InvalidOperationException();
            }
            bool flag = false;
            TSource local = default(TSource);
            foreach (TSource local2 in source)
            {
                local = local2;
                flag = true;
            }
            if (!flag)
            {
                throw new InvalidOperationException();
            }
            return local;
        }

        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
        {
            return new <OfType>d__1e<TResult>(-2) { <>3__source = source };
        }

        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            return new <Reverse>d__9<TSource>(-2) { <>3__source = source };
        }

        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            ICollection<TSource> is2 = source as ICollection<TSource>;
            if ((is2 != null) && (is2.Count != 1))
            {
                throw new InvalidOperationException();
            }
            bool flag = false;
            TSource local = default(TSource);
            foreach (TSource local2 in source)
            {
                if (flag)
                {
                    throw new InvalidOperationException();
                }
                local = local2;
                flag = true;
            }
            if (!flag)
            {
                throw new InvalidOperationException();
            }
            return local;
        }

        public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            bool flag = false;
            TSource local = default(TSource);
            foreach (TSource local2 in source)
            {
                if (flag)
                {
                    throw new InvalidOperationException();
                }
                local = local2;
                flag = true;
            }
            return local;
        }

        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
        {
            return new <Skip>d__13<TSource>(-2) { <>3__source = source, <>3__count = count };
        }

        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
        {
            return new <Take>d__19<TSource>(-2) { <>3__source = source, <>3__count = count };
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            TSource[] localArray;
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            ICollection<TSource> is2 = source as ICollection<TSource>;
            if (is2 != null)
            {
                localArray = new TSource[is2.Count];
                is2.CopyTo(localArray, 0);
                return localArray;
            }
            ICollection is3 = source as ICollection;
            if (is3 != null)
            {
                int num = 0;
                localArray = new TSource[is3.Count];
                foreach (object obj2 in is3)
                {
                    localArray[num++] = (TSource) obj2;
                }
                return localArray;
            }
            List<TSource> list = new List<TSource>();
            foreach (object obj2 in source)
            {
                list.Add((TSource) obj2);
            }
            return list.ToArray();
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return new <Where>d__2c<TSource>(-2) { <>3__source = source, <>3__predicate = predicate };
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new <Where>d__32<TSource>(-2) { <>3__source = source, <>3__predicate = predicate };
        }

        [CompilerGenerated]
        private sealed class <Cast>d__25<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public IEnumerable <>3__source;
            public IEnumerator <>7__wrap27;
            public IDisposable <>7__wrap28;
            private int <>l__initialThreadId;
            public object <NextValue>5__26;
            public IEnumerable source;

            [DebuggerHidden]
            public <Cast>d__25(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally29()
            {
                this.<>1__state = -1;
                this.<>7__wrap28 = this.<>7__wrap27 as IDisposable;
                if (this.<>7__wrap28 != null)
                {
                    this.<>7__wrap28.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.source == null)
                            {
                                throw new ArgumentNullException();
                            }
                            this.<>7__wrap27 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap27.MoveNext())
                            {
                                this.<NextValue>5__26 = this.<>7__wrap27.Current;
                                this.<>2__current = (TResult) this.<NextValue>5__26;
                                this.<>1__state = 2;
                                return true;
                            Label_008A:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally29();
                            break;

                        case 2:
                            goto Label_008A;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                EnumerableExtension.<Cast>d__25<TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Cast>d__25<TResult>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Cast>d__25<TResult>(0);
                }
                d__.source = this.<>3__source;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
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
                            this.<>m__Finally29();
                        }
                        break;
                }
            }

            TResult IEnumerator<TResult>.Current
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

        [CompilerGenerated]
        private sealed class <Concat>d__0<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public IEnumerable<TSource> <>3__first;
            public IEnumerable<TSource> <>3__second;
            public IEnumerator<TSource> <>7__wrap3;
            public IEnumerator<TSource> <>7__wrap5;
            private int <>l__initialThreadId;
            public TSource <NextValue>5__1;
            public TSource <NextValue>5__2;
            public IEnumerable<TSource> first;
            public IEnumerable<TSource> second;

            [DebuggerHidden]
            public <Concat>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
                }
            }

            private void <>m__Finally6()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap5 != null)
                {
                    this.<>7__wrap5.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.first == null)
                            {
                                throw new ArgumentNullException("first");
                            }
                            if (this.second == null)
                            {
                                throw new ArgumentNullException("second");
                            }
                            this.<>7__wrap3 = this.first.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap3.MoveNext())
                            {
                                this.<NextValue>5__1 = this.<>7__wrap3.Current;
                                this.<>2__current = this.<NextValue>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_00B8:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally4();
                            this.<>7__wrap5 = this.second.GetEnumerator();
                            this.<>1__state = 3;
                            while (this.<>7__wrap5.MoveNext())
                            {
                                this.<NextValue>5__2 = this.<>7__wrap5.Current;
                                this.<>2__current = this.<NextValue>5__2;
                                this.<>1__state = 4;
                                return true;
                            Label_0118:
                                this.<>1__state = 3;
                            }
                            this.<>m__Finally6();
                            break;

                        case 2:
                            goto Label_00B8;

                        case 4:
                            goto Label_0118;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Concat>d__0<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Concat>d__0<TSource>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Concat>d__0<TSource>(0);
                }
                d__.first = this.<>3__first;
                d__.second = this.<>3__second;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                            this.<>m__Finally4();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally6();
                        }
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

        [CompilerGenerated]
        private sealed class <OfType>d__1e<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public IEnumerable <>3__source;
            public IEnumerator <>7__wrap20;
            public IDisposable <>7__wrap21;
            private int <>l__initialThreadId;
            public object <NextValue>5__1f;
            public IEnumerable source;

            [DebuggerHidden]
            public <OfType>d__1e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally22()
            {
                this.<>1__state = -1;
                this.<>7__wrap21 = this.<>7__wrap20 as IDisposable;
                if (this.<>7__wrap21 != null)
                {
                    this.<>7__wrap21.Dispose();
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
                            break;

                        case 2:
                            goto Label_00A2;

                        default:
                            goto Label_00C0;
                    }
                    this.<>1__state = -1;
                    if (this.source == null)
                    {
                        throw new ArgumentNullException();
                    }
                    this.<>7__wrap20 = this.source.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap20.MoveNext())
                    {
                        this.<NextValue>5__1f = this.<>7__wrap20.Current;
                        if (!(this.<NextValue>5__1f is TResult))
                        {
                            continue;
                        }
                        this.<>2__current = (TResult) this.<NextValue>5__1f;
                        this.<>1__state = 2;
                        return true;
                    Label_00A2:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally22();
                Label_00C0:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                EnumerableExtension.<OfType>d__1e<TResult> d__e;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__e = (EnumerableExtension.<OfType>d__1e<TResult>) this;
                }
                else
                {
                    d__e = new EnumerableExtension.<OfType>d__1e<TResult>(0);
                }
                d__e.source = this.<>3__source;
                return d__e;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
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
                            this.<>m__Finally22();
                        }
                        break;
                }
            }

            TResult IEnumerator<TResult>.Current
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

        [CompilerGenerated]
        private sealed class <Reverse>d__9<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public IEnumerable<TSource> <>3__source;
            public int <>7__wrap10;
            public TSource[] <>7__wrapf;
            private int <>l__initialThreadId;
            public ICollection<TSource> <collectionOfT>5__a;
            public TSource <NextResult>5__c;
            public TSource[] <Result>5__b;
            public Stack<TSource> <Result>5__d;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <Reverse>d__9(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallye()
            {
                this.<>1__state = -1;
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
                            if (this.source == null)
                            {
                                throw new ArgumentNullException();
                            }
                            this.<collectionOfT>5__a = this.source as ICollection<TSource>;
                            if (this.<collectionOfT>5__a == null)
                            {
                                break;
                            }
                            this.<Result>5__b = new TSource[this.<collectionOfT>5__a.Count];
                            this.<collectionOfT>5__a.CopyTo(this.<Result>5__b, 0);
                            Array.Reverse(this.<Result>5__b);
                            this.<>1__state = 1;
                            this.<>7__wrapf = this.<Result>5__b;
                            this.<>7__wrap10 = 0;
                            while (this.<>7__wrap10 < this.<>7__wrapf.Length)
                            {
                                this.<NextResult>5__c = this.<>7__wrapf[this.<>7__wrap10];
                                this.<>2__current = this.<NextResult>5__c;
                                this.<>1__state = 2;
                                return true;
                            Label_00F3:
                                this.<>1__state = 1;
                                this.<>7__wrap10++;
                            }
                            this.<>m__Finallye();
                            goto Label_0171;

                        case 2:
                            goto Label_00F3;

                        case 3:
                            goto Label_0156;

                        default:
                            goto Label_0171;
                    }
                    this.<Result>5__d = new Stack<TSource>(this.source);
                    while (this.<Result>5__d.Count > 0)
                    {
                        this.<>2__current = this.<Result>5__d.Pop();
                        this.<>1__state = 3;
                        return true;
                    Label_0156:
                        this.<>1__state = -1;
                    }
                Label_0171:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Reverse>d__9<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Reverse>d__9<TSource>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Reverse>d__9<TSource>(0);
                }
                d__.source = this.<>3__source;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                        this.<>m__Finallye();
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

        [CompilerGenerated]
        private sealed class <Skip>d__13<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public int <>3__count;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap15;
            private int <>l__initialThreadId;
            public TSource <NextValue>5__14;
            public int count;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <Skip>d__13(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally16()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap15 != null)
                {
                    this.<>7__wrap15.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.source == null)
                            {
                                throw new ArgumentNullException();
                            }
                            this.<>7__wrap15 = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap15.MoveNext())
                            {
                                this.<NextValue>5__14 = this.<>7__wrap15.Current;
                                if (this.count-- > 0)
                                {
                                    goto Label_00A8;
                                }
                                this.<>2__current = this.<NextValue>5__14;
                                this.<>1__state = 2;
                                return true;
                            Label_00A1:
                                this.<>1__state = 1;
                            Label_00A8:;
                            }
                            this.<>m__Finally16();
                            break;

                        case 2:
                            goto Label_00A1;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Skip>d__13<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Skip>d__13<TSource>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Skip>d__13<TSource>(0);
                }
                d__.source = this.<>3__source;
                d__.count = this.<>3__count;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                            this.<>m__Finally16();
                        }
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

        [CompilerGenerated]
        private sealed class <Take>d__19<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public int <>3__count;
            public IEnumerable<TSource> <>3__source;
            private int <>l__initialThreadId;
            public IEnumerator<TSource> <Enumerator>5__1a;
            public int count;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <Take>d__19(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally1b()
            {
                this.<>1__state = -1;
                if (this.<Enumerator>5__1a != null)
                {
                    this.<Enumerator>5__1a.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.source == null)
                            {
                                throw new ArgumentNullException();
                            }
                            this.<Enumerator>5__1a = this.source.GetEnumerator();
                            this.<>1__state = 1;
                            while ((this.count-- > 0) && this.<Enumerator>5__1a.MoveNext())
                            {
                                this.<>2__current = this.<Enumerator>5__1a.Current;
                                this.<>1__state = 2;
                                return true;
                            Label_0079:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally1b();
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
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Take>d__19<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Take>d__19<TSource>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Take>d__19<TSource>(0);
                }
                d__.source = this.<>3__source;
                d__.count = this.<>3__count;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                            this.<>m__Finally1b();
                        }
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

        [CompilerGenerated]
        private sealed class <Where>d__2c<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public Func<TSource, bool> <>3__predicate;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap2e;
            private int <>l__initialThreadId;
            public TSource <NextValue>5__2d;
            public Func<TSource, bool> predicate;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <Where>d__2c(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally2f()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap2e != null)
                {
                    this.<>7__wrap2e.Dispose();
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
                            break;

                        case 2:
                            goto Label_00C0;

                        default:
                            goto Label_00DE;
                    }
                    this.<>1__state = -1;
                    if (this.source == null)
                    {
                        throw new ArgumentNullException("source");
                    }
                    if (this.predicate == null)
                    {
                        throw new ArgumentNullException("predicate");
                    }
                    this.<>7__wrap2e = this.source.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap2e.MoveNext())
                    {
                        this.<NextValue>5__2d = this.<>7__wrap2e.Current;
                        if (!this.predicate(this.<NextValue>5__2d))
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextValue>5__2d;
                        this.<>1__state = 2;
                        return true;
                    Label_00C0:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally2f();
                Label_00DE:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Where>d__2c<TSource> d__c;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__c = (EnumerableExtension.<Where>d__2c<TSource>) this;
                }
                else
                {
                    d__c = new EnumerableExtension.<Where>d__2c<TSource>(0);
                }
                d__c.source = this.<>3__source;
                d__c.predicate = this.<>3__predicate;
                return d__c;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                            this.<>m__Finally2f();
                        }
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

        [CompilerGenerated]
        private sealed class <Where>d__32<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TSource <>2__current;
            public Func<TSource, int, bool> <>3__predicate;
            public IEnumerable<TSource> <>3__source;
            public IEnumerator<TSource> <>7__wrap35;
            private int <>l__initialThreadId;
            public int <I>5__33;
            public TSource <NextValue>5__34;
            public Func<TSource, int, bool> predicate;
            public IEnumerable<TSource> source;

            [DebuggerHidden]
            public <Where>d__32(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally36()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap35 != null)
                {
                    this.<>7__wrap35.Dispose();
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
                            break;

                        case 2:
                            goto Label_00D8;

                        default:
                            goto Label_00F6;
                    }
                    this.<>1__state = -1;
                    if (this.source == null)
                    {
                        throw new ArgumentNullException("source");
                    }
                    if (this.predicate == null)
                    {
                        throw new ArgumentNullException("predicate");
                    }
                    this.<I>5__33 = 0;
                    this.<>7__wrap35 = this.source.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap35.MoveNext())
                    {
                        this.<NextValue>5__34 = this.<>7__wrap35.Current;
                        if (!this.predicate(this.<NextValue>5__34, this.<I>5__33++))
                        {
                            continue;
                        }
                        this.<>2__current = this.<NextValue>5__34;
                        this.<>1__state = 2;
                        return true;
                    Label_00D8:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally36();
                Label_00F6:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
            {
                EnumerableExtension.<Where>d__32<TSource> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (EnumerableExtension.<Where>d__32<TSource>) this;
                }
                else
                {
                    d__ = new EnumerableExtension.<Where>d__32<TSource>(0);
                }
                d__.source = this.<>3__source;
                d__.predicate = this.<>3__predicate;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TSource>.GetEnumerator();
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
                            this.<>m__Finally36();
                        }
                        break;
                }
            }

            TSource IEnumerator<TSource>.Current
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

