namespace Nomad.FileSystem.Archive
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ArchiveFormatRoot : IVirtualFolder, IDisposable, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IGetVirtualRoot
    {
        private IChangeVirtualFile FArchiveFile;
        private IVirtualItemUI FArchiveUI;
        private List<IVirtualItem> FContent;
        private string FFullName;
        private IVirtualFolder FParent;

        public ArchiveFormatRoot(IEnumerable<FindFormatResult> formatList, IChangeVirtualFile archiveFile, IVirtualFolder parent)
        {
            this.FArchiveFile = archiveFile;
            this.FArchiveUI = this.FArchiveFile as IVirtualItemUI;
            this.FContent = new List<IVirtualItem>();
            foreach (FindFormatResult result in formatList)
            {
                this.FContent.Add(new ArchiveFormatItem(result, archiveFile, parent));
            }
            this.FParent = (parent == null) ? archiveFile.Parent : parent;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return ((this.FArchiveUI != null) ? this.FArchiveUI.CreateContextMenuStrip(owner, options, onExecuteVerb) : null);
        }

        public void Dispose()
        {
        }

        public bool Equals(IVirtualItem other)
        {
            return false;
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return ((this.FArchiveUI != null) ? this.FArchiveUI.ExecuteVerb(owner, verb) : false);
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            return this.FContent;
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__0(-2) { <>4__this = this };
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            return ((this.FArchiveUI != null) ? this.FArchiveUI.GetIcon(size, style) : null);
        }

        public PropertyAvailability GetPropertyAvailability(int property)
        {
            switch (property)
            {
                case 0:
                case 6:
                    return PropertyAvailability.Normal;
            }
            return PropertyAvailability.None;
        }

        public bool IsChild(IVirtualItem Item)
        {
            return false;
        }

        public void ShowProperties(IWin32Window owner)
        {
            if (this.FArchiveUI != null)
            {
                this.FArchiveUI.ShowProperties(owner);
            }
        }

        public FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Directory;
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                return DefaultProperty.NameAttrPropertySet;
            }
        }

        public string FullName
        {
            get
            {
                if (this.FFullName == null)
                {
                    this.FFullName = new Uri(this.FArchiveFile.FullName).ToString() + '#';
                }
                return this.FFullName;
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return ((this.FArchiveUI != null) ? this.FArchiveUI.Highlighter : null);
            }
        }

        public object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0:
                        return this.Name;

                    case 6:
                        return this.Attributes;
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                return this.FArchiveFile.Name;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.FParent;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                IGetVirtualRoot parent = this.FArchiveFile.Parent as IGetVirtualRoot;
                return ((parent != null) ? parent.Root : null);
            }
        }

        public string ShortName
        {
            get
            {
                return this.Name;
            }
        }

        public string ToolTip
        {
            get
            {
                return ((this.FArchiveUI != null) ? this.FArchiveUI.ToolTip : null);
            }
        }

        [CompilerGenerated]
        private sealed class <GetFolders>d__0 : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public ArchiveFormatRoot <>4__this;
            private int <>l__initialThreadId;

            [DebuggerHidden]
            public <GetFolders>d__0(int <>1__state)
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
                return new ArchiveFormatRoot.<GetFolders>d__0(0) { <>4__this = this.<>4__this };
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

