namespace Nomad.Commons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class EnumHelper
    {
        public static IEnumerable<object> GetBrowsableValues(Type enumType)
        {
            return new <GetBrowsableValues>d__0(-2) { <>3__enumType = enumType };
        }

        public static T[] GetValues<T>() where T: struct
        {
            Array values = Enum.GetValues(typeof(T));
            T[] localArray = new T[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                localArray[i] = (T) values.GetValue(i);
            }
            return localArray;
        }

        public static T Parse<T>(string value) where T: struct
        {
            return Parse<T>(value, false);
        }

        public static T Parse<T>(string value, bool ignoreCase) where T: struct
        {
            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static bool TryParse<T>(string value, out T result) where T: struct
        {
            return TryParse<T>(value, false, out result);
        }

        public static bool TryParse<T>(string value, bool ignoreCase, out T result) where T: struct
        {
            try
            {
                result = (T) Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch (ArgumentException)
            {
                result = default(T);
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <GetBrowsableValues>d__0 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private object <>2__current;
            public Type <>3__enumType;
            public FieldInfo[] <>7__wrap4;
            public int <>7__wrap5;
            private int <>l__initialThreadId;
            public object[] <Attributes>5__2;
            public FieldInfo <NextField>5__1;
            public Type enumType;

            [DebuggerHidden]
            public <GetBrowsableValues>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>1__state = 1;
                            this.<>7__wrap4 = this.enumType.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static);
                            this.<>7__wrap5 = 0;
                            while (this.<>7__wrap5 < this.<>7__wrap4.Length)
                            {
                                this.<NextField>5__1 = this.<>7__wrap4[this.<>7__wrap5];
                                this.<Attributes>5__2 = this.<NextField>5__1.GetCustomAttributes(typeof(BrowsableAttribute), false);
                                if (!((this.<Attributes>5__2.Length <= 0) || ((BrowsableAttribute) this.<Attributes>5__2[0]).Browsable))
                                {
                                    goto Label_00D4;
                                }
                                this.<>2__current = this.<NextField>5__1.GetValue(null);
                                this.<>1__state = 2;
                                return true;
                            Label_00CC:
                                this.<>1__state = 1;
                            Label_00D4:
                                this.<>7__wrap5++;
                            }
                            this.<>m__Finally3();
                            break;

                        case 2:
                            goto Label_00CC;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                EnumHelper.<GetBrowsableValues>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new EnumHelper.<GetBrowsableValues>d__0(0);
                }
                d__.enumType = this.<>3__enumType;
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
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        this.<>m__Finally3();
                        break;
                }
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

