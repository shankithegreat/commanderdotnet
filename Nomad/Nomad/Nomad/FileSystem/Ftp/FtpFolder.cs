namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable, DebuggerDisplay("{GetType().Name}, {ItemInfo.AbsoluteUri}")]
    public sealed class FtpFolder : CustomAsyncFolder, IAsyncVirtualFolder, IVirtualCachedFolder, IChangeVirtualItem, IPersistVirtualItem, ISetVirtualProperty, ICreateVirtualFile, ICreateVirtualFolder, IVirtualItemUI, IVirtualFolderUI, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, ICloneable, ICreateVirtualLink, ISetOwnerWindow, ISerializable, IGetVirtualVolume, IGetVirtualRoot
    {
        private const string EntryIsExists = "IsExists";
        private const string EntryItemInfo = "ItemInfo";
        private VirtualPropertySet FAvailableSet;
        private VirtualItemVisualExtender FExtender;
        private bool? FIsExists;
        private bool FToolTipAvailable;
        private EventHandler<FtpChangedEventArg> FtpChangedHandler;
        private static IDictionary<Size, Image> FtpRootIcons;
        internal readonly FtpItemInfo ItemInfo;
        private Uri UriWithSlash;

        public event EventHandler<VirtualItemChangedEventArgs> OnChanged;

        internal FtpFolder(FtpContext context, Uri absoluteUri) : this(context, absoluteUri, null)
        {
        }

        private FtpFolder(SerializationInfo info, StreamingContext context)
        {
            this.ItemInfo = (FtpItemInfo) info.GetValue("ItemInfo", typeof(FtpItemInfo));
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                if (current.Name == "IsExists")
                {
                    this.FIsExists = new bool?((bool) current.Value);
                    break;
                }
            }
            this.Initialize();
        }

        internal FtpFolder(FtpContext context, Uri absoluteUri, IVirtualFolder parent) : this(context, absoluteUri, null, parent)
        {
        }

        internal FtpFolder(FtpContext context, Uri absoluteUri, bool? isExists, IVirtualFolder parent)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }
            Uri baseUri = absoluteUri;
            if (!PathHelper.HasTrailingDirectorySeparator(baseUri.AbsolutePath))
            {
                baseUri = new Uri(baseUri, PathHelper.IncludeTrailingDirectorySeparator(baseUri.AbsolutePath));
            }
            this.FIsExists = isExists;
            this.ItemInfo = new FtpItemInfo(context, baseUri, parent);
            this.Initialize();
        }

        internal FtpFolder(FtpContext context, Uri baseUri, string name, DateTime lastWriteTime, IVirtualFolder parent)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }
            this.FIsExists = true;
            this.ItemInfo = new FtpItemInfo(context, baseUri, name, lastWriteTime, parent);
            Uri absoluteUri = this.ItemInfo.AbsoluteUri;
            if (!PathHelper.HasTrailingDirectorySeparator(absoluteUri.AbsolutePath))
            {
                this.ItemInfo.AbsoluteUri = new Uri(absoluteUri, PathHelper.IncludeTrailingDirectorySeparator(absoluteUri.AbsolutePath));
            }
            this.Initialize();
        }

        public override IAsyncResult BeginGetContent()
        {
            IEnumerable<IVirtualItem> enumerable;
            base.StopAsync();
            if (this.ItemInfo.Context.Cache.TryGetValue(this.ItemInfo.AbsoluteUri, out enumerable))
            {
                base.Content = new HashList<IVirtualItem>(enumerable);
                if (this.ItemInfo.Context.UseCache && this.ItemInfo.Context.UsePrefetch)
                {
                    FtpFolder parent = this.ItemInfo.Parent as FtpFolder;
                    List<Uri> state = new List<Uri>();
                    if (parent != null)
                    {
                        state.Add(parent.ItemInfo.AbsoluteUri);
                    }
                    foreach (IVirtualItem item in enumerable)
                    {
                        FtpFolder folder2 = item as FtpFolder;
                        if (folder2 != null)
                        {
                            state.Add(folder2.ItemInfo.AbsoluteUri);
                        }
                    }
                    if (state.Count > 0)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.PrefetchFolders), state);
                    }
                }
                return new AsyncFolderResult(this);
            }
            return base.BeginGetContent();
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            return ((destFolder is ICreateVirtualFile) ? LinkType.Default : LinkType.None);
        }

        public bool CanMoveTo(IVirtualFolder dest)
        {
            return false;
        }

        public bool CanSetProperty(int property)
        {
            return (property == 0);
        }

        public override void ClearContentCache()
        {
            this.ItemInfo.Context.Cache.Remove(this.ItemInfo.AbsoluteUri);
            base.RaiseCacheContentChanged(EventArgs.Empty);
            base.ClearContentCache();
        }

        public object Clone()
        {
            FtpFolder folder = (FtpFolder) base.MemberwiseClone();
            folder.Content = null;
            return folder;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public IChangeVirtualFile CreateFile(string name)
        {
            IVirtualItem item;
            name = StringHelper.ApplyCharacterCasing(name, this.ItemInfo.Context.UploadFileNameCasing);
            Uri uri = new Uri(this.ItemInfo.AbsoluteUri, Path.Combine(this.ItemInfo.AbsoluteUri.AbsolutePath, this.ItemInfo.Context.EncodeString(name)));
            try
            {
                long? nullable;
                DateTime dateTimestamp = this.ItemInfo.Context.GetDateTimestamp(uri);
                try
                {
                    nullable = new long?(this.ItemInfo.Context.GetFileSize(uri));
                }
                catch (WebException exception)
                {
                    if ((exception.Status != WebExceptionStatus.ProtocolError) || (((FtpWebResponse) exception.Response).StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable))
                    {
                        throw;
                    }
                    nullable = null;
                }
                item = new FtpFile(this.ItemInfo.Context, this.ItemInfo.AbsoluteUri, name, nullable, dateTimestamp, this);
            }
            catch (WebException exception2)
            {
                if ((exception2.Status != WebExceptionStatus.ProtocolError) || (((FtpWebResponse) exception2.Response).StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable))
                {
                    throw;
                }
                item = new FtpFile(this.ItemInfo.Context, uri, false, this);
            }
            if (!(item is IVirtualFile))
            {
                throw new WebException(string.Format(Resources.sFailedCreateFileBecauseFolderExists, this.ItemInfo.Context.DecodeString(uri.ToString())), WebExceptionStatus.UnknownError);
            }
            return (IChangeVirtualFile) item;
        }

        public IVirtualFolder CreateFolder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name is null or empty");
            }
            Uri uri = new Uri(name, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                uri = this.ItemInfo.AbsoluteUri.MakeRelativeUri(uri);
                if (uri.IsAbsoluteUri)
                {
                    throw new ArgumentException("Cannot create folder on different ftp host");
                }
            }
            string[] strArray = uri.ToString().Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            Uri absoluteUri = this.ItemInfo.AbsoluteUri;
            Uri uri3 = null;
            foreach (string str in strArray)
            {
                uri3 = new Uri(absoluteUri, Path.Combine(absoluteUri.AbsolutePath, this.ItemInfo.Context.EncodeString(str)));
                string b = Uri.UnescapeDataString(str);
                bool flag = false;
                foreach (string str3 in this.ItemInfo.Context.ListDirectory(absoluteUri))
                {
                    if (string.Equals(str3, b, StringComparison.Ordinal))
                    {
                        flag = true;
                        break;
                    }
                }
                absoluteUri = uri3;
                if (!flag)
                {
                    this.ItemInfo.Context.MakeDirectory(uri3);
                    FtpContext.RaiseFtpChangedEvent(WatcherChangeTypes.Created, uri3, null);
                }
            }
            return new FtpFolder(this.ItemInfo.Context, uri3, true, null);
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            if (type != LinkType.Default)
            {
                throw new ArgumentException("Unsupported link type", "type");
            }
            return this.ItemInfo.CreateShortCut(destFolder, name);
        }

        public void Delete(bool SendToBin)
        {
            this.ItemInfo.Context.RemoveDirectory(this.ItemInfo.AbsoluteUri);
            FtpContext.RaiseFtpChangedEvent(WatcherChangeTypes.Deleted, this.ItemInfo.AbsoluteUri, null);
            VirtualItem.RaiseVirtualItemChangedEvent(WatcherChangeTypes.Deleted, this);
        }

        public bool Equals(IVirtualItem other)
        {
            FtpFolder folder = other as FtpFolder;
            return ((folder != null) && this.ItemInfo.AbsoluteUri.Equals(folder.ItemInfo.AbsoluteUri));
        }

        public override bool Equals(object obj)
        {
            FtpFolder folder = obj as FtpFolder;
            if (folder != null)
            {
                return this.ItemInfo.AbsoluteUri.Equals(folder.ItemInfo.AbsoluteUri);
            }
            return base.Equals(obj);
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return this.ItemInfo.ExecuteVerb(owner, verb);
        }

        public override IEnumerable<IVirtualItem> GetCachedContent()
        {
            IEnumerable<IVirtualItem> enumerable;
            if (this.ItemInfo.Context.Cache.TryGetValue(this.ItemInfo.AbsoluteUri, out enumerable))
            {
                return enumerable;
            }
            return base.GetCachedContent();
        }

        public override IEnumerable<IVirtualItem> GetContent()
        {
            return new <GetContent>d__e(-2) { <>4__this = this };
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__7(-2) { <>4__this = this };
        }

        public override int GetHashCode()
        {
            return this.ItemInfo.AbsoluteUri.GetHashCode();
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            if (this.ItemInfo.AbsoluteUri.AbsolutePath == "/")
            {
                Image image;
                if ((FtpRootIcons == null) || !FtpRootIcons.TryGetValue(size, out image))
                {
                    Icon icon = new Icon(Resources.FtpRoot, size);
                    image = icon.ToBitmap();
                    icon.Dispose();
                    if (FtpRootIcons == null)
                    {
                        FtpRootIcons = IconCollection.Create();
                    }
                    FtpRootIcons.Add(size, image);
                }
                return image;
            }
            return this.Extender.GetIcon(size, (style & IconStyle.CanUseAlphaBlending) > 0);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemInfo", this.ItemInfo);
            if (this.FIsExists.HasValue)
            {
                info.AddValue("IsExists", this.FIsExists.Value);
            }
        }

        public string GetPrefferedLinkName(LinkType type)
        {
            return ((type == LinkType.Default) ? (this.Name + ".url") : null);
        }

        public PropertyAvailability GetPropertyAvailability(int property)
        {
            return (this.AvailableProperties[property] ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        private void Initialize()
        {
            this.FtpChangedHandler = new EventHandler<FtpChangedEventArg>(this.OnFtpChanged);
            FtpContext.FtpChanged += this.FtpChangedHandler;
        }

        public bool IsChild(IVirtualItem Item)
        {
            FtpItemInfo itemInfo = FtpItemInfo.GetItemInfo(Item);
            if (this.UriWithSlash == null)
            {
                this.UriWithSlash = new Uri(PathHelper.IncludeTrailingDirectorySeparator(this.ItemInfo.AbsoluteUri.ToString()));
            }
            return ((itemInfo != null) && this.UriWithSlash.IsBaseOf(itemInfo.AbsoluteUri));
        }

        private void ListParsingFailed(string list)
        {
            Exception exception = new WarningException(string.Format("Failed to parse LIST details on '{0}'. Maybe unknown LIST answer format?", this.FullName));
            exception.Data.Add("LIST", list);
            throw exception;
        }

        public IVirtualItem MoveTo(IVirtualFolder dest)
        {
            throw new NotImplementedException();
        }

        private void OnFtpChanged(object sender, FtpChangedEventArg e)
        {
            IList<IVirtualItem> content = base.Content;
            if ((content != null) && ((base.BackgroundThread == null) || !base.BackgroundThread.IsAlive))
            {
                if (this.ItemInfo.AbsoluteUri.Equals(e.AbsoluteUri))
                {
                    if (e.ChangeType == WatcherChangeTypes.Deleted)
                    {
                        this.FIsExists = false;
                    }
                    else if (e.ChangeType == WatcherChangeTypes.Renamed)
                    {
                        this.ItemInfo.Name = e.NewName;
                    }
                }
                else if (this.ItemInfo.AbsoluteUri.Equals(e.BaseUri))
                {
                    lock (content)
                    {
                        if (e.ChangeType == WatcherChangeTypes.Created)
                        {
                            IVirtualItem item = FtpItem.FromUri(this.ItemInfo.Context, e.AbsoluteUri, VirtualItemType.Unknown, this);
                            content.Add(item);
                            base.RaiseChanged(WatcherChangeTypes.Created, item);
                        }
                        else
                        {
                            for (int i = content.Count - 1; i >= 0; i--)
                            {
                                IVirtualItem item2 = content[i];
                                FtpItemInfo itemInfo = FtpItemInfo.GetItemInfo(item2);
                                if (itemInfo.AbsoluteUri.Equals(e.AbsoluteUri))
                                {
                                    switch (e.ChangeType)
                                    {
                                        case WatcherChangeTypes.Deleted:
                                            content.RemoveAt(i);
                                            base.RaiseChanged(WatcherChangeTypes.Deleted, item2);
                                            break;

                                        case WatcherChangeTypes.Renamed:
                                        {
                                            FtpItem item3 = item2 as FtpItem;
                                            if (item3 != null)
                                            {
                                                item3.ResetVisualCache();
                                            }
                                            itemInfo.Name = e.NewName;
                                            base.RaiseChanged(WatcherChangeTypes.Renamed, item2);
                                            return;
                                        }
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void PopulateContent()
        {
            List<Uri> folders = null;
            if (this.ItemInfo.Context.UseCache && this.ItemInfo.Context.UsePrefetch)
            {
                folders = new List<Uri>();
                FtpFolder parent = this.Parent as FtpFolder;
                if (parent != null)
                {
                    folders.Add(parent.ItemInfo.AbsoluteUri);
                }
            }
            bool flag = false;
            StringBuilder builder = new StringBuilder(0x200);
            foreach (string str in this.ItemInfo.Context.ListDirectoryDetails(this.ItemInfo.AbsoluteUri))
            {
                IVirtualItem item = FtpItem.ParseListString(this.ItemInfo.Context, this.ItemInfo.AbsoluteUri, str, this);
                if (item != null)
                {
                    FtpLink link = item as FtpLink;
                    if (link != null)
                    {
                        IVirtualItem target = link.Target;
                    }
                    if (folders != null)
                    {
                        FtpFolder folder2 = item as FtpFolder;
                        if (folder2 != null)
                        {
                            folders.Add(folder2.ItemInfo.AbsoluteUri);
                        }
                    }
                    flag = true;
                    base.NewItem(item);
                }
                else if (builder.Length < 0x200)
                {
                    builder.AppendLine(str);
                }
            }
            if (!(flag || (builder.Length <= 0)))
            {
                this.ListParsingFailed(builder.ToString());
            }
            if (!base.CancellationPending && this.ItemInfo.Context.UseCache)
            {
                IList<IVirtualItem> content = base.Content;
                lock (content)
                {
                    this.ItemInfo.Context.Cache[this.ItemInfo.AbsoluteUri] = new List<IVirtualItem>(content);
                }
                base.RaiseCacheContentChanged(EventArgs.Empty);
                if (folders != null)
                {
                    this.PrefetchFolders(folders);
                }
            }
        }

        private void PrefetchFolders(object folders)
        {
            foreach (Uri uri in (List<Uri>) folders)
            {
                if (!this.ItemInfo.Context.Cache.ContainsKey(uri))
                {
                    IVirtualFolder parent = new FtpFolder(this.ItemInfo.Context, uri);
                    List<IVirtualItem> list = new List<IVirtualItem>();
                    foreach (string str in this.ItemInfo.Context.ListDirectoryDetails(uri))
                    {
                        IVirtualItem item = FtpItem.ParseListString(this.ItemInfo.Context, uri, str, parent);
                        if (item != null)
                        {
                            FtpLink link = item as FtpLink;
                            if (link != null)
                            {
                                IVirtualItem target = link.Target;
                            }
                            list.Add(item);
                        }
                    }
                    this.ItemInfo.Context.Cache[uri] = list;
                }
            }
        }

        protected override void RaiseChanged(VirtualItemChangedEventArgs e)
        {
            if (this.OnChanged != null)
            {
                this.OnChanged(this, e);
            }
        }

        public void ShowProperties(IWin32Window owner)
        {
            this.ItemInfo.ShowProperties(owner, this);
        }

        public void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            using (PropertiesDialog dialog = new PropertiesDialog())
            {
                dialog.Execute(owner, items);
            }
        }

        public override string ToString()
        {
            return this.ItemInfo.AbsoluteUri.ToString();
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
                if (this.FAvailableSet == null)
                {
                    this.FAvailableSet = this.ItemInfo.CreateAvailableSet();
                    this.FAvailableSet[2] = true;
                }
                return this.FAvailableSet;
            }
        }

        public override Nomad.FileSystem.Virtual.CacheState CacheState
        {
            get
            {
                IEnumerable<IVirtualItem> enumerable;
                Nomad.FileSystem.Virtual.CacheState unknown = Nomad.FileSystem.Virtual.CacheState.Unknown;
                if (this.ItemInfo.Context.Cache.TryGetValue(this.ItemInfo.AbsoluteUri, out enumerable))
                {
                    unknown |= Nomad.FileSystem.Virtual.CacheState.HasContent;
                    foreach (IVirtualItem item in enumerable)
                    {
                        unknown |= Nomad.FileSystem.Virtual.CacheState.HasItems;
                        if (item is IVirtualFolder)
                        {
                            return (unknown | Nomad.FileSystem.Virtual.CacheState.HasFolders);
                        }
                    }
                }
                return unknown;
            }
        }

        public bool CanSendToBin
        {
            get
            {
                return false;
            }
        }

        public uint ClusterSize
        {
            get
            {
                return 0;
            }
        }

        public bool Exists
        {
            get
            {
                if (!this.FIsExists.HasValue)
                {
                    try
                    {
                        this.ItemInfo.Context.PrintWorkingDirectory(this.ItemInfo.AbsoluteUri);
                        this.FIsExists = true;
                    }
                    catch (WebException exception)
                    {
                        switch (exception.Status)
                        {
                            case WebExceptionStatus.NameResolutionFailure:
                            case WebExceptionStatus.ConnectFailure:
                            case WebExceptionStatus.ReceiveFailure:
                            case WebExceptionStatus.SendFailure:
                            case WebExceptionStatus.RequestCanceled:
                            case WebExceptionStatus.ConnectionClosed:
                            case WebExceptionStatus.Timeout:
                                this.FIsExists = false;
                                goto Label_00D0;

                            case WebExceptionStatus.ProtocolError:
                                if (((FtpWebResponse) exception.Response).StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                                {
                                    throw;
                                }
                                this.FIsExists = false;
                                goto Label_00D0;
                        }
                        throw;
                    }
                }
            Label_00D0:
                return this.FIsExists.Value;
            }
        }

        private VirtualItemVisualExtender Extender
        {
            get
            {
                if (this.FExtender == null)
                {
                    this.FExtender = new VirtualItemVisualExtender(this);
                }
                return this.FExtender;
            }
        }

        public string FileSystem
        {
            get
            {
                return null;
            }
        }

        public string FullName
        {
            get
            {
                return this.ItemInfo.FullName;
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return this.Extender.Highlighter;
            }
        }

        public object this[int propertyId]
        {
            get
            {
                if (propertyId == 2)
                {
                    return this.Extender.Type;
                }
                return this.ItemInfo[propertyId];
            }
            set
            {
                if (propertyId != 0)
                {
                    throw new ApplicationException("Cannot set this property");
                }
                ((IChangeVirtualItem) this).Name = (string) value;
            }
        }

        public string Location
        {
            get
            {
                return new Uri(this.ItemInfo.AbsoluteUri, "/").ToString();
            }
        }

        public override string Name
        {
            get
            {
                return this.ItemInfo.Name;
            }
        }

        string IChangeVirtualItem.Name
        {
            get
            {
                return this.ItemInfo.Name;
            }
            set
            {
                this.ItemInfo.Rename(value);
            }
        }

        public IWin32Window Owner
        {
            get
            {
                return this.ItemInfo.Context.Owner;
            }
            set
            {
                this.ItemInfo.Context.Owner = value;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.ItemInfo.Parent;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                if (this.ItemInfo.AbsoluteUri.AbsolutePath != "/")
                {
                    return new FtpFolder(this.ItemInfo.Context, new Uri(this.ItemInfo.AbsoluteUri, "/"), this.FIsExists.HasValue ? this.FIsExists : null, null);
                }
                return this;
            }
        }

        public string ShortName
        {
            get
            {
                if (this.ItemInfo.AbsoluteUri.AbsolutePath == "/")
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(this.ItemInfo.AbsoluteUri.Host);
                    if (!this.ItemInfo.AbsoluteUri.IsDefaultPort)
                    {
                        builder.Append(':');
                        builder.Append(this.ItemInfo.AbsoluteUri.Port);
                    }
                    return builder.ToString();
                }
                return this.ItemInfo.Name;
            }
        }

        public string ToolTip
        {
            get
            {
                IEnumerable<IVirtualItem> enumerable;
                if (!this.FToolTipAvailable && this.ItemInfo.Context.Cache.TryGetValue(this.ItemInfo.AbsoluteUri, out enumerable))
                {
                    this.Extender.ToolTip = VirtualItemVisualExtender.GetContentToolTip(enumerable);
                    this.FToolTipAvailable = true;
                }
                if (this.FToolTipAvailable)
                {
                    return this.Extender.ToolTip;
                }
                return null;
            }
        }

        public DriveType VolumeType
        {
            get
            {
                return DriveType.Network;
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__e : IEnumerable<IVirtualItem>, IEnumerable, IEnumerator<IVirtualItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualItem <>2__current;
            public FtpFolder <>4__this;
            public IEnumerator<string> <>7__wrap13;
            private int <>l__initialThreadId;
            public List<IVirtualItem> <FolderContent>5__f;
            public IVirtualItem <Item>5__12;
            public string <Line>5__11;
            public StringBuilder <ListBuilder>5__10;

            [DebuggerHidden]
            public <GetContent>d__e(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally14()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap13 != null)
                {
                    this.<>7__wrap13.Dispose();
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
                            this.<FolderContent>5__f = new List<IVirtualItem>();
                            this.<ListBuilder>5__10 = new StringBuilder(0x200);
                            this.<>7__wrap13 = this.<>4__this.ItemInfo.Context.ListDirectoryDetails(this.<>4__this.ItemInfo.AbsoluteUri).GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap13.MoveNext())
                            {
                                this.<Line>5__11 = this.<>7__wrap13.Current;
                                this.<Item>5__12 = FtpItem.ParseListString(this.<>4__this.ItemInfo.Context, this.<>4__this.ItemInfo.AbsoluteUri, this.<Line>5__11, this.<>4__this);
                                if (this.<Item>5__12 == null)
                                {
                                    goto Label_0114;
                                }
                                this.<FolderContent>5__f.Add(this.<Item>5__12);
                                this.<>2__current = this.<Item>5__12;
                                this.<>1__state = 2;
                                return true;
                            Label_010A:
                                this.<>1__state = 1;
                                goto Label_013F;
                            Label_0114:
                                if (this.<ListBuilder>5__10.Length < 0x200)
                                {
                                    this.<ListBuilder>5__10.AppendLine(this.<Line>5__11);
                                }
                            Label_013F:;
                            }
                            this.<>m__Finally14();
                            if ((this.<FolderContent>5__f.Count == 0) && (this.<ListBuilder>5__10.Length > 0))
                            {
                                this.<>4__this.ListParsingFailed(this.<ListBuilder>5__10.ToString());
                            }
                            this.<>4__this.ItemInfo.Context.Cache[this.<>4__this.ItemInfo.AbsoluteUri] = this.<FolderContent>5__f;
                            this.<>4__this.RaiseCacheContentChanged(EventArgs.Empty);
                            break;

                        case 2:
                            goto Label_010A;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<IVirtualItem> IEnumerable<IVirtualItem>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new FtpFolder.<GetContent>d__e(0) { <>4__this = this.<>4__this };
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
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally14();
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
        private sealed class <GetFolders>d__7 : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public FtpFolder <>4__this;
            public IEnumerator<IVirtualFolder> <>7__wrapa;
            private int <>l__initialThreadId;
            public IEnumerable<IVirtualItem> <CachedContent>5__8;
            public IVirtualFolder <NextFolder>5__9;

            [DebuggerHidden]
            public <GetFolders>d__7(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapa != null)
                {
                    this.<>7__wrapa.Dispose();
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
                            if (this.<>4__this.ItemInfo.Context.Cache.TryGetValue(this.<>4__this.ItemInfo.AbsoluteUri, out this.<CachedContent>5__8))
                            {
                                this.<>7__wrapa = this.<CachedContent>5__8.OfType<IVirtualFolder>().GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrapa.MoveNext())
                                {
                                    this.<NextFolder>5__9 = this.<>7__wrapa.Current;
                                    this.<>2__current = this.<NextFolder>5__9;
                                    this.<>1__state = 2;
                                    return true;
                                Label_00AE:
                                    this.<>1__state = 1;
                                }
                                this.<>m__Finallyb();
                            }
                            break;

                        case 2:
                            goto Label_00AE;
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
                return new FtpFolder.<GetFolders>d__7(0) { <>4__this = this.<>4__this };
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
                            this.<>m__Finallyb();
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

