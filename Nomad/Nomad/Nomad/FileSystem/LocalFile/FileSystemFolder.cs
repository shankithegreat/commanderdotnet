namespace Nomad.FileSystem.LocalFile
{
    using Microsoft.IO;
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class FileSystemFolder : EjectableFileSystemFolder, IChangeVirtualItem, IPersistVirtualItem, IVirtualItem, ISimpleItem, IEquatable<IVirtualItem>, ISetVirtualProperty, IGetVirtualProperty, IElevatableFrom, IElevatable, ICreateVirtualFile, ICreateVirtualFolder, ICloneable
    {
        private const string EntryContentFolder = "ContentFolder";
        private const string EntryIsShellFolderShortcut = "IsShellFolderShortcut";
        private DirectoryInfo FContentFolder;
        private FileSystemEventHandler FolderGlobalHandler;
        private FileSystemWatcher FolderWatcher;
        private WeakReference FThumbnail;
        public static bool ProcessFolderShortcuts;

        public FileSystemFolder(DirectoryInfo info) : this(info, null)
        {
        }

        public FileSystemFolder(string folderPath) : this(folderPath, null)
        {
        }

        public FileSystemFolder(DirectoryInfo Info, IVirtualFolder parent) : base(Info, parent)
        {
            base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, true);
        }

        protected FileSystemFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (name != null)
                {
                    if (!(name == "IsShellFolderShortcut"))
                    {
                        if (name == "ContentFolder")
                        {
                            goto Label_0066;
                        }
                    }
                    else
                    {
                        base.SetCapability(FileSystemItem.ItemCapability.IsShellFolderShortcut, (bool) current.Value);
                        base.SetCapability(FileSystemItem.ItemCapability.HasShellFolderShortcut, true);
                    }
                }
                continue;
            Label_0066:
                this.FContentFolder = (DirectoryInfo) current.Value;
            }
            base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, true);
        }

        public FileSystemFolder(string folderPath, IVirtualFolder Parent) : base(folderPath, Parent)
        {
            base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, true);
        }

        protected override bool CacheProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 10:
                case 11:
                    return true;
            }
            return base.CacheProperty(propertyId);
        }

        public override bool CanSetProperty(int property)
        {
            return ((property == 11) || base.CanSetProperty(property));
        }

        public object Clone()
        {
            return this.InternalClone();
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[11] = base.IsPropertyCached(11) || System.IO.File.Exists(this.DesktopIniPath);
            set[10] = ((this.Attributes & FileAttributes.ReparsePoint) > 0) || this.IsShellFolderShortcut;
            set[0x15] = base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail);
            return set;
        }

        public override void Delete(bool sendToBin)
        {
            if (!(!this.IsShellFolderShortcut || sendToBin))
            {
                IFileProxy proxy = (IFileProxy) base.Proxy;
                proxy.Delete(this.DesktopIniPath);
                proxy.Delete(Path.Combine(base.FullName, "target.lnk"));
            }
            base.Delete(sendToBin);
        }

        protected override void DisposeWatcher(bool disposing)
        {
            base.DisposeWatcher(disposing);
            if (this.FolderWatcher != null)
            {
                this.FolderWatcher.Dispose();
            }
            this.FolderWatcher = null;
            if (this.FolderGlobalHandler != null)
            {
                LocalFileSystemCreator.GlobalFileChanged -= this.FolderGlobalHandler;
                base.SetCapability(FileSystemItem.ItemCapability.GlobalFolderChangedAssigned, false);
            }
            this.FolderGlobalHandler = null;
        }

        protected override void EnableWatcher()
        {
            base.EnableWatcher();
            if (!base.CheckCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
            {
                if (this.FolderWatcher != null)
                {
                    bool flag = false;
                    try
                    {
                        this.FolderWatcher.EnableRaisingEvents = true;
                    }
                    catch (ArgumentException)
                    {
                        flag = true;
                    }
                    catch (FileNotFoundException)
                    {
                        flag = true;
                    }
                    catch (Exception exception)
                    {
                        Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        flag = true;
                    }
                    if (flag)
                    {
                        this.FolderWatcher.Dispose();
                        this.FolderWatcher = null;
                    }
                }
                if (!((this.FolderGlobalHandler == null) || base.CheckCapability(FileSystemItem.ItemCapability.GlobalFolderChangedAssigned)))
                {
                    LocalFileSystemCreator.GlobalFileChanged += this.FolderGlobalHandler;
                    base.SetCapability(FileSystemItem.ItemCapability.GlobalFolderChangedAssigned, true);
                }
            }
        }

        private DirectoryInfo GetContentFolder()
        {
            try
            {
                if (!(ProcessFolderShortcuts && ((base.RefreshedInfo.Attributes & FileAttributes.ReadOnly) != 0)))
                {
                    return null;
                }
                IFileProxy proxy = (IFileProxy) base.Proxy;
                FileInfo info = proxy.Get(this.DesktopIniPath);
                FileInfo info2 = proxy.Get(Path.Combine(base.FullName, "target.lnk"));
                if ((info.Exists && ((info.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == (FileAttributes.System | FileAttributes.Hidden))) && info2.Exists)
                {
                    bool flag = false;
                    using (TextReader reader = info.OpenText())
                    {
                        flag = string.Equals(Ini.ReadValue(reader, ".ShellClassInfo", "CLSID2"), "{0AFACED1-E828-11D1-9187-B532F1E9575D}", StringComparison.OrdinalIgnoreCase);
                    }
                    if (flag)
                    {
                        using (Stream stream = info2.OpenRead())
                        {
                            using (ShellLink link = new ShellLink(stream))
                            {
                                string path = link.Path;
                                if (path.Length > 0)
                                {
                                    IDirectoryProxy proxy2 = (IDirectoryProxy) base.Proxy;
                                    if (proxy2.Exists(path))
                                    {
                                        return base.CreateInfo<DirectoryInfo>(path);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            if (defaultIcon || base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
            {
                return ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, size);
            }
            return ImageProvider.Default.GetFolderIcon(base.FullName, size);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (base.CheckCapability(FileSystemItem.ItemCapability.HasShellFolderShortcut))
            {
                info.AddValue("IsShellFolderShortcut", base.CheckCapability(FileSystemItem.ItemCapability.IsShellFolderShortcut));
                if (this.IsShellFolderShortcut)
                {
                    info.AddValue("ContentFolder", this.ContentFolder);
                }
            }
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 10:
                    return this.Target;

                case 11:
                    return this.Description;

                case 0x15:
                {
                    object thumbnail = null;
                    if (base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail))
                    {
                        thumbnail = this.Thumbnail;
                    }
                    if (!((thumbnail != null) || base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail)))
                    {
                        thumbnail = base.GetProperty(propertyId);
                    }
                    return thumbnail;
                }
                case 2:
                    if (!base.Extender.HasType && ((base.RefreshedInfo.Attributes & FileAttributes.ReparsePoint) > 0))
                    {
                        ReparsePointInfo reparsePointInfo = ReparsePoint.GetReparsePointInfo(base.FullName);
                        if (reparsePointInfo != null)
                        {
                            switch (reparsePointInfo.ReparseType)
                            {
                                case ReparseType.JunctionPoint:
                                    base.Extender.Type = Resources.sItemTypeReparsePoint;
                                    break;

                                case ReparseType.MountPoint:
                                    base.Extender.Type = Resources.sItemTypeMountPoint;
                                    break;
                            }
                        }
                    }
                    break;

                default:
                    return base.GetProperty(propertyId);
            }
            return base.Extender.Type;
        }

        protected override void InitializeWatcher(string path)
        {
            base.InitializeWatcher(path);
            VolumeInfo folderVolume = base.FolderVolume;
            if ((folderVolume != null) && !folderVolume.IsReadOnly)
            {
                if ((((this.FolderWatcher == null) && (Environment.OSVersion.Platform == PlatformID.Win32NT)) && !base.CheckCapability(FileSystemItem.ItemCapability.IsElevated)) && (!folderVolume.IsSlowDrive || (CustomFileSystemFolder.SlowVolumeAutoRefresh != AutoRefreshMode.None)))
                {
                    string directoryName = Path.GetDirectoryName(PathHelper.ExcludeTrailingDirectorySeparator(path));
                    if (directoryName != null)
                    {
                        this.FolderWatcher = new FileSystemWatcher();
                        try
                        {
                            this.FolderWatcher.BeginInit();
                            this.FolderWatcher.Path = directoryName;
                            this.FolderWatcher.Filter = Path.GetFileName(PathHelper.ExcludeTrailingDirectorySeparator(path));
                            this.FolderWatcher.NotifyFilter = NotifyFilters.DirectoryName;
                            this.FolderWatcher.Deleted += new FileSystemEventHandler(this.OnFolderDeleted);
                            this.FolderWatcher.Renamed += new RenamedEventHandler(this.OnFolderDeleted);
                            this.FolderWatcher.EndInit();
                        }
                        catch (ArgumentException)
                        {
                            this.FolderWatcher.Dispose();
                            this.FolderWatcher = null;
                        }
                    }
                }
                if ((this.FolderWatcher == null) && (this.FolderGlobalHandler == null))
                {
                    this.FolderGlobalHandler = new FileSystemEventHandler(this.OnGlobalFolderChanged);
                }
            }
        }

        protected override object InternalClone()
        {
            FileSystemFolder folder = (FileSystemFolder) base.InternalClone();
            folder.FThumbnail = this.FThumbnail;
            folder.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail));
            return folder;
        }

        public IVirtualItem MoveTo(IVirtualFolder dest)
        {
            return base.MoveTo(dest, delegate (string destDirName) {
                base.FolderInfo.MoveTo(destDirName);
            });
        }

        private void OnFolderDeleted(object source, FileSystemEventArgs e)
        {
            this.ResetVisualCache();
            base.RaiseChanged(WatcherChangeTypes.Deleted, this);
        }

        private void OnGlobalFolderChanged(object source, FileSystemEventArgs e)
        {
            if (string.Equals(e.FullPath, this.ComparableName, FileSystemItem.ComparisonRule))
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Deleted:
                    case WatcherChangeTypes.Renamed:
                        this.OnFolderDeleted(source, e);
                        break;
                }
            }
        }

        protected internal override void ResetInfo()
        {
            base.ResetInfo();
            this.FContentFolder = null;
            base.SetCapability(FileSystemItem.ItemCapability.HasContentFolder, false);
        }

        private void SetName(string newName)
        {
            if (string.Equals(this.Name, newName, StringComparison.OrdinalIgnoreCase))
            {
                if (!base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                {
                    FileSystemItem.CheckItemName(newName);
                    string folderPath = Microsoft.IO.Directory.Rename(this.ComparableName, newName);
                    base.FullName = PathHelper.IncludeTrailingDirectorySeparator(folderPath);
                    LocalFileSystemCreator.RaiseFileChangedEvent(folderPath, newName);
                }
            }
            else
            {
                base.SetName(newName, delegate (string destDirName) {
                    base.FolderInfo.MoveTo(destDirName);
                });
            }
        }

        public override void SetProperty(int propertyId, object value)
        {
            if (propertyId == 11)
            {
                this.Description = (string) value;
            }
            else
            {
                base.SetProperty(propertyId, value);
            }
        }

        public override FileAttributes Attributes
        {
            get
            {
                return (base.Attributes | (this.IsShellFolderShortcut ? 0x400 : 0));
            }
        }

        protected override DirectoryInfo ContentFolder
        {
            get
            {
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasContentFolder))
                {
                    this.FContentFolder = this.GetContentFolder();
                    base.SetCapability(FileSystemItem.ItemCapability.HasContentFolder, true);
                }
                return (this.FContentFolder ?? base.ContentFolder);
            }
        }

        protected string Description
        {
            get
            {
                string str = null;
                string desktopIniPath = this.DesktopIniPath;
                if (System.IO.File.Exists(desktopIniPath))
                {
                    try
                    {
                        using (TextReader reader = System.IO.File.OpenText(desktopIniPath))
                        {
                            str = Ini.ReadValue(reader, ".ShellClassInfo", "InfoTip");
                        }
                        if (!(string.IsNullOrEmpty(str) || (str[0] != '@')))
                        {
                            str = null;
                        }
                    }
                    catch
                    {
                    }
                }
                return str;
            }
            set
            {
                WatcherChangeTypes deleted;
                bool flag = string.IsNullOrEmpty(value);
                PersistentIni ini = new PersistentIni(this.DesktopIniPath);
                ini.Read();
                ini.Set(".ShellClassInfo", "InfoTip", flag ? null : value);
                if (System.IO.File.Exists(ini.FileName))
                {
                    System.IO.File.SetAttributes(ini.FileName, FileAttributes.Normal);
                    if (!ini.HasValues)
                    {
                        System.IO.File.Delete(ini.FileName);
                        deleted = WatcherChangeTypes.Deleted;
                    }
                    else
                    {
                        deleted = WatcherChangeTypes.Changed;
                    }
                }
                else
                {
                    deleted = WatcherChangeTypes.Created;
                }
                if (ini.HasValues)
                {
                    ini.CompactWrite = true;
                    ini.Write();
                    System.IO.File.SetAttributes(ini.FileName, FileAttributes.System | FileAttributes.Hidden);
                }
                LocalFileSystemCreator.RaiseFileChangedEvent(deleted, ini.FileName);
                base.AddPropertyToCache(11, flag ? null : value);
                base.Extender.ToolTip = null;
            }
        }

        protected string DesktopIniPath
        {
            get
            {
                return Path.Combine(base.FullName, "desktop.ini");
            }
        }

        private bool IsShellFolderShortcut
        {
            get
            {
                if (base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                {
                    return false;
                }
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasShellFolderShortcut))
                {
                    base.SetCapability(FileSystemItem.ItemCapability.IsShellFolderShortcut, !string.Equals(this.ComparableName, PathHelper.ExcludeTrailingDirectorySeparator(this.ContentFolder.FullName), FileSystemItem.ComparisonRule));
                    base.SetCapability(FileSystemItem.ItemCapability.HasShellFolderShortcut, true);
                }
                return base.CheckCapability(FileSystemItem.ItemCapability.IsShellFolderShortcut);
            }
        }

        string IChangeVirtualItem.Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.SetName(value);
            }
        }

        protected string Target
        {
            get
            {
                if ((base.RefreshedInfo.Attributes & FileAttributes.ReparsePoint) > 0)
                {
                    ReparsePointInfo reparsePointInfo = ReparsePoint.GetReparsePointInfo(base.FullName);
                    switch (reparsePointInfo.ReparseType)
                    {
                        case ReparseType.JunctionPoint:
                        case ReparseType.SymbolicLink:
                            return reparsePointInfo.Target;

                        case ReparseType.MountPoint:
                        {
                            string defaultPath = VolumeCache.Get(reparsePointInfo.Target).GetDefaultPath();
                            if (!PathHelper.IsRootPath(defaultPath))
                            {
                                goto Label_009B;
                            }
                            return defaultPath;
                        }
                    }
                }
                else if (this.IsShellFolderShortcut)
                {
                    return this.ContentFolder.FullName;
                }
            Label_009B:
                return null;
            }
        }

        protected Image Thumbnail
        {
            get
            {
                if ((this.FThumbnail != null) && this.FThumbnail.IsAlive)
                {
                    return (Image) this.FThumbnail.Target;
                }
                Image target = null;
                try
                {
                    string path = Path.Combine(base.FullName, "folder.jpg");
                    if (System.IO.File.Exists(path) && System.IO.File.GetAttributes(path).CheckAttribute((FileAttributes.System | FileAttributes.Hidden)))
                    {
                        using (Image image2 = Image.FromFile(path))
                        {
                            target = new Bitmap(image2);
                        }
                    }
                    else
                    {
                        base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, false);
                    }
                }
                catch
                {
                    base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, false);
                }
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail))
                {
                    base.ResetAvailableSet();
                }
                if (target != null)
                {
                    this.FThumbnail = new WeakReference(target);
                }
                return target;
            }
        }
    }
}

