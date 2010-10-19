namespace Microsoft.Win32.Network
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class LmHelper
    {
        public static IEnumerable<string> GetShares(string serverName, STYPE shareType)
        {
            return new <GetShares>d__12(-2) { <>3__serverName = serverName, <>3__shareType = shareType };
        }

        public static IEnumerable<T> NetShareEnumerator<T>(string serverName)
        {
            return new <NetShareEnumerator>d__0<T>(-2) { <>3__serverName = serverName };
        }

        [CompilerGenerated]
        private sealed class <GetShares>d__12 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public string <>3__serverName;
            public STYPE <>3__shareType;
            public IEnumerator<SHARE_INFO_2> <>7__wrap15;
            public IEnumerator<share_info_50> <>7__wrap17;
            private int <>l__initialThreadId;
            public SHARE_INFO_2 <NextShare>5__13;
            public share_info_50 <NextShare>5__14;
            public string serverName;
            public STYPE shareType;

            [DebuggerHidden]
            public <GetShares>d__12(int <>1__state)
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

            private void <>m__Finally18()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap17 != null)
                {
                    this.<>7__wrap17.Dispose();
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
                            goto Label_00BE;

                        case 4:
                            goto Label_0163;

                        default:
                            goto Label_018A;
                    }
                    this.<>1__state = -1;
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        this.<>7__wrap15 = LmHelper.NetShareEnumerator<SHARE_INFO_2>(this.serverName).GetEnumerator();
                        this.<>1__state = 1;
                        while (this.<>7__wrap15.MoveNext())
                        {
                            this.<NextShare>5__13 = this.<>7__wrap15.Current;
                            if (this.<NextShare>5__13.ShareType != this.shareType)
                            {
                                continue;
                            }
                            this.<>2__current = this.<NextShare>5__13.Path;
                            this.<>1__state = 2;
                            return true;
                        Label_00BE:
                            this.<>1__state = 1;
                        }
                        this.<>m__Finally16();
                    }
                    else
                    {
                        if (Environment.OSVersion.Platform != PlatformID.Win32Windows)
                        {
                            throw new InvalidOperationException();
                        }
                        this.<>7__wrap17 = LmHelper.NetShareEnumerator<share_info_50>(this.serverName).GetEnumerator();
                        this.<>1__state = 3;
                        while (this.<>7__wrap17.MoveNext())
                        {
                            this.<NextShare>5__14 = this.<>7__wrap17.Current;
                            if (this.<NextShare>5__14.ShareType != this.shareType)
                            {
                                continue;
                            }
                            this.<>2__current = this.<NextShare>5__14.Path;
                            this.<>1__state = 4;
                            return true;
                        Label_0163:
                            this.<>1__state = 3;
                        }
                        this.<>m__Finally18();
                    }
                Label_018A:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                LmHelper.<GetShares>d__12 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new LmHelper.<GetShares>d__12(0);
                }
                d__.serverName = this.<>3__serverName;
                d__.shareType = this.<>3__shareType;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
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

                    case 3:
                    case 4:
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

            string IEnumerator<string>.Current
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
        private sealed class <NetShareEnumerator>d__0<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private T <>2__current;
            public string <>3__serverName;
            private int <>l__initialThreadId;
            public IntPtr <Buffer>5__3;
            public int <I>5__8;
            public int <I>5__c;
            public int <level>5__2;
            public PlatformID <Platform>5__1;
            public int <Readed>5__5;
            public ushort <Readed>5__a;
            public int <Result>5__4;
            public int <ResumeHandle>5__7;
            public IntPtr <SubBufer>5__9;
            public IntPtr <SubBufer>5__d;
            public int <Total>5__6;
            public ushort <Total>5__b;
            public string serverName;

            [DebuggerHidden]
            public <NetShareEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallye()
            {
                this.<>1__state = -1;
                Lm.NetApiBufferFree(this.<Buffer>5__3);
            }

            private void <>m__Finallyf()
            {
                this.<>1__state = -1;
                Marshal.FreeHGlobal(this.<Buffer>5__3);
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
                            this.<Platform>5__1 = Environment.OSVersion.Platform;
                            this.<level>5__2 = -1;
                            if (this.<Platform>5__1 != PlatformID.Win32NT)
                            {
                                break;
                            }
                            if (typeof(T) == typeof(SHARE_INFO_1))
                            {
                                this.<level>5__2 = 1;
                            }
                            else if (typeof(T) == typeof(SHARE_INFO_2))
                            {
                                this.<level>5__2 = 2;
                            }
                            goto Label_010E;

                        case 2:
                            goto Label_020B;

                        case 4:
                            goto Label_0334;

                        default:
                            goto Label_0371;
                    }
                    if (this.<Platform>5__1 == PlatformID.Win32Windows)
                    {
                        if (typeof(T) == typeof(share_info_1))
                        {
                            this.<level>5__2 = 1;
                        }
                        else if (typeof(T) == typeof(share_info_50))
                        {
                            this.<level>5__2 = 50;
                        }
                    }
                Label_010E:
                    if (this.<level>5__2 < 0)
                    {
                        throw new InvalidOperationException();
                    }
                    if (this.<Platform>5__1 == PlatformID.Win32NT)
                    {
                        this.<ResumeHandle>5__7 = 0;
                        do
                        {
                            this.<Result>5__4 = Lm.NetShareEnum(this.serverName, this.<level>5__2, out this.<Buffer>5__3, -1, out this.<Readed>5__5, out this.<Total>5__6, ref this.<ResumeHandle>5__7);
                            if ((this.<Result>5__4 != 0) && (this.<Result>5__4 != 0xea))
                            {
                                throw new Win32Exception(this.<Result>5__4);
                            }
                            this.<>1__state = 1;
                            this.<I>5__8 = 0;
                            while (this.<I>5__8 < this.<Readed>5__5)
                            {
                                this.<SubBufer>5__9 = new IntPtr(this.<Buffer>5__3.ToInt64() + (Marshal.SizeOf(typeof(T)) * this.<I>5__8));
                                this.<>2__current = (T) Marshal.PtrToStructure(this.<SubBufer>5__9, typeof(T));
                                this.<>1__state = 2;
                                return true;
                            Label_020B:
                                this.<>1__state = 1;
                                this.<I>5__8++;
                            }
                            this.<>m__Finallye();
                        }
                        while (this.<Result>5__4 == 0xea);
                    }
                    else
                    {
                        if (this.<Platform>5__1 != PlatformID.Win32Windows)
                        {
                            throw new InvalidOperationException();
                        }
                        this.<Buffer>5__3 = Marshal.AllocHGlobal(0xffff);
                        this.<>1__state = 3;
                        this.<Result>5__4 = Lm.NetShareEnum(this.serverName, this.<level>5__2, this.<Buffer>5__3, 0xffff, out this.<Readed>5__a, out this.<Total>5__b);
                        if (this.<Result>5__4 != 0)
                        {
                            throw new Win32Exception(this.<Result>5__4);
                        }
                        this.<I>5__c = 0;
                        while (this.<I>5__c < this.<Readed>5__a)
                        {
                            this.<SubBufer>5__d = new IntPtr(this.<Buffer>5__3.ToInt64() + (Marshal.SizeOf(typeof(T)) * this.<I>5__c));
                            this.<>2__current = (T) Marshal.PtrToStructure(this.<SubBufer>5__d, typeof(T));
                            this.<>1__state = 4;
                            return true;
                        Label_0334:
                            this.<>1__state = 3;
                            this.<I>5__c++;
                        }
                        this.<>m__Finallyf();
                    }
                Label_0371:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                LmHelper.<NetShareEnumerator>d__0<T> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (LmHelper.<NetShareEnumerator>d__0<T>) this;
                }
                else
                {
                    d__ = new LmHelper.<NetShareEnumerator>d__0<T>(0);
                }
                d__.serverName = this.<>3__serverName;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
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
                            this.<>m__Finallye();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyf();
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

