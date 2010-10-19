namespace Nomad.FileSystem.LocalFile
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Microsoft.Win32.Security;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.Commons.Threading;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class CustomFileSystemFolder : FileSystemItem, IUpdateVirtualProperty, ISetVirtualProperty, IVirtualCachedFolder, ICreateVirtualLink, IVirtualFolderUI, IVirtualFolder, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IDisposable, IGetVirtualVolume, IGetVirtualRoot
    {
        private DirectoryInfo _FolderInfo;
        private FileTimeSetter _FolderTimeSetter;
        private WaitCallback CalculateFolderSizeCallback;
        private const string CategoryLocalFolder = "LocalFolder";
        private List<FileSystemItem> Content;
        private FileSystemEventHandler ContentGlobalHandler;
        private object ContentLock;
        private Dictionary<string, int> ContentMap;
        private FileSystemChangeNotification ContentNotification;
        private FileSystemWatcher ContentWatcher;
        private BitArray DeletedContent;
        private IVirtualFolder FRoot;
        private VolumeInfo FVolume;
        private CancelEventHandler QueryRemoveHandler;
        public static AutoRefreshMode SlowVolumeAutoRefresh = AutoRefreshMode.Simplified;

        public event EventHandler CachedContentChanged;

        private event EventHandler<VirtualItemChangedEventArgs> FOnChanged;

        public event EventHandler<VirtualItemChangedEventArgs> OnChanged
        {
            add
            {
                this.FOnChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Combine(this.FOnChanged, value);
                if (this.Content != null)
                {
                    this.EnableWatcher();
                }
            }
            remove
            {
                this.FOnChanged = (EventHandler<VirtualItemChangedEventArgs>) Delegate.Remove(this.FOnChanged, value);
                if (this.FOnChanged == null)
                {
                    this.DisposeWatcher(true);
                    this.ClearContentCache();
                }
            }
        }

        protected CustomFileSystemFolder(DirectoryInfo info, IVirtualFolder parent) : base((info != null) ? PathHelper.IncludeTrailingDirectorySeparator(info.FullName) : null, parent)
        {
            this.ContentLock = new object();
            this._FolderInfo = info;
            this.Initialize();
        }

        protected CustomFileSystemFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ContentLock = new object();
            this.Initialize();
        }

        protected CustomFileSystemFolder(string folderPath, IVirtualFolder parent) : base(PathHelper.IncludeTrailingDirectorySeparator(folderPath), parent)
        {
            this.ContentLock = new object();
            this.Initialize();
        }

        protected virtual void BeginGetContent()
        {
            if (this.ContentWatcher != null)
            {
                this.ContentWatcher.EnableRaisingEvents = false;
            }
        }

        public void BeginUpdate()
        {
            if (!((this._FolderTimeSetter != null) || base.CheckCapability(FileSystemItem.ItemCapability.IsElevated)))
            {
                this._FolderTimeSetter = new FileTimeSetter(this.ComparableName, true);
                GC.SuppressFinalize(this._FolderTimeSetter);
            }
        }

        protected override bool CacheProperty(int propertyId)
        {
            return ((propertyId == 15) || base.CacheProperty(propertyId));
        }

        private void CalculateFolderSize(object state)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(base.FullName);
            VirtualPropertySet propertySet = new VirtualPropertySet(new int[] { 3 });
            VirtualItemChangedEventArgs e = new VirtualItemChangedEventArgs(this, propertySet);
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                uint clusterSize = 0;
                if (OS.IsWinVista)
                {
                    propertySet[4] = true;
                }
                else
                {
                    clusterSize = this.ClusterSize;
                    if (clusterSize > 0)
                    {
                        propertySet[4] = true;
                    }
                }
                long num2 = 0L;
                long num3 = 0L;
                while ((stack.Count > 0) && (this.CalculateFolderSizeCallback != null))
                {
                    string directoryPath = stack.Pop();
                    try
                    {
                        foreach (ReadOnlyFileSystemInfo info in Microsoft.IO.Directory.GetItems(directoryPath))
                        {
                            if (this.CalculateFolderSizeCallback == null)
                            {
                                goto Label_025A;
                            }
                            if ((info.Attributes & FileAttributes.Directory) > 0)
                            {
                                stack.Push(info.FullName);
                            }
                            else
                            {
                                long length = info.Length;
                                num2 += length;
                                if (OS.IsWinVista)
                                {
                                    try
                                    {
                                        using (SafeFileHandle handle = Microsoft.IO.File.OpenReadAttributes(info.FullName))
                                        {
                                            FILE_STANDARD_INFO file_standard_info;
                                            if (!Windows.GetFileInformationByHandleEx(handle, out file_standard_info))
                                            {
                                                throw new Win32IOException();
                                            }
                                            num3 += file_standard_info.AllocationSize;
                                        }
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                    }
                                }
                                else if (clusterSize > 0)
                                {
                                    if ((info.Attributes & FileAttributes.Compressed) > 0)
                                    {
                                        length = Microsoft.IO.File.GetCompressedFileSize(info.FullName);
                                    }
                                    num3 += (long) (((length / ((ulong) clusterSize)) + Math.Sign((long) (length % ((ulong) clusterSize)))) * clusterSize);
                                }
                            }
                            if (stopwatch.ElapsedMilliseconds >= 500L)
                            {
                                base.AddPropertyToCache(3, num2);
                                if (propertySet[4])
                                {
                                    base.AddPropertyToCache(4, num3);
                                }
                                if (base.CheckCapability(FileSystemItem.ItemCapability.HasExtender))
                                {
                                    base.Extender.ToolTip = null;
                                }
                                stopwatch.Reset();
                                base.OnItemChanged(e);
                                stopwatch.Start();
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                Label_025A:;
                }
                base.AddPropertyToCache(3, num2);
                if (propertySet[4])
                {
                    base.AddPropertyToCache(4, num3);
                }
                if (base.CheckCapability(FileSystemItem.ItemCapability.HasExtender))
                {
                    base.Extender.ToolTip = null;
                }
            }
            catch (Exception exception)
            {
                base.Extender.ToolTip = exception.Message;
            }
            finally
            {
                stopwatch.Stop();
                base.OnItemChanged(e);
            }
        }

        public override void CancelGetSlowProperty()
        {
            this.CalculateFolderSizeCallback = null;
            base.CancelGetSlowProperty();
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if ((folder != null) && folder.Exists)
            {
                LinkType type = LinkType.ShellFolderLink | LinkType.Default;
                VolumeInfo folderVolume = folder.FolderVolume;
                if (folderVolume != null)
                {
                    if ((folderVolume.Capabilities & VolumeCapabilities.ReparsePoints) > ((VolumeCapabilities) 0))
                    {
                        type |= LinkType.JuntionPoint;
                    }
                    if (OS.IsWinVista && string.Equals(folderVolume.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase))
                    {
                        type |= LinkType.SymbolicLink;
                    }
                }
                return type;
            }
            return LinkType.None;
        }

        public override bool CanSetProperty(int property)
        {
            switch (property)
            {
                case 7:
                case 8:
                case 9:
                    return OS.IsWinNT;

                case 14:
                    if (OS.IsWinNT)
                    {
                        VolumeInfo itemVolume = base.ItemVolume;
                        return ((itemVolume != null) && ((itemVolume.Capabilities & VolumeCapabilities.FilePersistentAcls) > ((VolumeCapabilities) 0)));
                    }
                    return false;
            }
            return base.CanSetProperty(property);
        }

        public void ClearContentCache()
        {
            lock (this.ContentLock)
            {
                if (this.Content != null)
                {
                    foreach (FileSystemItem item in this.Content)
                    {
                        item.ItemChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.OnItemChanged);
                        SlowPropertyProvider provider = item;
                        if (provider != null)
                        {
                            provider.CancelGetSlowProperty();
                        }
                    }
                }
                this.Content = null;
                this.ContentMap = null;
                this.DeletedContent = null;
            }
            this.RaiseCachedContentChanged(EventArgs.Empty);
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[3] = true;
            set[4] = OS.IsWinVista || (this.ClusterSize > 0);
            if (OS.IsWinNT)
            {
                VolumeInfo itemVolume = base.ItemVolume;
                if ((itemVolume != null) && ((itemVolume.Capabilities & VolumeCapabilities.FilePersistentAcls) > ((VolumeCapabilities) 0)))
                {
                    set[14] = true;
                    set[15] = true;
                }
            }
            return set;
        }

        private static Dictionary<string, int> CreateContentMap()
        {
            switch (FileSystemItem.ComparisonRule)
            {
                case StringComparison.Ordinal:
                    return new Dictionary<string, int>(StringComparer.Ordinal);

                case StringComparison.OrdinalIgnoreCase:
                    return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            }
            throw new ApplicationException("Cannot create content map. Unsupported StringComparison rule.");
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, IEnumerable<IVirtualItem> items, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return ShellContextMenuHelper.CreateContextMenu(owner, GetFileNames(items), options, onExecuteVerb);
        }

        public IChangeVirtualFile CreateFile(string name)
        {
            FileSystemItem.CheckItemName(name);
            FileSystemItem item = LocalFileSystemCreator.FromFullName(Path.Combine(this.ContentFolder.FullName, name), VirtualItemType.File, null);
            if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
            {
                item.Proxy = base.Proxy;
                item.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
            }
            return (IChangeVirtualFile) item;
        }

        public IVirtualFolder CreateFolder(string name)
        {
            if (string.Equals(PathHelper.ExcludeTrailingDirectorySeparator(name), PathHelper.ExcludeTrailingDirectorySeparator(this.ContentFolder.FullName), FileSystemItem.ComparisonRule))
            {
                return this;
            }
            string str = PathHelper.IncludeTrailingDirectorySeparator(this.ContentFolder.FullName);
            if (name.StartsWith(str, FileSystemItem.ComparisonRule))
            {
                name = name.Substring(str.Length);
            }
            if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
            {
                FileSystemFolder folder = new FileSystemFolder(this.ContentFolder.CreateSubdirectory(name)) {
                    Proxy = base.Proxy
                };
                folder.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, folder.FullName);
                return folder;
            }
            string str2 = str;
            foreach (string str3 in StringHelper.SplitString(name, new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }))
            {
                str2 = Path.Combine(str2, str3);
                if (!System.IO.Directory.Exists(str2))
                {
                    System.IO.Directory.CreateDirectory(str2);
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, str2);
                }
            }
            return new FileSystemFolder(str2);
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            IVirtualItem item;
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if (folder == null)
            {
                throw new ArgumentException("destFolder is not CustomFileSystemFolder");
            }
            switch (type)
            {
                case LinkType.Default:
                case LinkType.ShellFolderLink:
                case LinkType.JuntionPoint:
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
                    {
                        folder.ResetInfo();
                        folder.Proxy = base.Proxy;
                        folder.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
                    }
                    break;
            }
            switch (type)
            {
                case LinkType.Default:
                    return FileSystemItem.CreateShellLink(folder, name, base.FullName);

                case LinkType.ShellFolderLink:
                    return this.CreateShellFolderLink(folder, name);

                case LinkType.JuntionPoint:
                    item = folder.CreateFolder(name);
                    ((IDirectoryProxy) base.Proxy).CreateJunctionPoint(item.FullName, base.FullName);
                    return item;

                case LinkType.SymbolicLink:
                {
                    string fileName = Path.Combine(destFolder.FullName, name);
                    ((IFileProxy) base.Proxy).CreateSymbolicLink(fileName, base.FullName, true);
                    item = new FileSystemFile(fileName);
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, fileName);
                    return item;
                }
            }
            throw new ArgumentException("Unsupported link type", "type");
        }

        protected IVirtualFolder CreateShellFolderLink(CustomFileSystemFolder folder, string name)
        {
            CustomFileSystemFolder destFolder = (CustomFileSystemFolder) folder.CreateFolder(name);
            destFolder.Info.Attributes = FileAttributes.ReadOnly;
            this.CreateLink(destFolder, "target.lnk", LinkType.Default);
            FileInfo info = base.CreateInfo<FileInfo>(Path.Combine(destFolder.FullName, "desktop.ini"));
            using (TextWriter writer = info.CreateText())
            {
                writer.WriteLine("[.ShellClassInfo]");
                writer.WriteLine("CLSID2={0AFACED1-E828-11D1-9187-B532F1E9575D}");
            }
            info.Attributes = FileAttributes.System | FileAttributes.Hidden;
            return destFolder;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.DisposeWatcher(disposing);
            this.ClearContentCache();
        }

        protected virtual void DisposeWatcher(bool disposing)
        {
            Debug.WriteLine("CustomFileSystemFolder.DisposeWatcher()", "LocalFolder");
            if (this.ContentGlobalHandler != null)
            {
                LocalFileSystemCreator.GlobalFileChanged -= this.ContentGlobalHandler;
                base.SetCapability(FileSystemItem.ItemCapability.GlobalFileChangedAssigned, false);
            }
            this.ContentGlobalHandler = null;
            if (this.ContentWatcher != null)
            {
                this.ContentWatcher.Dispose();
            }
            this.ContentWatcher = null;
            if (this.ContentNotification != null)
            {
                this.ContentNotification.Dispose();
            }
            this.ContentNotification = null;
            if (this.QueryRemoveHandler != null)
            {
                VolumeEvents.UnregisterRemovingEvent(Path.GetPathRoot(base.FullName), this.QueryRemoveHandler);
                base.SetCapability(FileSystemItem.ItemCapability.QueryRemoveAssigned, false);
            }
            this.QueryRemoveHandler = null;
        }

        public bool Elevate(IPluginProcess process)
        {
            bool flag;
            if (process == null)
            {
                throw new ArgumentNullException();
            }
            if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
            {
                return false;
            }
            IPluginActivator activator = process as IPluginActivator;
            if (activator == null)
            {
                return false;
            }
            if (!(process.IsAlive || process.Start()))
            {
                return false;
            }
            IDirectoryProxy proxy = activator.Create<IDirectoryProxy>("filesystemproxy");
            try
            {
                base.Proxy = proxy;
                this._FolderInfo = base.CreateInfo<DirectoryInfo>(base.FullName);
                base.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
                flag = true;
            }
            catch (RemotingException)
            {
            }
            return flag;
        }

        protected virtual void EnableWatcher()
        {
            Debug.WriteLine("CustomFileSystemFolder.EnableWatcher()", "LocalFolder");
            this.InitializeWatcher(this.ContentFolder.FullName);
            if (!base.CheckCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
            {
                if (!((this.ContentGlobalHandler == null) || base.CheckCapability(FileSystemItem.ItemCapability.GlobalFileChangedAssigned)))
                {
                    LocalFileSystemCreator.GlobalFileChanged += this.ContentGlobalHandler;
                    base.SetCapability(FileSystemItem.ItemCapability.GlobalFileChangedAssigned, true);
                    Debug.WriteLine("> GlobalFileChanged assigned", "LocalFolder");
                }
                if (this.ContentWatcher != null)
                {
                    bool flag = false;
                    try
                    {
                        this.ContentWatcher.EnableRaisingEvents = true;
                        Debug.WriteLine(string.Format("> ContentWatcher.EnableRaisingEvents = {0}", (this.ContentWatcher != null) && this.ContentWatcher.EnableRaisingEvents), "LocalFolder");
                    }
                    catch (ArgumentException)
                    {
                        this.SetDeletedCapability(true);
                        flag = true;
                    }
                    catch (FileNotFoundException)
                    {
                        this.SetDeletedCapability(true);
                        flag = true;
                    }
                    catch (Exception exception)
                    {
                        Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        this.InitializeContentNotification(this.ContentWatcher.Path, this.ContentWatcher.NotifyFilter);
                        flag = true;
                    }
                    if (flag)
                    {
                        this.ContentWatcher.Dispose();
                        this.ContentWatcher = null;
                    }
                }
                if (this.ContentNotification != null)
                {
                    try
                    {
                        this.ContentNotification.EnableRaisingEvents = true;
                        Debug.WriteLine(string.Format("> ContentNotification.EnableRaisingEvents = {0}", (this.ContentNotification != null) && this.ContentNotification.EnableRaisingEvents), "LocalFolder");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        this.SetDeletedCapability(true);
                    }
                }
                if (!((this.QueryRemoveHandler == null) || base.CheckAnyCapability(FileSystemItem.ItemCapability.QueryRemoveAssigned | FileSystemItem.ItemCapability.Deleted)))
                {
                    VolumeEvents.RegisterRemovingEvent(Path.GetPathRoot(base.FullName), this.QueryRemoveHandler);
                    base.SetCapability(FileSystemItem.ItemCapability.QueryRemoveAssigned, true);
                }
            }
        }

        protected void EndGetContent()
        {
            if (this.FOnChanged != null)
            {
                this.EnableWatcher();
            }
        }

        public void EndUpdate()
        {
            if ((this._FolderTimeSetter != null) && this._FolderTimeSetter.Set())
            {
                base.SetCapability(FileSystemItem.ItemCapability.ItemRefreshNeeded, true);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
            }
            this._FolderTimeSetter = null;
        }

        public IEnumerable<IVirtualItem> GetCachedContent()
        {
            List<FileSystemItem> content = null;
            Predicate<int> match = null;
            bool flag = false;
            lock (this.ContentLock)
            {
                content = this.Content;
                if (content == null)
                {
                    content = new List<FileSystemItem>(this.GetContent(false));
                    this.Content = content;
                    this.ContentMap = null;
                    flag = true;
                }
                else if (this.DeletedContent != null)
                {
                    if (match == null)
                    {
                        match = delegate (int index) {
                            return (this.DeletedContent.Length > index) && this.DeletedContent[index];
                        };
                    }
                    flag = this.RemoveAll<FileSystemItem>(content, match) > 0;
                    this.ContentMap = null;
                    this.DeletedContent = null;
                }
                if (!((this.ContentMap != null) || base.CheckCapability(FileSystemItem.ItemCapability.DisableContentMap)))
                {
                    this.RebuildContentMap();
                }
            }
            if (flag)
            {
                this.RaiseCachedContentChanged(EventArgs.Empty);
            }
            return content.ToArray();
        }

        public IEnumerable<IVirtualItem> GetContent()
        {
            return this.GetContent(true).Cast<IVirtualItem>();
        }

        protected IEnumerable<FileSystemItem> GetContent(bool cacheItems)
        {
            return new <GetContent>d__c(-2) { <>4__this = this, <>3__cacheItems = cacheItems };
        }

        private static string[] GetFileNames(IEnumerable<IVirtualItem> items)
        {
            List<string> list;
            if (items is ICollection<IVirtualItem>)
            {
                list = new List<string>(((ICollection<IVirtualItem>) items).Count);
            }
            else
            {
                list = new List<string>();
            }
            foreach (IVirtualItem item in items)
            {
                if (!(item is FileSystemItem))
                {
                    throw new ArgumentException("items must contain only file system items");
                }
                list.Add(item.FullName);
            }
            return list.ToArray();
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return this.GetCachedContent().OfType<IVirtualFolder>();
        }

        public virtual string GetPrefferedLinkName(LinkType type)
        {
            switch (type)
            {
                case LinkType.Default:
                    return (this.Name + ".lnk");

                case LinkType.ShellFolderLink:
                case LinkType.JuntionPoint:
                case LinkType.SymbolicLink:
                    return this.Name;
            }
            return null;
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 1:
                    return null;

                case 3:
                case 4:
                    if (!base.CheckCapability(FileSystemItem.ItemCapability.HasSize))
                    {
                        if (this.CalculateFolderSizeCallback == null)
                        {
                            this.CalculateFolderSizeCallback = new WaitCallback(this.CalculateFolderSize);
                        }
                        SlowPropertyProvider.SlowPropertyQueue.Value.QueueWeakWorkItem(this.CalculateFolderSizeCallback);
                        base.SetCapability(FileSystemItem.ItemCapability.HasSize, true);
                        base.ResetAvailableSet();
                    }
                    return 0L;

                case 14:
                    return this.FolderInfo.GetAccessControl(AccessControlSections.All);

                case 15:
                    return this.Owner;
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 1:
                    return PropertyAvailability.None;

                case 3:
                case 4:
                    return (base.CheckCapability(FileSystemItem.ItemCapability.HasSize) ? PropertyAvailability.Normal : PropertyAvailability.OnDemand);
            }
            return base.GetPropertyAvailability(propertyId);
        }

        protected virtual VolumeInfo GetVolume()
        {
            return VolumeCache.FromPath(this.ComparableName);
        }

        public bool HasChildren()
        {
            foreach (string str in Microsoft.IO.Directory.GetItemNames(base.FullName))
            {
                return true;
            }
            return false;
        }

        private void Initialize()
        {
            base.SetCapability(FileSystemItem.ItemCapability.HasLastAccessTime | FileSystemItem.ItemCapability.HasLastWriteTime | FileSystemItem.ItemCapability.HasCreationTime, OS.IsWinNT);
        }

        private void InitializeContentNotification(string path, NotifyFilters notifyFilter)
        {
            this.ContentNotification = new FileSystemChangeNotification();
            this.ContentNotification.BeginInit();
            try
            {
                this.ContentNotification.Path = path;
                this.ContentNotification.NotifyFilter = notifyFilter & ~NotifyFilters.CreationTime;
                this.ContentNotification.Changed += new EventHandler(this.OnNotificationChanged);
            }
            catch (DirectoryNotFoundException)
            {
                this.SetDeletedCapability(true);
            }
            finally
            {
                this.ContentNotification.EndInit();
            }
        }

        private void InitializeContentWatcher(string path, NotifyFilters notifyFilter)
        {
            this.ContentWatcher = new FileSystemWatcher();
            this.ContentWatcher.BeginInit();
            try
            {
                this.ContentWatcher.Path = path;
                this.ContentWatcher.NotifyFilter = notifyFilter;
                this.ContentWatcher.Changed += new FileSystemEventHandler(this.OnWatcherChanged);
                this.ContentWatcher.Created += new FileSystemEventHandler(this.OnWatcherCreated);
                this.ContentWatcher.Deleted += new FileSystemEventHandler(this.OnWatcherDeleted);
                this.ContentWatcher.Renamed += new RenamedEventHandler(this.OnWatcherRenamed);
                this.ContentWatcher.Error += new ErrorEventHandler(this.OnWatcherError);
            }
            catch (ArgumentException)
            {
                this.SetDeletedCapability(true);
            }
            finally
            {
                this.ContentWatcher.EndInit();
            }
        }

        protected virtual void InitializeWatcher(string path)
        {
            Debug.WriteLine(string.Format("CustomFileSystemFolder.InitializeWatcher({0})", path), "LocalFolder");
            VolumeInfo folderVolume = this.FolderVolume;
            if ((folderVolume != null) && !folderVolume.IsReadOnly)
            {
                AutoRefreshMode none;
                if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
                {
                    none = AutoRefreshMode.None;
                }
                else
                {
                    none = folderVolume.IsSlowDrive ? SlowVolumeAutoRefresh : AutoRefreshMode.Full;
                }
                if (none == AutoRefreshMode.None)
                {
                    if (this.ContentGlobalHandler == null)
                    {
                        this.ContentGlobalHandler = new FileSystemEventHandler(this.OnGlobalFileChanged);
                    }
                }
                else
                {
                    NotifyFilters notifyFilter = NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName;
                    if (none == AutoRefreshMode.Full)
                    {
                        notifyFilter |= NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size;
                    }
                    if ((((this.ContentWatcher == null) && (this.ContentNotification == null)) && !base.CheckCapability(FileSystemItem.ItemCapability.DisableContentMap)) && (Environment.OSVersion.Platform == PlatformID.Win32NT))
                    {
                        this.InitializeContentWatcher(path, notifyFilter);
                    }
                    if ((this.ContentNotification == null) && (this.ContentWatcher == null))
                    {
                        this.InitializeContentNotification(path, notifyFilter);
                    }
                    if (!((((this.ContentWatcher == null) && (this.ContentNotification == null)) || !folderVolume.CanEject) || folderVolume.IsReadOnly))
                    {
                        this.QueryRemoveHandler = new CancelEventHandler(this.OnQueryRemove);
                    }
                }
            }
        }

        protected override object InternalClone()
        {
            CustomFileSystemFolder folder = (CustomFileSystemFolder) base.InternalClone();
            folder.ContentLock = new object();
            folder.Initialize();
            folder.FVolume = this.FVolume;
            folder.SetCapability(FileSystemItem.ItemCapability.HasVolume, base.CheckCapability(FileSystemItem.ItemCapability.HasVolume));
            return folder;
        }

        public bool IsChild(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return ((item is FileSystemItem) && item.FullName.StartsWith(PathHelper.IncludeTrailingDirectorySeparator(base.FullName), FileSystemItem.ComparisonRule));
        }

        private void OnGlobalFileChanged(object source, FileSystemEventArgs e)
        {
            if (string.Equals(Path.GetDirectoryName(e.FullPath), this.ComparableName, FileSystemItem.ComparisonRule))
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                        this.OnWatcherCreated(source, e);
                        break;

                    case WatcherChangeTypes.Deleted:
                        this.OnWatcherDeleted(source, e);
                        break;

                    case WatcherChangeTypes.Changed:
                        this.OnWatcherChanged(source, e);
                        break;

                    case WatcherChangeTypes.Renamed:
                        this.OnWatcherRenamed(source, (RenamedEventArgs) e);
                        break;
                }
            }
        }

        private void OnItemChanged(object sender, VirtualItemChangedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                this.OnWatcherDeleted(((FileSystemItem) e.Item).ComparableName, -1);
            }
            else
            {
                this.RaiseChanged(e);
            }
        }

        private void OnNotificationChanged(object source, EventArgs e)
        {
            this.ClearContentCache();
            this.RaiseChanged(WatcherChangeTypes.All, null);
        }

        private void OnQueryRemove(object source, CancelEventArgs e)
        {
            this.DisposeWatcher(false);
        }

        private void OnWatcherChanged(object source, FileSystemEventArgs e)
        {
            this.OnWatcherChanged(e.FullPath, -1);
        }

        private void OnWatcherChanged(string fullPath, int itemIndex)
        {
            FileSystemItem item;
            lock (this.ContentLock)
            {
                List<FileSystemItem> content = this.Content;
                if ((content == null) || !((itemIndex >= 0) || this.ContentMap.TryGetValue(fullPath, out itemIndex)))
                {
                    return;
                }
                item = content[itemIndex];
                if (!item.Exists)
                {
                    this.OnWatcherDeleted(fullPath, itemIndex);
                    return;
                }
                if ((this.DeletedContent != null) && (itemIndex < this.DeletedContent.Count))
                {
                    this.DeletedContent[itemIndex] = false;
                }
            }
            item.ResetVisualCache();
            this.RaiseChanged(item, (this.ContentWatcher == null) ? VirtualProperty.All : DefaultProperty.FromNotifyFilters(this.ContentWatcher.NotifyFilter & ~NotifyFilters.FileName));
        }

        private void OnWatcherCreated(object source, FileSystemEventArgs e)
        {
            this.OnWatcherCreated(e.FullPath, -1);
        }

        private void OnWatcherCreated(string fullPath, int itemIndex)
        {
            FileSystemItem item;
            lock (this.ContentLock)
            {
                List<FileSystemItem> content = this.Content;
                if (content == null)
                {
                    return;
                }
                if ((itemIndex >= 0) || this.ContentMap.TryGetValue(fullPath, out itemIndex))
                {
                    this.OnWatcherChanged(fullPath, itemIndex);
                    return;
                }
                try
                {
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
                    {
                        item = LocalFileSystemCreator.FromFileSystemInfo(base.CreateInfo<FileSystemInfo>(fullPath), this);
                    }
                    else
                    {
                        item = LocalFileSystemCreator.FromFullName(fullPath, VirtualItemType.Unknown, this);
                    }
                    item.ItemChanged += new EventHandler<VirtualItemChangedEventArgs>(this.OnItemChanged);
                    content.Add(item);
                    this.ContentMap.Add(item.ComparableName, content.Count - 1);
                }
                catch (FileNotFoundException)
                {
                    return;
                }
            }
            this.RaiseChanged(WatcherChangeTypes.Created, item);
        }

        private void OnWatcherDeleted(object source, FileSystemEventArgs e)
        {
            this.OnWatcherDeleted(e.FullPath, -1);
        }

        private void OnWatcherDeleted(string fullPath, int itemIndex)
        {
            FileSystemItem item;
            lock (this.ContentLock)
            {
                List<FileSystemItem> content = this.Content;
                if ((content == null) || !((itemIndex >= 0) || this.ContentMap.TryGetValue(fullPath, out itemIndex)))
                {
                    return;
                }
                item = content[itemIndex];
                item.ItemChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.OnItemChanged);
                if (this.DeletedContent == null)
                {
                    this.DeletedContent = new BitArray(content.Count);
                }
                else if (this.DeletedContent.Count <= itemIndex)
                {
                    this.DeletedContent.Length = content.Count;
                }
                this.DeletedContent[itemIndex] = true;
            }
            this.RaiseChanged(WatcherChangeTypes.Deleted, item);
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            if (this.ContentWatcher != null)
            {
                this.InitializeContentNotification(this.ContentWatcher.Path, this.ContentWatcher.NotifyFilter);
                this.ContentWatcher.Dispose();
                this.ContentWatcher = null;
            }
            this.ClearContentCache();
            this.RaiseChanged(WatcherChangeTypes.All, null);
        }

        private void OnWatcherRenamed(object source, RenamedEventArgs e)
        {
            FileSystemItem item;
            lock (this.ContentLock)
            {
                int num;
                List<FileSystemItem> content = this.Content;
                if (content == null)
                {
                    return;
                }
                if (this.ContentMap.TryGetValue(e.FullPath, out num))
                {
                    this.OnWatcherDeleted(e.OldFullPath, -1);
                    this.OnWatcherCreated(e.FullPath, num);
                    return;
                }
                if (!this.ContentMap.TryGetValue(e.OldFullPath, out num))
                {
                    return;
                }
                FileSystemItem item2 = content[num];
                item2.ItemChanged -= new EventHandler<VirtualItemChangedEventArgs>(this.OnItemChanged);
                if (base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
                {
                    FileSystemInfo info;
                    if (item2 is IVirtualFolder)
                    {
                        info = base.CreateInfo<DirectoryInfo>(e.FullPath);
                    }
                    else
                    {
                        info = base.CreateInfo<FileInfo>(e.FullPath);
                    }
                    item = LocalFileSystemCreator.FromFileSystemInfo(info, this);
                }
                else
                {
                    item = LocalFileSystemCreator.FromFullName(e.FullPath, (item2 is IVirtualFolder) ? VirtualItemType.Folder : VirtualItemType.File, this);
                }
                item.ItemChanged += new EventHandler<VirtualItemChangedEventArgs>(this.OnItemChanged);
                content[num] = item;
                this.ContentMap.Remove(item2.ComparableName);
                this.ContentMap.Add(item.ComparableName, num);
            }
            this.RaiseChanged(WatcherChangeTypes.Renamed, item);
        }

        protected void RaiseCachedContentChanged(EventArgs e)
        {
            if (this.CachedContentChanged != null)
            {
                this.CachedContentChanged(this, e);
            }
        }

        protected void RaiseChanged(VirtualItemChangedEventArgs e)
        {
            if (this.FOnChanged != null)
            {
                this.FOnChanged(this, e);
            }
            this.RaiseCachedContentChanged(e);
        }

        private void RaiseChanged(IVirtualItem item, VirtualPropertySet propertySet)
        {
            if (this.FOnChanged != null)
            {
                this.RaiseChanged(new VirtualItemChangedEventArgs(item, propertySet));
            }
        }

        public void RaiseChanged(WatcherChangeTypes changeType, IVirtualItem item)
        {
            if (this.FOnChanged != null)
            {
                this.RaiseChanged(new VirtualItemChangedEventArgs(changeType, item));
            }
        }

        private void RebuildContentMap()
        {
            List<FileSystemItem> content = this.Content;
            this.ContentMap = CreateContentMap();
            for (int i = 0; i < content.Count; i++)
            {
                this.ContentMap.Add(content[i].ComparableName, i);
            }
        }

        private int RemoveAll<T>(List<T> list, Predicate<int> match)
        {
            int num = 0;
            int count = list.Count;
            int num3 = 0;
            while (num < count)
            {
                if (match(num))
                {
                    num3++;
                }
                else if (num3 > 0)
                {
                    list[num - num3] = list[num];
                }
                num++;
            }
            list.RemoveRange(list.Count - num3, num3);
            return num3;
        }

        protected internal override void ResetInfo()
        {
            if ((this._FolderInfo != null) && RemotingServices.IsTransparentProxy(this._FolderInfo))
            {
                LocalFileSystemCreator.Sponsor.Unregister(this._FolderInfo);
            }
            this._FolderInfo = null;
            base.ResetInfo();
        }

        protected void SetAccessControl(DirectorySecurity security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }
            AccessControlSections all = AccessControlSections.All;
            if (!Microsoft.Win32.Security.Security.ChangeCurrentProcessPrivilege("SeRestorePrivilege", true))
            {
                all &= ~all;
            }
            DirectorySecurity directorySecurity = new DirectorySecurity();
            directorySecurity.SetSecurityDescriptorBinaryForm(security.GetSecurityDescriptorBinaryForm(), all);
            this.FolderInfo.SetAccessControl(directorySecurity);
        }

        protected override void SetDeletedCapability(bool value)
        {
            base.SetDeletedCapability(value);
            if (value)
            {
                this.RaiseChanged(WatcherChangeTypes.Deleted, this);
            }
        }

        public override void SetProperty(int propertyId, object value)
        {
            switch (propertyId)
            {
                case 7:
                    if (this._FolderTimeSetter == null)
                    {
                        ((IDirectoryProxy) base.Proxy).SetCreationTime(this.ComparableName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FolderTimeSetter.CreationTime = (DateTime) value;
                    break;

                case 8:
                    if (this._FolderTimeSetter == null)
                    {
                        ((IDirectoryProxy) base.Proxy).SetLastWriteTime(this.ComparableName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FolderTimeSetter.LastWriteTime = (DateTime) value;
                    break;

                case 9:
                    if (this._FolderTimeSetter == null)
                    {
                        ((IDirectoryProxy) base.Proxy).SetLastAccessTime(this.ComparableName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FolderTimeSetter.LastAccessTime = (DateTime) value;
                    break;

                case 14:
                    this.SetAccessControl((DirectorySecurity) value);
                    break;

                default:
                    base.SetProperty(propertyId, value);
                    break;
            }
        }

        public void ShowProperties(IWin32Window owner, IEnumerable<IVirtualItem> items)
        {
            if (!ShellContextMenuHelper.ExecuteVerb(owner, "properties", GetFileNames(items)))
            {
                using (PropertiesDialog dialog = new PropertiesDialog())
                {
                    dialog.Execute(owner, items);
                }
            }
        }

        public override FileAttributes Attributes
        {
            get
            {
                if (base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                {
                    return FileAttributes.Directory;
                }
                return (base.Attributes | FileAttributes.Directory);
            }
        }

        public Nomad.FileSystem.Virtual.CacheState CacheState
        {
            get
            {
                Nomad.FileSystem.Virtual.CacheState unknown = Nomad.FileSystem.Virtual.CacheState.Unknown;
                lock (this.ContentLock)
                {
                    List<FileSystemItem> content = this.Content;
                    if (content == null)
                    {
                        return unknown;
                    }
                    unknown |= Nomad.FileSystem.Virtual.CacheState.HasContent;
                    for (int i = 0; i < content.Count; i++)
                    {
                        if (((this.DeletedContent == null) || (this.DeletedContent.Length <= i)) || !this.DeletedContent[i])
                        {
                            unknown |= Nomad.FileSystem.Virtual.CacheState.HasItems;
                            if (content[i] is IVirtualFolder)
                            {
                                return (unknown | Nomad.FileSystem.Virtual.CacheState.HasFolders);
                            }
                        }
                    }
                }
                return unknown;
            }
        }

        public uint ClusterSize
        {
            get
            {
                VolumeInfo folderVolume = this.FolderVolume;
                return ((folderVolume != null) ? folderVolume.ClusterSize : 0);
            }
        }

        public override string ComparableName
        {
            get
            {
                return PathHelper.ExcludeTrailingDirectorySeparator(base.FullName);
            }
        }

        protected override bool Compressed
        {
            set
            {
                if (value != this.Compressed)
                {
                    ((IDirectoryProxy) base.Proxy).SetCompressedState(base.FullName, value);
                    base.ResetAvailableSet();
                }
            }
        }

        protected virtual DirectoryInfo ContentFolder
        {
            get
            {
                return this.FolderInfo;
            }
        }

        public string FileSystem
        {
            get
            {
                VolumeInfo folderVolume = this.FolderVolume;
                return ((folderVolume != null) ? folderVolume.DriveFormat : null);
            }
        }

        protected DirectoryInfo FolderInfo
        {
            get
            {
                if (!((this._FolderInfo != null) || base.CheckCapability(FileSystemItem.ItemCapability.Unreadable)))
                {
                    this._FolderInfo = base.CreateInfo<DirectoryInfo>(base.FullName);
                }
                return this._FolderInfo;
            }
        }

        protected internal VolumeInfo FolderVolume
        {
            get
            {
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasVolume))
                {
                    try
                    {
                        this.FVolume = this.GetVolume();
                    }
                    catch (NotSupportedException)
                    {
                        base.SetCapability(FileSystemItem.ItemCapability.Unreadable, true);
                    }
                    catch (IOException exception)
                    {
                        if (!base.ProcessNotFoundException(exception))
                        {
                            throw;
                        }
                    }
                    base.SetCapability(FileSystemItem.ItemCapability.HasVolume, true);
                }
                return this.FVolume;
            }
        }

        protected internal override FileSystemInfo Info
        {
            get
            {
                return this.FolderInfo;
            }
        }

        public string Location
        {
            get
            {
                VolumeInfo folderVolume = this.FolderVolume;
                return ((folderVolume != null) ? folderVolume.Location : null);
            }
        }

        protected string Owner
        {
            get
            {
                try
                {
                    return this.FolderInfo.GetAccessControl(AccessControlSections.Owner).GetOwner(typeof(NTAccount)).ToString();
                }
                catch
                {
                    return null;
                }
            }
        }

        public virtual IVirtualFolder Root
        {
            get
            {
                if (this.FRoot == null)
                {
                    string pathRoot;
                    try
                    {
                        pathRoot = Path.GetPathRoot(base.FullName);
                    }
                    catch (ArgumentException)
                    {
                        pathRoot = Path.GetPathRoot(PathHelper.NormalizeInvalidPath(base.FullName));
                    }
                    this.FRoot = (IVirtualFolder) VirtualItem.FromFullName(pathRoot, VirtualItemType.Folder);
                }
                return this.FRoot;
            }
        }

        public DriveType VolumeType
        {
            get
            {
                VolumeInfo folderVolume = this.FolderVolume;
                return ((folderVolume != null) ? folderVolume.DriveType : DriveType.Unknown);
            }
        }

        [CompilerGenerated]
        private sealed class <GetContent>d__c : IEnumerable<FileSystemItem>, IEnumerable, IEnumerator<FileSystemItem>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private FileSystemItem <>2__current;
            public bool <>3__cacheItems;
            public CustomFileSystemFolder <>4__this;
            public IEnumerator<FileSystemInfo> <>7__wrap13;
            private int <>l__initialThreadId;
            public IEnumerable<FileSystemInfo> <ContentEnumeration>5__f;
            public List<FileSystemItem> <FolderContent>5__d;
            public Dictionary<string, int> <FolderContentMap>5__e;
            public FileSystemItem <Item>5__11;
            public FileSystemInfo <NextInfo>5__10;
            public UnreadableFileSystemInfo <Unreadable>5__12;
            public bool cacheItems;

            [DebuggerHidden]
            public <GetContent>d__c(int <>1__state)
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
                bool flag;
                try
                {
                    int num = this.<>1__state;
                    if (num != 0)
                    {
                        if (num != 4)
                        {
                            goto Label_0395;
                        }
                        goto Label_02E7;
                    }
                    this.<>1__state = -1;
                    if (!this.<>4__this.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                    {
                        this.<>4__this.BeginGetContent();
                        this.<FolderContent>5__d = null;
                        this.<FolderContentMap>5__e = null;
                        if (this.cacheItems)
                        {
                            this.<FolderContent>5__d = new List<FileSystemItem>();
                            if (!this.<>4__this.CheckCapability(FileSystemItem.ItemCapability.DisableContentMap))
                            {
                                this.<FolderContentMap>5__e = CustomFileSystemFolder.CreateContentMap();
                            }
                        }
                        this.<ContentEnumeration>5__f = null;
                        if (this.<>4__this.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
                        {
                            try
                            {
                                this.<ContentEnumeration>5__f = this.<>4__this.ContentFolder.GetFileSystemInfos();
                            }
                            catch (RemotingException)
                            {
                                this.<>4__this.ResetInfo();
                            }
                        }
                        if (this.<ContentEnumeration>5__f == null)
                        {
                            this.<ContentEnumeration>5__f = Microsoft.IO.Directory.GetFileSystemInfos(this.<>4__this.ContentFolder.FullName);
                        }
                        this.<>7__wrap13 = this.<ContentEnumeration>5__f.GetEnumerator();
                        this.<>1__state = 2;
                        while (this.<>7__wrap13.MoveNext())
                        {
                            this.<NextInfo>5__10 = this.<>7__wrap13.Current;
                            this.<Unreadable>5__12 = this.<NextInfo>5__10 as UnreadableFileSystemInfo;
                            if (this.<Unreadable>5__12 != null)
                            {
                                this.<Item>5__11 = LocalFileSystemCreator.FromFullName(this.<Unreadable>5__12.FullName, ((this.<Unreadable>5__12.Attributes & FileAttributes.Directory) > 0) ? VirtualItemType.Folder : VirtualItemType.File, this.<>4__this);
                                this.<Item>5__11.SetCapability(FileSystemItem.ItemCapability.Unreadable, true);
                            }
                            else
                            {
                                this.<Item>5__11 = LocalFileSystemCreator.FromFileSystemInfo(this.<NextInfo>5__10, this.<>4__this);
                                if (RemotingServices.IsTransparentProxy(this.<NextInfo>5__10))
                                {
                                    LocalFileSystemCreator.Sponsor.Register(this.<NextInfo>5__10);
                                    this.<Item>5__11.Proxy = this.<>4__this.Proxy;
                                    this.<Item>5__11.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
                                }
                            }
                            this.<Item>5__11.ItemChanged += new EventHandler<VirtualItemChangedEventArgs>(this.<>4__this.OnItemChanged);
                            if (this.<FolderContent>5__d != null)
                            {
                                this.<FolderContent>5__d.Add(this.<Item>5__11);
                                try
                                {
                                    if (this.<FolderContentMap>5__e != null)
                                    {
                                        this.<FolderContentMap>5__e.Add(this.<Item>5__11.ComparableName, this.<FolderContent>5__d.Count - 1);
                                    }
                                }
                                catch (ArgumentException)
                                {
                                    this.<>4__this.SetCapability(FileSystemItem.ItemCapability.DisableContentMap, true);
                                    this.<FolderContentMap>5__e = null;
                                    if (this.<>4__this.ContentWatcher != null)
                                    {
                                        this.<>4__this.ContentWatcher.Dispose();
                                        this.<>4__this.ContentWatcher = null;
                                    }
                                }
                            }
                            this.<>2__current = this.<Item>5__11;
                            this.<>1__state = 4;
                            return true;
                        Label_02E7:
                            this.<>1__state = 2;
                        }
                        this.<>m__Finally14();
                        if ((this.<FolderContent>5__d != null) && !this.<>4__this.CheckCapability(FileSystemItem.ItemCapability.Deleted))
                        {
                            lock (this.<>4__this.ContentLock)
                            {
                                this.<>4__this.Content = this.<FolderContent>5__d;
                                this.<>4__this.ContentMap = this.<FolderContentMap>5__e;
                                this.<>4__this.DeletedContent = null;
                            }
                            this.<>4__this.RaiseCachedContentChanged(EventArgs.Empty);
                        }
                        this.<>4__this.EndGetContent();
                    }
                Label_0395:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<FileSystemItem> IEnumerable<FileSystemItem>.GetEnumerator()
            {
                CustomFileSystemFolder.<GetContent>d__c _c;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    _c = this;
                }
                else
                {
                    _c = new CustomFileSystemFolder.<GetContent>d__c(0) {
                        <>4__this = this.<>4__this
                    };
                }
                _c.cacheItems = this.<>3__cacheItems;
                return _c;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.LocalFile.FileSystemItem>.GetEnumerator();
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
                    case 2:
                    case 4:
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

            FileSystemItem IEnumerator<FileSystemItem>.Current
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

