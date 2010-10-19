namespace Nomad.FileSystem.Virtual
{
    using Nomad;
    using Nomad.Commons.Collections;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class CustomAsyncFolder : EventBackgroundWorker
    {
        protected readonly object ContentLock = new object();
        private WaitHandle[] EventArray;
        private HashList<IVirtualItem> FContent;
        private Nomad.FileSystem.Virtual.CacheState FContentState;
        private EventWaitHandle NewItemEvent = new AutoResetEvent(false);

        public event EventHandler CachedContentChanged;

        internal CustomAsyncFolder()
        {
            this.EventArray = new WaitHandle[] { base.ExitThreadEvent, this.NewItemEvent };
        }

        public virtual IAsyncResult BeginGetContent()
        {
            this.RestartBackgroundThread();
            return new AsyncFolderResult(this, base.ExitThreadEvent);
        }

        protected void BeginUpdateContent()
        {
        }

        public virtual void ClearContentCache()
        {
            lock (this.ContentLock)
            {
                if (this.FContent == null)
                {
                    this.FContent = new HashList<IVirtualItem>();
                }
                else
                {
                    this.FContent.Clear();
                }
                this.FContentState = Nomad.FileSystem.Virtual.CacheState.HasContent;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Content = null;
        }

        protected override void DoWork()
        {
            this.BeginUpdateContent();
            try
            {
                this.PopulateContent();
            }
            finally
            {
                base.CancelAsync();
                this.EndUpdateContent();
            }
        }

        public IEnumerable<IVirtualItem> EndGetContent(IAsyncResult result)
        {
            AsyncFolderResult result2 = result as AsyncFolderResult;
            if ((result2 == null) || (result2.Folder != this))
            {
                throw new ArgumentException();
            }
            result.AsyncWaitHandle.WaitOne();
            if (base.ThreadError != null)
            {
                throw base.ThreadError;
            }
            return this.GetCachedContent();
        }

        protected void EndUpdateContent()
        {
        }

        public virtual IEnumerable<IVirtualItem> GetCachedContent()
        {
            IEnumerable<IVirtualItem> enumerable = null;
            lock (this.ContentLock)
            {
                if (this.Content != null)
                {
                    enumerable = new List<IVirtualItem>(this.Content);
                }
            }
            return (enumerable ?? ((IEnumerable<IVirtualItem>) new IVirtualItem[0]));
        }

        public virtual IEnumerable<IVirtualItem> GetContent()
        {
            return new <GetContent>d__0(-2) { <>4__this = this };
        }

        protected void NewItem(IVirtualItem item)
        {
            bool flag;
            lock (this.ContentLock)
            {
                Nomad.FileSystem.Virtual.CacheState fContentState = this.FContentState;
                this.FContentState |= Nomad.FileSystem.Virtual.CacheState.HasItems | Nomad.FileSystem.Virtual.CacheState.HasContent;
                if (item is IVirtualFolder)
                {
                    this.FContentState |= Nomad.FileSystem.Virtual.CacheState.HasFolders;
                }
                flag = fContentState != this.FContentState;
                this.FContent.Add(item);
            }
            this.NewItemEvent.Set();
            this.RaiseChanged(WatcherChangeTypes.Created, item);
            if (flag)
            {
                this.RaiseCacheContentChanged(EventArgs.Empty);
            }
        }

        protected abstract void PopulateContent();
        protected void RaiseCacheContentChanged(EventArgs e)
        {
            if (this.CachedContentChanged != null)
            {
                this.CachedContentChanged(this, e);
            }
        }

        protected abstract void RaiseChanged(VirtualItemChangedEventArgs e);
        public void RaiseChanged(WatcherChangeTypes changeType, IVirtualItem item)
        {
            this.RaiseChanged(new VirtualItemChangedEventArgs(changeType, item));
        }

        private void RestartBackgroundThread()
        {
            base.StopAsync();
            this.ClearContentCache();
            base.RunAsync(ThreadPriority.Normal);
        }

        public virtual Nomad.FileSystem.Virtual.CacheState CacheState
        {
            get
            {
                return this.FContentState;
            }
        }

        protected HashList<IVirtualItem> Content
        {
            get
            {
                return this.FContent;
            }
            set
            {
                lock (this.ContentLock)
                {
                    this.FContent = value;
                    if (this.FContent == null)
                    {
                        this.FContentState = Nomad.FileSystem.Virtual.CacheState.Unknown;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__0 : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public CustomAsyncFolder <>4__this;
            public object <>7__wrap4;
            private int <>l__initialThreadId;
            public int <Count>5__1;
            public int <I>5__2;

            [DebuggerHidden]
            public <GetContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
                this.<>4__this.EndUpdateContent();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    bool flag2;
                    int num = this.<>1__state;
                    if (num == 0)
                    {
                        this.<>1__state = -1;
                        this.<>4__this.RestartBackgroundThread();
                        this.<>4__this.BeginUpdateContent();
                        this.<>1__state = 1;
                        this.<Count>5__1 = 0;
                        this.<I>5__2 = 0;
                        goto Label_011A;
                    }
                    if (num != 3)
                    {
                        goto Label_0121;
                    }
                    goto Label_00E2;
                Label_0058:
                    Monitor.Enter(this.<>7__wrap4 = this.<>4__this.ContentLock);
                    try
                    {
                        this.<Count>5__1 = this.<>4__this.FContent.Count;
                    }
                    finally
                    {
                        Monitor.Exit(this.<>7__wrap4);
                    }
                    if (this.<I>5__2 >= this.<Count>5__1)
                    {
                        goto Label_00EB;
                    }
                    this.<>2__current = this.<>4__this.FContent[this.<I>5__2++];
                    this.<>1__state = 3;
                    return true;
                Label_00E2:
                    this.<>1__state = 1;
                    goto Label_011A;
                Label_00EB:
                    if (this.<>4__this.CancellationPending)
                    {
                        this.System.IDisposable.Dispose();
                        goto Label_0121;
                    }
                    WaitHandle.WaitAny(this.<>4__this.EventArray);
                Label_011A:
                    flag2 = true;
                    goto Label_0058;
                Label_0121:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItem> IEnumerable<IVirtualItem>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new CustomAsyncFolder.<GetContent>d__0(0) { <>4__this = this.<>4__this };
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
                switch (this.<>1__state)
                {
                    case 1:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally3();
                        }
                        break;
                }
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
    }
}

