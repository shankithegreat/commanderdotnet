namespace Nomad.FileSystem.Archive
{
    using Microsoft.IO;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class ArchiveFolder : CustomArchiveItem, IVirtualCachedFolder, IVirtualItemUI, IVirtualFolderUI, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, IGetVirtualRoot
    {
        private FileSystemWatcher ArchiveWatcher;
        private System.Threading.Timer DeleteArchiveTimer;
        private const string EntryFolderName = "FolderName";
        private IEnumerable<ISimpleItem> FArchiveContent;
        private long FCompressedSize;
        private WeakReference FContent;
        private readonly string FFolderName;
        private string FName;
        private IVirtualFolder FParent;
        private long FSize;
        private bool FSizeAvailable;
        private CancelEventHandler QueryRemoveHandler;
        private EventHandler<VolumeEventArgs> VolumeRemovedHandler;

        public event EventHandler CachedContentChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        private event EventHandler<VirtualItemChangedEventArgs> FOnChanged;

        public event EventHandler<VirtualItemChangedEventArgs> OnChanged
        {
            add
            {
                this.FOnChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Combine(this.FOnChanged, value);
                this.EnableWatcher();
            }
            remove
            {
                this.FOnChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Remove(this.FOnChanged, value);
                if (this.FOnChanged == null)
                {
                    this.DisposeWatcher(true);
                    this.Flush();
                }
            }
        }

        protected ArchiveFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.FArchiveContent = CustomArchiveItem.GetDeserializationContent(base.FArchiveUri, base.FFormatInfo);
            string str = info.GetString("ItemName");
            if (!string.IsNullOrEmpty(str))
            {
                foreach (ISimpleItem item in this.FArchiveContent)
                {
                    if (str.Equals(item.Name, CustomArchiveItem.ComparisonRule))
                    {
                        base.FItem = item;
                        this.FFolderName = PathHelper.IncludeTrailingDirectorySeparator(base.FItem.Name);
                        return;
                    }
                }
            }
            this.FFolderName = info.GetString("FolderName");
        }

        public ArchiveFolder(ISimpleItem item, Uri archiveUri, IEnumerable<ISimpleItem> archiveContent, IVirtualFolder parent) : base(item, archiveUri, ((IGetArchiveFormatInfo) archiveContent).FormatInfo)
        {
            this.FFolderName = PathHelper.IncludeTrailingDirectorySeparator(base.FItem.Name);
            this.FArchiveContent = archiveContent;
            this.FParent = parent;
        }

        public ArchiveFolder(string folderName, Uri archiveUri, IEnumerable<ISimpleItem> archiveContent, IVirtualFolder parent) : base(null, archiveUri, ((IGetArchiveFormatInfo) archiveContent).FormatInfo)
        {
            this.FFolderName = PathHelper.IncludeTrailingDirectorySeparator(folderName);
            this.FArchiveContent = archiveContent;
            this.FParent = parent;
        }

        public void CalculateFolderSize()
        {
            foreach (ISimpleItem item in this.ArchiveContent)
            {
                if (item.Name.StartsWith(this.FFolderName, CustomArchiveItem.ComparisonRule))
                {
                    this.FSize += Convert.ToInt64(item[3]);
                    this.FCompressedSize += Convert.ToInt64(item[5]);
                }
            }
            this.FSizeAvailable = true;
            base.Extender.ToolTip = null;
            base.ResetAvailableSet();
            ArchiveFolder fParent = this.FParent as ArchiveFolder;
            if (fParent != null)
            {
                fParent.RaiseChanged(this, new VirtualPropertySet(new int[] { 3, 5 }));
            }
        }

        public void ClearContentCache()
        {
            IGetArchiveFormatInfo archiveContent = this.ArchiveContent as IGetArchiveFormatInfo;
            if (!((archiveContent == null) || archiveContent.RefreshContent()))
            {
                throw new ApplicationException("Refresh archive content failed.");
            }
            this.FContent = null;
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[6] = true;
            set[3] = this.FSizeAvailable;
            set[5] = this.FSizeAvailable;
            set[ArchiveProperty.ArchiveFormat] = string.IsNullOrEmpty(this.FFolderName);
            return set;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            if (string.IsNullOrEmpty(this.FFolderName) && base.FArchiveUri.IsFile)
            {
                return ShellContextMenuHelper.CreateContextMenu(owner, new string[] { base.FArchiveUri.LocalPath }, options, onExecuteVerb);
            }
            return null;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        private void DestroyDeleteTimer()
        {
            if (this.DeleteArchiveTimer != null)
            {
                this.DeleteArchiveTimer.Dispose();
            }
            this.DeleteArchiveTimer = null;
        }

        public void Dispose()
        {
            this.DisposeWatcher(true);
            if (this.FArchiveContent != null)
            {
                CustomArchiveItem.RememberDeserializationContext(base.FArchiveUri, this.FArchiveContent);
            }
            this.FArchiveContent = null;
            base.FItem = null;
            this.FParent = null;
        }

        private void DisposeWatcher(bool disposing)
        {
            if (this.ArchiveWatcher != null)
            {
                this.ArchiveWatcher.Dispose();
            }
            this.ArchiveWatcher = null;
            if (this.QueryRemoveHandler != null)
            {
                VolumeEvents.UnregisterRemovingEvent(Path.GetPathRoot(base.FArchiveUri.LocalPath), this.QueryRemoveHandler);
            }
            this.QueryRemoveHandler = null;
            if (disposing)
            {
                if (this.VolumeRemovedHandler != null)
                {
                    VolumeEvents.Removed -= this.VolumeRemovedHandler;
                }
                this.VolumeRemovedHandler = null;
            }
        }

        private void EnableWatcher()
        {
            this.InitializeWatcher();
            if (this.QueryRemoveHandler != null)
            {
                VolumeEvents.RegisterRemovingEvent(Path.GetPathRoot(base.FArchiveUri.LocalPath), this.QueryRemoveHandler);
            }
            if (this.VolumeRemovedHandler != null)
            {
                VolumeEvents.Removed += this.VolumeRemovedHandler;
            }
            if (this.ArchiveWatcher != null)
            {
                try
                {
                    this.ArchiveWatcher.EnableRaisingEvents = true;
                }
                catch (ArgumentException)
                {
                    this.ArchiveWatcher.Dispose();
                    this.ArchiveWatcher = null;
                }
                catch (FileNotFoundException)
                {
                    this.ArchiveWatcher.Dispose();
                    this.ArchiveWatcher = null;
                }
            }
        }

        public override bool Equals(IVirtualItem other)
        {
            ArchiveFolder folder = other as ArchiveFolder;
            return (((folder != null) && base.Equals(other)) && string.Equals(this.FFolderName, folder.FFolderName, CustomArchiveItem.ComparisonRule));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            if (string.IsNullOrEmpty(this.FFolderName) && base.FArchiveUri.IsFile)
            {
                LocalFileSystemCreator.ExecuteVerb(owner, base.FArchiveUri.LocalPath, verb);
                return true;
            }
            return false;
        }

        private void Flush()
        {
            IFlushStream fArchiveContent = this.FArchiveContent as IFlushStream;
            if (fArchiveContent != null)
            {
                fArchiveContent.Flush();
            }
        }

        public IVirtualItem FromName(string name)
        {
            return CustomArchiveItem.FromName(base.FArchiveUri, this.ArchiveContent, name);
        }

        public IEnumerable<IVirtualItem> GetCachedContent()
        {
            return this.GetContent();
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            List<IVirtualItem> content = this.Content;
            if (content == null)
            {
                content = new List<IVirtualItem>(this.GetContent(this.FFolderName));
                this.Content = content;
            }
            return content;
        }

        private IEnumerable<IVirtualItem> GetContent(string folderName)
        {
            return new <GetContent>d__0(-2) { <>4__this = this, <>3__folderName = folderName };
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return this.GetContent().OfType<IVirtualFolder>();
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            if ((!base.Extender.HasIcon(size) && string.IsNullOrEmpty(this.FFolderName)) && base.FArchiveUri.IsFile)
            {
                Image fileIcon = ImageProvider.Default.GetFileIcon(base.FArchiveUri.LocalPath, size);
                base.Extender.SetIcon(fileIcon, size);
                return fileIcon;
            }
            return base.Extender.GetIcon(size, (style & IconStyle.CanUseAlphaBlending) > 0);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (base.FItem == null)
            {
                info.AddValue("FolderName", this.FFolderName);
            }
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 3:
                case 5:
                    if (!this.FSizeAvailable)
                    {
                        this.CalculateFolderSize();
                    }
                    return ((propertyId == 3) ? this.FSize : this.FCompressedSize);
            }
            if (propertyId == ArchiveProperty.ArchiveFormat)
            {
                return base.FFormatInfo;
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            if (propertyId == 6)
            {
                return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public bool HasChildren()
        {
            return (this.GetContent(this.FFolderName).FirstOrDefault<IVirtualItem>() != null);
        }

        private void InitializeWatcher()
        {
            if (base.FArchiveUri.IsFile)
            {
                string directoryName = Path.GetDirectoryName(base.FArchiveUri.LocalPath);
                if ((this.ArchiveWatcher == null) && (Environment.OSVersion.Platform == PlatformID.Win32NT))
                {
                    this.ArchiveWatcher = new FileSystemWatcher();
                    try
                    {
                        this.ArchiveWatcher.BeginInit();
                        this.ArchiveWatcher.Path = directoryName;
                        this.ArchiveWatcher.Filter = Path.GetFileName(base.FArchiveUri.LocalPath);
                        this.ArchiveWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                        this.ArchiveWatcher.Changed += new FileSystemEventHandler(this.OnWatcherChanged);
                        this.ArchiveWatcher.Deleted += new FileSystemEventHandler(this.OnWatcherDeleted);
                        this.ArchiveWatcher.Renamed += new RenamedEventHandler(this.OnWatcherRenamed);
                        this.ArchiveWatcher.EndInit();
                    }
                    catch (ArgumentException)
                    {
                        this.ArchiveWatcher.Dispose();
                        this.ArchiveWatcher = null;
                    }
                }
                if ((this.QueryRemoveHandler == null) && (this.VolumeRemovedHandler == null))
                {
                    try
                    {
                        VolumeInfo info = VolumeCache.FromPath(directoryName);
                        if ((info != null) && info.CanEject)
                        {
                            if (!((this.QueryRemoveHandler != null) || info.IsReadOnly))
                            {
                                this.QueryRemoveHandler = new CancelEventHandler(this.OnQueryRemove);
                            }
                            if (this.VolumeRemovedHandler == null)
                            {
                                this.VolumeRemovedHandler = new EventHandler<VolumeEventArgs>(this.OnVolumeRemoved);
                            }
                        }
                    }
                    catch (IOException)
                    {
                        this.QueryRemoveHandler = null;
                        this.VolumeRemovedHandler = null;
                    }
                }
            }
        }

        public bool IsChild(IVirtualItem Item)
        {
            return (((Item is ArchiveItem) || (Item is ArchiveFolder)) && Item.FullName.StartsWith(this.FullName, CustomArchiveItem.ComparisonRule));
        }

        private void OnQueryRemove(object source, CancelEventArgs e)
        {
            this.DisposeWatcher(false);
            this.Flush();
        }

        private void OnVolumeRemoved(object source, VolumeEventArgs e)
        {
            if (base.FArchiveUri.LocalPath.StartsWith(e.DriveChar + ":", CustomArchiveItem.ComparisonRule))
            {
                this.RaiseChanged(WatcherChangeTypes.Deleted, this);
            }
        }

        private void OnWatcherChanged(object source, FileSystemEventArgs e)
        {
            bool flag = false;
            try
            {
                this.ClearContentCache();
            }
            catch (ApplicationException)
            {
                flag = true;
            }
            catch (Exception exception)
            {
                flag = true;
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
            if (flag)
            {
                this.RaiseChanged(WatcherChangeTypes.Deleted, this);
            }
            else
            {
                this.Content = null;
                this.RaiseChanged(WatcherChangeTypes.All, null);
            }
        }

        private void OnWatcherDeleted(object source, FileSystemEventArgs e)
        {
            this.DeleteArchiveTimer = new System.Threading.Timer(new TimerCallback(this.WatcherDeleteTimer), null, 100, -1);
        }

        private void OnWatcherRenamed(object source, RenamedEventArgs e)
        {
            string localPath = base.FArchiveUri.LocalPath;
            if (string.Equals(e.FullPath, localPath, StringComparison.OrdinalIgnoreCase))
            {
                this.DestroyDeleteTimer();
                Thread.Sleep(100);
                this.OnWatcherChanged(source, e);
            }
            else if (string.Equals(e.OldFullPath, localPath, StringComparison.OrdinalIgnoreCase))
            {
                this.RaiseChanged(WatcherChangeTypes.Deleted, this);
            }
        }

        private void RaiseChanged(IVirtualItem item, VirtualPropertySet propertySet)
        {
            if (this.FOnChanged != null)
            {
                this.FOnChanged(this, new VirtualItemChangedEventArgs(item, propertySet));
            }
        }

        public void RaiseChanged(WatcherChangeTypes changeType, IVirtualItem item)
        {
            if (this.FOnChanged != null)
            {
                this.FOnChanged(this, new VirtualItemChangedEventArgs(changeType, item));
            }
        }

        public void ShowProperties(IWin32Window owner)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(this.FFolderName) && base.FArchiveUri.IsFile)
            {
                flag = ShellContextMenuHelper.ExecuteVerb(owner, "properties", new string[] { base.FArchiveUri.LocalPath });
            }
            if (!flag)
            {
                using (PropertiesDialog dialog = new PropertiesDialog())
                {
                    dialog.Execute(owner, new IVirtualItem[] { this });
                }
            }
        }

        public void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            using (PropertiesDialog dialog = new PropertiesDialog())
            {
                dialog.Execute(owner, items);
            }
        }

        private void WatcherDeleteTimer(object state)
        {
            this.DestroyDeleteTimer();
            this.RaiseChanged(WatcherChangeTypes.Deleted, this);
        }

        protected IEnumerable<ISimpleItem> ArchiveContent
        {
            get
            {
                if (this.FArchiveContent == null)
                {
                    this.FArchiveContent = CustomArchiveItem.GetDeserializationContent(base.FArchiveUri, base.FFormatInfo);
                    if ((base.FItem == null) && !string.IsNullOrEmpty(this.FFolderName))
                    {
                        foreach (ISimpleItem item in this.FArchiveContent)
                        {
                            if (this.FFolderName.Equals(PathHelper.IncludeTrailingDirectorySeparator(item.Name), CustomArchiveItem.ComparisonRule) && ((((FileAttributes) item[6]) & FileAttributes.Directory) > 0))
                            {
                                base.FItem = item;
                            }
                        }
                    }
                }
                return this.FArchiveContent;
            }
        }

        public override FileAttributes Attributes
        {
            get
            {
                return (FileAttributes.Directory | base.Attributes);
            }
        }

        public Nomad.FileSystem.Virtual.CacheState CacheState
        {
            get
            {
                Nomad.FileSystem.Virtual.CacheState hasContent = Nomad.FileSystem.Virtual.CacheState.HasContent;
                List<IVirtualItem> content = this.Content;
                if (content != null)
                {
                    foreach (IVirtualItem item in content)
                    {
                        hasContent |= Nomad.FileSystem.Virtual.CacheState.HasItems;
                        if (item is IVirtualFolder)
                        {
                            return (hasContent | Nomad.FileSystem.Virtual.CacheState.HasFolders);
                        }
                    }
                    return hasContent;
                }
                foreach (ISimpleItem item2 in this.ArchiveContent)
                {
                    hasContent |= Nomad.FileSystem.Virtual.CacheState.HasItems;
                    if ((((FileAttributes) item2[6]) & FileAttributes.Directory) > 0)
                    {
                        return (hasContent | Nomad.FileSystem.Virtual.CacheState.HasFolders);
                    }
                }
                return hasContent;
            }
        }

        private List<IVirtualItem> Content
        {
            get
            {
                if ((this.FContent != null) && this.FContent.IsAlive)
                {
                    return (List<IVirtualItem>) this.FContent.Target;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    this.FContent = null;
                }
                else
                {
                    this.FContent = new WeakReference(value);
                }
            }
        }

        public override string FullName
        {
            get
            {
                return (base.FArchiveUri.ToString() + '#' + this.FFolderName);
            }
        }

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this.FFolderName))
                {
                    this.FName = Uri.UnescapeDataString(Path.GetFileName(base.FArchiveUri.AbsolutePath));
                }
                if (this.FName == null)
                {
                    this.FName = ArchiveItem.GetFileName(this.FFolderName);
                }
                return this.FName;
            }
        }

        public override IVirtualFolder Parent
        {
            get
            {
                if (this.FParent == null)
                {
                    if (string.IsNullOrEmpty(this.FFolderName))
                    {
                        this.FParent = VirtualItem.FromFullName(base.FArchiveUri.ToString(), VirtualItemType.File).Parent;
                    }
                    else
                    {
                        this.FParent = new ArchiveFolder(Path.GetDirectoryName(PathHelper.ExcludeTrailingDirectorySeparator(this.FFolderName)), base.FArchiveUri, this.ArchiveContent, null);
                    }
                }
                return this.FParent;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                if (string.IsNullOrEmpty(this.FFolderName))
                {
                    return VirtualItemHelper.GetFolderRoot(this.Parent);
                }
                return new ArchiveFolder(string.Empty, base.FArchiveUri, this.ArchiveContent, null);
            }
        }

        public string ToolTip
        {
            get
            {
                return base.Extender.ToolTip;
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__0 : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public string <>3__folderName;
            public ArchiveFolder <>4__this;
            public IEnumerator<ISimpleItem> <>7__wrap7;
            public Dictionary<string, int>.KeyCollection.Enumerator <>7__wrap9;
            private int <>l__initialThreadId;
            public Dictionary<string, int> <AddedFolders>5__3;
            public IEqualityComparer<string> <Comparer>5__1;
            public int <DelimiterIndex>5__5;
            public string <FolderName>5__6;
            public Dictionary<string, int> <FolderNames>5__2;
            public ISimpleItem <Item>5__4;
            public string folderName;

            [DebuggerHidden]
            public <GetContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally8()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap7 != null)
                {
                    this.<>7__wrap7.Dispose();
                }
            }

            private void <>m__Finallya()
            {
                this.<>1__state = -1;
                this.<>7__wrap9.Dispose();
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
                            switch (CustomArchiveItem.ComparisonRule)
                            {
                                case StringComparison.Ordinal:
                                    this.<Comparer>5__1 = StringComparer.Ordinal;
                                    break;

                                case StringComparison.OrdinalIgnoreCase:
                                    this.<Comparer>5__1 = StringComparer.OrdinalIgnoreCase;
                                    break;
                            }
                            throw new ApplicationException("Cannot determine equality comparer.");

                        case 2:
                            goto Label_01AA;

                        case 3:
                            goto Label_01EF;

                        case 5:
                            goto Label_02B5;

                        default:
                            goto Label_02D3;
                    }
                    this.<FolderNames>5__2 = new Dictionary<string, int>(this.<Comparer>5__1);
                    this.<AddedFolders>5__3 = new Dictionary<string, int>(this.<Comparer>5__1);
                    this.<>7__wrap7 = this.<>4__this.ArchiveContent.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap7.MoveNext())
                    {
                        this.<Item>5__4 = this.<>7__wrap7.Current;
                        if (!this.<Item>5__4.Name.StartsWith(this.folderName, CustomArchiveItem.ComparisonRule))
                        {
                            continue;
                        }
                        this.<DelimiterIndex>5__5 = this.<Item>5__4.Name.IndexOf(Path.DirectorySeparatorChar, this.folderName.Length);
                        if (this.<DelimiterIndex>5__5 >= 0)
                        {
                            goto Label_01F9;
                        }
                        if ((((FileAttributes) this.<Item>5__4[6]) & FileAttributes.Directory) <= 0)
                        {
                            goto Label_01B4;
                        }
                        this.<AddedFolders>5__3.Add(this.<Item>5__4.Name, 0);
                        this.<>2__current = new ArchiveFolder(this.<Item>5__4, this.<>4__this.FArchiveUri, this.<>4__this.ArchiveContent, this.<>4__this);
                        this.<>1__state = 2;
                        return true;
                    Label_01AA:
                        this.<>1__state = 1;
                        goto Label_021D;
                    Label_01B4:
                        this.<>2__current = new ArchiveFile(this.<Item>5__4, this.<>4__this.FArchiveUri, this.<>4__this.FFormatInfo, this.<>4__this);
                        this.<>1__state = 3;
                        return true;
                    Label_01EF:
                        this.<>1__state = 1;
                        goto Label_021D;
                    Label_01F9:
                        this.<FolderNames>5__2[this.<Item>5__4.Name.Substring(0, this.<DelimiterIndex>5__5)] = 0;
                    Label_021D:;
                    }
                    this.<>m__Finally8();
                    this.<>7__wrap9 = this.<FolderNames>5__2.Keys.GetEnumerator();
                    this.<>1__state = 4;
                    while (this.<>7__wrap9.MoveNext())
                    {
                        this.<FolderName>5__6 = this.<>7__wrap9.Current;
                        if (this.<AddedFolders>5__3.ContainsKey(this.<FolderName>5__6))
                        {
                            continue;
                        }
                        this.<>2__current = new ArchiveFolder(this.<FolderName>5__6, this.<>4__this.FArchiveUri, this.<>4__this.ArchiveContent, this.<>4__this);
                        this.<>1__state = 5;
                        return true;
                    Label_02B5:
                        this.<>1__state = 4;
                    }
                    this.<>m__Finallya();
                Label_02D3:
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
                ArchiveFolder.<GetContent>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new ArchiveFolder.<GetContent>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.folderName = this.<>3__folderName;
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
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally8();
                        }
                        break;

                    case 4:
                    case 5:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallya();
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

