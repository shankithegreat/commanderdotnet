namespace Nomad.FileSystem.Shell
{
    using Microsoft.Shell;
    using Nomad;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class ShellFolder : CustomShellItem, IVirtualFolderUI, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, IGetVirtualRoot
    {
        internal ShellFolder(SafeShellItem item) : base(item)
        {
        }

        protected ShellFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal ShellFolder(SafeShellItem item, SFGAO attributes, IVirtualFolder parent) : base(item, attributes, parent)
        {
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            bool hasVolumeInfo = this.HasVolumeInfo;
            set[0x1a] = hasVolumeInfo;
            set[0x1b] = hasVolumeInfo;
            set[0x1c] = hasVolumeInfo;
            return set;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return ShellContextMenuHelper.CreateContextMenu(owner, this.GetItemsContextMenu(owner, items), options, onExecuteVerb);
        }

        internal static CustomShellItem CreateShellItem(SafeShellItem item, SFGAO attributes, IVirtualFolder parent)
        {
            if ((attributes & SFGAO.SFGAO_FOLDER) > ((SFGAO) 0))
            {
                return new ShellFolder(item, attributes, parent);
            }
            if ((attributes & SFGAO.SFGAO_STREAM) > ((SFGAO) 0))
            {
                return new ChangeShellFile(item, attributes, parent);
            }
            return new ShellFile(item, attributes, parent);
        }

        public void Dispose()
        {
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            return this.GetContent(SHCONTF.SHCONTF_FOLDERS | SHCONTF.SHCONTF_NONFOLDERS);
        }

        private IEnumerable<IVirtualItem> GetContent(SHCONTF grfFlags)
        {
            return new <GetContent>d__0(-2) { <>4__this = this, <>3__grfFlags = grfFlags };
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__b(-2) { <>4__this = this };
        }

        private IContextMenu GetItemsContextMenu(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            IContextMenu menu;
            List<IntPtr> list = new List<IntPtr>();
            foreach (IVirtualItem item in items)
            {
                CustomShellItem item2 = item as CustomShellItem;
                if (item2 == null)
                {
                    throw new ArgumentException("One of the items is not CustomShellItem");
                }
                list.Add(item2.ItemInfo.RelativePidl);
            }
            IShellFolder folder = base.ItemInfo.BindToFolder();
            try
            {
                menu = folder.GetUIObjectOf<IContextMenu>(owner.Handle, list.ToArray());
            }
            finally
            {
                Marshal.ReleaseComObject(folder);
            }
            return menu;
        }

        public bool IsChild(IVirtualItem item)
        {
            CustomShellItem item2 = item as CustomShellItem;
            if (item2 != null)
            {
                int depth = ITEMIDLIST.GetDepth(base.ItemInfo.AbsolutePidl);
                return ITEMIDLIST.Equals(base.ItemInfo.AbsolutePidl, item2.ItemInfo.AbsolutePidl, depth);
            }
            return false;
        }

        public void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            ShellContextMenuHelper.ExecuteVerb(owner, "properties", null, this.GetItemsContextMenu(owner, items));
        }

        private bool HasVolumeInfo
        {
            get
            {
                if (base.CheckItemAttribute(SFGAO.SFGAO_FILESYSTEM))
                {
                    string displayNameOf = base.ItemInfo.GetDisplayNameOf(SHGNO.SHGDN_FORPARSING);
                    if (displayNameOf.StartsWith("::", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    return (string.Equals(Path.GetPathRoot(displayNameOf), displayNameOf, StringComparison.OrdinalIgnoreCase) && (base.ItemInfo.Folder is IShellFolder2));
                }
                return false;
            }
        }

        public override object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0x1a:
                        return CustomShellItem.GetShellProperty(base.ItemInfo, SHCOLUMNID.PropertySetVolume, 3);

                    case 0x1b:
                        return CustomShellItem.GetShellProperty(base.ItemInfo, SHCOLUMNID.PropertySetVolume, 2);

                    case 0x1c:
                        return CustomShellItem.GetShellProperty(base.ItemInfo, SHCOLUMNID.PropertySetVolume, 4);
                }
                return base[property];
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                IVirtualFolder folder;
                ITEMIDLIST itemidlist = ITEMIDLIST.FromPidl(base.ItemInfo.AbsolutePidl);
                if (itemidlist.mkid.Length == 1)
                {
                    return this;
                }
                IntPtr pidl = Marshal.AllocCoTaskMem(itemidlist.GetSize(1));
                try
                {
                    itemidlist.ToPidl(pidl, 1);
                    folder = new ShellFolder(new SafeShellItem(pidl));
                }
                catch
                {
                    Marshal.FreeCoTaskMem(pidl);
                    throw;
                }
                return folder;
            }
        }

        public override string ToolTip
        {
            get
            {
                if (this.HasVolumeInfo)
                {
                    object obj2 = this[0x1a];
                    if (obj2 != null)
                    {
                        StringBuilder builder = new StringBuilder(base.Extender.ToolTip);
                        builder.AppendLine();
                        builder.Append(Resources.sToolTipFreeSpace);
                        builder.AppendLine(SizeTypeConverter.FormatSize<long>(Convert.ToInt64(this[0x1b]), SizeFormat.Dynamic));
                        builder.Append(Resources.sToolTipTotalSize);
                        builder.Append(SizeTypeConverter.FormatSize<long>(Convert.ToInt64(obj2), SizeFormat.Dynamic));
                        return builder.ToString();
                    }
                }
                return base.Extender.ToolTip;
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__0 : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public SHCONTF <>3__grfFlags;
            public ShellFolder <>4__this;
            public IEnumerator<IntPtr> <>7__wrap7;
            private int <>l__initialThreadId;
            public SafeShellItem <ChildItem>5__5;
            public IShellFolder <Folder>5__2;
            public SFGAO <ItemAttributes>5__4;
            public IntPtr <NextItem>5__3;
            public ITEMIDLIST <ParentIdList>5__1;
            public SHCONTF grfFlags;

            [DebuggerHidden]
            public <GetContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally6()
            {
                this.<>1__state = -1;
                Marshal.ReleaseComObject(this.<Folder>5__2);
            }

            private void <>m__Finally8()
            {
                this.<>1__state = 1;
                if (this.<>7__wrap7 != null)
                {
                    this.<>7__wrap7.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 5)
                        {
                            goto Label_0248;
                        }
                        goto Label_021C;
                    }
                    this.<>1__state = -1;
                    this.<ParentIdList>5__1 = ITEMIDLIST.FromPidl(this.<>4__this.ItemInfo.AbsolutePidl);
                    SHITEMID[] itemIdList = new SHITEMID[] { new SHITEMID() };
                    this.<ParentIdList>5__1.Append(itemIdList);
                    this.<Folder>5__2 = this.<>4__this.ItemInfo.BindToFolder();
                    this.<>1__state = 1;
                    this.<>7__wrap7 = this.<Folder>5__2.GetObjects(IntPtr.Zero, this.grfFlags).GetEnumerator();
                    this.<>1__state = 2;
                    while (this.<>7__wrap7.MoveNext())
                    {
                        this.<NextItem>5__3 = this.<>7__wrap7.Current;
                        this.<ItemAttributes>5__4 = SFGAO.SFGAO_FOLDER | SFGAO.SFGAO_STREAM;
                        try
                        {
                            IntPtr[] apidl = new IntPtr[] { this.<NextItem>5__3 };
                            this.<Folder>5__2.GetAttributesOf(1, apidl, ref this.<ItemAttributes>5__4);
                            if (!this.<ParentIdList>5__1.IsEmpty)
                            {
                                ITEMIDLIST itemidlist = ITEMIDLIST.FromPidl(this.<NextItem>5__3);
                                Debug.Assert(itemidlist.mkid != null, "mkid is null");
                                Debug.Assert(itemidlist.mkid.Length == 1, "mkid.length != 1");
                                this.<ParentIdList>5__1.mkid[this.<ParentIdList>5__1.mkid.Length - 1] = itemidlist.mkid[0];
                                IntPtr pidl = Marshal.AllocCoTaskMem(this.<ParentIdList>5__1.Size);
                                try
                                {
                                    this.<ParentIdList>5__1.ToPidl(pidl);
                                    this.<ChildItem>5__5 = new SafeShellItem(pidl);
                                }
                                catch
                                {
                                    Marshal.FreeCoTaskMem(pidl);
                                    throw;
                                }
                            }
                            else
                            {
                                this.<ChildItem>5__5 = new SafeShellItem(this.<Folder>5__2, this.<NextItem>5__3);
                            }
                        }
                        finally
                        {
                            Marshal.FreeCoTaskMem(this.<NextItem>5__3);
                        }
                        this.<>2__current = ShellFolder.CreateShellItem(this.<ChildItem>5__5, this.<ItemAttributes>5__4, this.<>4__this);
                        this.<>1__state = 5;
                        return true;
                    Label_021C:
                        this.<>1__state = 2;
                    }
                    this.<>m__Finally8();
                    this.<>m__Finally6();
                Label_0248:
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
                ShellFolder.<GetContent>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ShellFolder.<GetContent>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.grfFlags = this.<>3__grfFlags;
                return d__;
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
                    case 2:
                    case 5:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 2:
                                case 5:
                                    try
                                    {
                                    }
                                    finally
                                    {
                                        this.<>m__Finally8();
                                    }
                                    break;
                            }
                        }
                        finally
                        {
                            this.<>m__Finally6();
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

        [CompilerGenerated]
        private sealed class <GetFolders>d__b : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public ShellFolder <>4__this;
            public IEnumerator<IVirtualItem> <>7__wrapd;
            private int <>l__initialThreadId;
            public IVirtualItem <NextItem>5__c;

            [DebuggerHidden]
            public <GetFolders>d__b(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallye()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapd != null)
                {
                    this.<>7__wrapd.Dispose();
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
                            this.<>7__wrapd = this.<>4__this.GetContent(SHCONTF.SHCONTF_FOLDERS).GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrapd.MoveNext())
                            {
                                this.<NextItem>5__c = this.<>7__wrapd.Current;
                                this.<>2__current = (IVirtualFolder) this.<NextItem>5__c;
                                this.<>1__state = 2;
                                return true;
                            Label_0078:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finallye();
                            break;

                        case 2:
                            goto Label_0078;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IVirtualFolder> IEnumerable<IVirtualFolder>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new ShellFolder.<GetFolders>d__b(0) { <>4__this = this.<>4__this };
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
                }
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

