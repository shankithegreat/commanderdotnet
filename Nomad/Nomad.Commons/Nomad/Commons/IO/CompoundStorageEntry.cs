namespace Nomad.Commons.IO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class CompoundStorageEntry : CompoundEntry
    {
        private const uint NOSTREAM = uint.MaxValue;

        private static int CompareNames(string name1, string name2)
        {
            int num = name1.Length - name2.Length;
            if (num == 0)
            {
                num = string.Compare(name1, name2, StringComparison.OrdinalIgnoreCase);
            }
            return num;
        }

        private IEnumerable<uint> GetChildren()
        {
            return new <GetChildren>d__0(-2) { <>4__this = this };
        }

        public IEnumerable<CompoundEntry> GetEntries()
        {
            return new <GetEntries>d__5(-2) { <>4__this = this };
        }

        public Guid Clsid
        {
            get
            {
                return base.Entry._clsId;
            }
        }

        public CompoundEntry this[string name]
        {
            get
            {
                uint index = base.Entry._sidChild;
                if (index == uint.MaxValue)
                {
                    return null;
                }
                for (int i = CompareNames(name, base.Directory[index]._ab); i != 0; i = CompareNames(name, base.Directory[index]._ab))
                {
                    if (i < 0)
                    {
                        index = base.Directory[index]._sidLeftSib;
                    }
                    else
                    {
                        index = base.Directory[index]._sidRightSib;
                    }
                    if (index == uint.MaxValue)
                    {
                        return null;
                    }
                }
                return CompoundEntry.CreateEntry(base.Owner, base.Directory, index);
            }
        }

        public CompoundStorageEntry Parent
        {
            get
            {
                return base.GetParentStorage();
            }
        }

        public CompoundStorageEntry Root
        {
            get
            {
                if (base.Entry._mse == STGTY.STGTY_ROOT)
                {
                    return null;
                }
                return base.Owner.Root;
            }
        }

        [CompilerGenerated]
        private sealed class <GetChildren>d__0 : IEnumerable<uint>, IEnumerable, IEnumerator<uint>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private uint <>2__current;
            public CompoundStorageEntry <>4__this;
            private int <>l__initialThreadId;
            public uint <DID>5__2;
            public Stack<uint> <StorageTree>5__1;

            [DebuggerHidden]
            public <GetChildren>d__0(int <>1__state)
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
                        this.<StorageTree>5__1 = new Stack<uint>();
                        this.<StorageTree>5__1.Push(this.<>4__this.Entry._sidChild);
                        while (this.<StorageTree>5__1.Count > 0)
                        {
                            this.<DID>5__2 = this.<StorageTree>5__1.Pop();
                            this.<>2__current = this.<DID>5__2;
                            this.<>1__state = 1;
                            return true;
                        Label_0080:
                            this.<>1__state = -1;
                            if (this.<>4__this.Directory[this.<DID>5__2]._sidLeftSib != uint.MaxValue)
                            {
                                this.<StorageTree>5__1.Push(this.<>4__this.Directory[this.<DID>5__2]._sidLeftSib);
                            }
                            if (this.<>4__this.Directory[this.<DID>5__2]._sidRightSib != uint.MaxValue)
                            {
                                this.<StorageTree>5__1.Push(this.<>4__this.Directory[this.<DID>5__2]._sidRightSib);
                            }
                        }
                        break;

                    case 1:
                        goto Label_0080;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new CompoundStorageEntry.<GetChildren>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.UInt32>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            uint IEnumerator<uint>.Current
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
        private sealed class <GetEntries>d__5 : IEnumerable<CompoundEntry>, IEnumerable, IEnumerator<CompoundEntry>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private CompoundEntry <>2__current;
            public CompoundStorageEntry <>4__this;
            public IEnumerator<uint> <>7__wrap8;
            private int <>l__initialThreadId;
            public uint <NextDID>5__6;
            public CompoundEntry <TreeEntry>5__7;

            [DebuggerHidden]
            public <GetEntries>d__5(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally9()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap8 != null)
                {
                    this.<>7__wrap8.Dispose();
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
                            this.<>7__wrap8 = this.<>4__this.GetChildren().GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap8.MoveNext())
                            {
                                this.<NextDID>5__6 = this.<>7__wrap8.Current;
                                this.<TreeEntry>5__7 = CompoundEntry.CreateEntry(this.<>4__this.Owner, this.<>4__this.Directory, this.<NextDID>5__6);
                                if (this.<TreeEntry>5__7 == null)
                                {
                                    goto Label_00B3;
                                }
                                this.<>2__current = this.<TreeEntry>5__7;
                                this.<>1__state = 2;
                                return true;
                            Label_00AC:
                                this.<>1__state = 1;
                            Label_00B3:;
                            }
                            this.<>m__Finally9();
                            break;

                        case 2:
                            goto Label_00AC;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<CompoundEntry> IEnumerable<CompoundEntry>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new CompoundStorageEntry.<GetEntries>d__5(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Commons.IO.CompoundEntry>.GetEnumerator();
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
                            this.<>m__Finally9();
                        }
                        break;
                }
            }

            CompoundEntry IEnumerator<CompoundEntry>.Current
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

