namespace Nomad.FileSystem.Archive.Wcx
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class WcxArchiveContent : IEnumerable<ISimpleItem>, IEnumerable, IGetArchiveFormatInfo
    {
        private WcxArchiveContext Context;
        private WeakReference FContent;

        internal WcxArchiveContent(WcxArchiveContext context)
        {
            this.Context = context;
        }

        public IEnumerator<ISimpleItem> GetEnumerator()
        {
            return new <GetEnumerator>d__0(0) { <>4__this = this };
        }

        public bool RefreshContent()
        {
            this.FContent = null;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GetEnumerator>d__e(0) { <>4__this = this };
        }

        public ArchiveFormatInfo FormatInfo
        {
            get
            {
                return this.Context.FormatInfo;
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<ISimpleItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ISimpleItem <>2__current;
            public WcxArchiveContent <>4__this;
            public IEnumerator<ISimpleItem> <>7__wrapa;
            public IntPtr <ArcHandle>5__3;
            public HeaderDataEx <HeaderEx>5__6;
            public int <Index>5__5;
            public ISimpleItem <Item>5__8;
            public List<ISimpleItem> <NewContent>5__4;
            public ISimpleItem <NextItem>5__1;
            public OpenArchiveData <OpenData>5__2;
            public int <ProcessResult>5__9;
            public int <ReadResult>5__7;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapa != null)
                {
                    this.<>7__wrapa.Dispose();
                }
            }

            private void <>m__Finallyc()
            {
                this.<>1__state = -1;
                this.<>4__this.Context.FormatInfo.CloseArchive(this.<ArcHandle>5__3);
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
                            if ((this.<>4__this.FContent == null) || !this.<>4__this.FContent.IsAlive)
                            {
                                break;
                            }
                            this.<>7__wrapa = ((IEnumerable<ISimpleItem>) this.<>4__this.FContent.Target).GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrapa.MoveNext())
                            {
                                this.<NextItem>5__1 = this.<>7__wrapa.Current;
                                this.<>2__current = this.<NextItem>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_00B9:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finallyb();
                            goto Label_02A7;

                        case 2:
                            goto Label_00B9;

                        case 4:
                            goto Label_01E9;

                        default:
                            goto Label_02A7;
                    }
                    this.<OpenData>5__2 = new OpenArchiveData();
                    this.<OpenData>5__2.ArcName = this.<>4__this.Context.ArchiveName;
                    this.<OpenData>5__2.OpenMode = PK_OM.PK_OM_LIST;
                    this.<ArcHandle>5__3 = this.<>4__this.Context.FormatInfo.OpenArchive(ref this.<OpenData>5__2);
                    if (this.<OpenData>5__2.OpenResult != 0)
                    {
                        WcxErrors.ThrowExceptionForError(this.<OpenData>5__2.OpenResult);
                    }
                    this.<>1__state = 3;
                    this.<NewContent>5__4 = new List<ISimpleItem>();
                    this.<Index>5__5 = 0;
                    this.<HeaderEx>5__6 = new HeaderDataEx();
                    this.<ReadResult>5__7 = this.<>4__this.Context.ReadHeader(this.<ArcHandle>5__3, ref this.<HeaderEx>5__6);
                    while (this.<ReadResult>5__7 == 0)
                    {
                        this.<Item>5__8 = new WcxArchiveItem(this.<>4__this.Context, this.<HeaderEx>5__6, this.<Index>5__5++);
                        this.<>2__current = this.<Item>5__8;
                        this.<>1__state = 4;
                        return true;
                    Label_01E9:
                        this.<>1__state = 3;
                        this.<ProcessResult>5__9 = this.<>4__this.Context.ProcessFile(this.<ArcHandle>5__3, PK_OPERATION.PK_SKIP, null, null);
                        if (this.<ProcessResult>5__9 != 0)
                        {
                            WcxErrors.ThrowExceptionForError(this.<ProcessResult>5__9);
                        }
                        this.<NewContent>5__4.Add(this.<Item>5__8);
                        this.<ReadResult>5__7 = this.<>4__this.Context.ReadHeader(this.<ArcHandle>5__3, ref this.<HeaderEx>5__6);
                    }
                    if (this.<ReadResult>5__7 != 10)
                    {
                        WcxErrors.ThrowExceptionForError(this.<ReadResult>5__7);
                    }
                    this.<>4__this.FContent = new WeakReference(this.<NewContent>5__4);
                    this.<>m__Finallyc();
                Label_02A7:
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
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyb();
                        }
                        break;

                    case 3:
                    case 4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyc();
                        }
                        break;
                }
            }

            ISimpleItem IEnumerator<ISimpleItem>.Current
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
        private sealed class GetEnumerator>d__e : IEnumerator<object>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private object <>2__current;
            public WcxArchiveContent <>4__this;
            public IEnumerator<ISimpleItem> <>7__wrap10;
            public ISimpleItem <NextItem>5__f;

            [DebuggerHidden]
            public GetEnumerator>d__e(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally11()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap10 != null)
                {
                    this.<>7__wrap10.Dispose();
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
                            this.<>7__wrap10 = this.<>4__this.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap10.MoveNext())
                            {
                                this.<NextItem>5__f = this.<>7__wrap10.Current;
                                this.<>2__current = this.<NextItem>5__f;
                                this.<>1__state = 2;
                                return true;
                            Label_006C:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally11();
                            break;

                        case 2:
                            goto Label_006C;
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
                            this.<>m__Finally11();
                        }
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

