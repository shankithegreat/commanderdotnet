namespace Nomad.Commons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class NaturalStringComparer : IComparer<string>, IEqualityComparer<string>
    {
        private IComparer<string> Comparer;

        public NaturalStringComparer(IComparer<string> comparer)
        {
            this.Comparer = comparer;
        }

        public NaturalStringComparer(StringComparison comparision)
        {
            switch (comparision)
            {
                case StringComparison.CurrentCulture:
                    this.Comparer = StringComparer.CurrentCulture;
                    break;

                case StringComparison.CurrentCultureIgnoreCase:
                    this.Comparer = StringComparer.CurrentCultureIgnoreCase;
                    break;

                case StringComparison.InvariantCulture:
                    this.Comparer = StringComparer.InvariantCulture;
                    break;

                case StringComparison.InvariantCultureIgnoreCase:
                    this.Comparer = StringComparer.InvariantCultureIgnoreCase;
                    break;

                case StringComparison.Ordinal:
                    this.Comparer = StringComparer.Ordinal;
                    break;

                case StringComparison.OrdinalIgnoreCase:
                    this.Comparer = StringComparer.OrdinalIgnoreCase;
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public int Compare(string x, string y)
        {
            using (IEnumerator<object> enumerator = EnumStringTokens(x).GetEnumerator())
            {
                using (IEnumerator<object> enumerator2 = EnumStringTokens(y).GetEnumerator())
                {
                    bool flag;
                    bool flag3;
                    goto Label_0109;
                Label_001F:
                    flag = enumerator.MoveNext();
                    bool flag2 = enumerator2.MoveNext();
                    if (flag && flag2)
                    {
                        string current = enumerator.Current as string;
                        string str2 = enumerator2.Current as string;
                        int num = 0;
                        if ((current != null) && (str2 != null))
                        {
                            num = this.Comparer.Compare(current, str2);
                        }
                        else if ((current == null) && (str2 == null))
                        {
                            num = ((int) enumerator.Current) - ((int) enumerator2.Current);
                        }
                        else
                        {
                            if (current != null)
                            {
                                return 1;
                            }
                            if (str2 != null)
                            {
                                return -1;
                            }
                        }
                        if (num != 0)
                        {
                            return num;
                        }
                    }
                    else
                    {
                        if (flag)
                        {
                            return 1;
                        }
                        if (!flag2)
                        {
                            goto Label_013B;
                        }
                        return -1;
                    }
                Label_0109:
                    flag3 = true;
                    goto Label_001F;
                }
            }
        Label_013B:
            return 0;
        }

        private static IEnumerable<object> EnumStringTokens(string str)
        {
            return new <EnumStringTokens>d__0(-2) { <>3__str = str };
        }

        public bool Equals(string x, string y)
        {
            return (this.Compare(x, y) == 0);
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            return obj.GetHashCode();
        }

        private static object GetToken(string str, int start, int end, bool isDigitToken)
        {
            int num;
            string s = str.Substring(start, end - start);
            if (isDigitToken && int.TryParse(s, out num))
            {
                return num;
            }
            return s;
        }

        [CompilerGenerated]
        private sealed class <EnumStringTokens>d__0 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private object <>2__current;
            public string <>3__str;
            private int <>l__initialThreadId;
            public bool <DigitToken>5__1;
            public int <I>5__2;
            public bool <IsDigit>5__4;
            public int <Start>5__3;
            public string str;

            [DebuggerHidden]
            public <EnumStringTokens>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        if (string.IsNullOrEmpty(this.str))
                        {
                            break;
                        }
                        this.<DigitToken>5__1 = false;
                        this.<I>5__2 = 0;
                        this.<Start>5__3 = 0;
                        while (this.<I>5__2 < this.str.Length)
                        {
                            this.<IsDigit>5__4 = char.IsDigit(this.str[this.<I>5__2]);
                            if (!(this.<IsDigit>5__4 ^ this.<DigitToken>5__1))
                            {
                                goto Label_00FD;
                            }
                            if (this.<I>5__2 <= this.<Start>5__3)
                            {
                                goto Label_00E4;
                            }
                            this.<>2__current = NaturalStringComparer.GetToken(this.str, this.<Start>5__3, this.<I>5__2, this.<DigitToken>5__1);
                            this.<>1__state = 1;
                            return true;
                        Label_00DD:
                            this.<>1__state = -1;
                        Label_00E4:
                            this.<DigitToken>5__1 = this.<IsDigit>5__4;
                            this.<Start>5__3 = this.<I>5__2;
                        Label_00FD:
                            this.<I>5__2++;
                        }
                        if (this.<I>5__2 <= this.<Start>5__3)
                        {
                            break;
                        }
                        this.<>2__current = NaturalStringComparer.GetToken(this.str, this.<Start>5__3, this.<I>5__2, this.<DigitToken>5__1);
                        this.<>1__state = 2;
                        return true;

                    case 1:
                        goto Label_00DD;

                    case 2:
                        this.<>1__state = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                NaturalStringComparer.<EnumStringTokens>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new NaturalStringComparer.<EnumStringTokens>d__0(0);
                }
                d__.str = this.<>3__str;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Object>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            object IEnumerator<object>.Current
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

