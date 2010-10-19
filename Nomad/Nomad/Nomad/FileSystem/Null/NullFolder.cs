namespace Nomad.FileSystem.Null
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class NullFolder : NullItem, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, ICreateVirtualFile, ICreateVirtualFolder
    {
        internal NullFolder(string name) : base(name)
        {
        }

        public IChangeVirtualFile CreateFile(string name)
        {
            return new NullFile(name);
        }

        public IVirtualFolder CreateFolder(string name)
        {
            return new NullFolder(name);
        }

        public void Dispose()
        {
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            return new <GetContent>d__0(-2) { <>4__this = this };
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__3(-2) { <>4__this = this };
        }

        public bool IsChild(IVirtualItem Item)
        {
            return (Item is NullItem);
        }

        public override FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Directory;
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__0 : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public NullFolder <>4__this;
            private int <>l__initialThreadId;

            [DebuggerHidden]
            public <GetContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                if (this.<>1__state == 0)
                {
                    this.<>1__state = -1;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItem> IEnumerable<IVirtualItem>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new NullFolder.<GetContent>d__0(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualItem>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            IVirtualItem IEnumerator<IVirtualItem>.Current
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
        private sealed class <GetFolders>d__3 : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public NullFolder <>4__this;
            private int <>l__initialThreadId;

            [DebuggerHidden]
            public <GetFolders>d__3(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private bool MoveNext()
            {
                if (this.<>1__state == 0)
                {
                    this.<>1__state = -1;
                }
                return false;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualFolder> IEnumerable<IVirtualFolder>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new NullFolder.<GetFolders>d__3(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Virtual.IVirtualFolder>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            IVirtualFolder IEnumerator<IVirtualFolder>.Current
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

