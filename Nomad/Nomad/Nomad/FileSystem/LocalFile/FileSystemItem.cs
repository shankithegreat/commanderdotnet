namespace Nomad.FileSystem.LocalFile
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Shell;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.Commons.Threading;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Security.Principal;
    using System.Windows.Forms;

    public abstract class FileSystemItem : SlowPropertyProvider, IVirtualItemUI, IVirtualAlternateStreams, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ISerializable, IDisposable
    {
        private string _FullName;
        private string _Name;
        private IFileSystemProxy _Proxy;
        protected static StringComparison ComparisonRule = StringComparison.OrdinalIgnoreCase;
        private const string EntryFullName = "FullName";
        private VirtualItemVisualExtender FExtender;
        private IDictionary<Size, Image> FIcons;
        private IVirtualFolder FParent;
        private int FStoredChangeVector;
        private Action<Tuple<Size, Image, bool>> GetIconCallback;
        private ItemCapability HasCapabilities;
        protected const string LinkExtension = ".lnk";
        protected static VirtualPropertySet NamePropertySet;

        public event EventHandler<VirtualItemChangedEventArgs> ItemChanged;

        static FileSystemItem()
        {
            int[] properties = new int[1];
            NamePropertySet = new VirtualPropertySet(properties);
        }

        protected FileSystemItem(SerializationInfo info, StreamingContext context)
        {
            this._FullName = (string) info.GetValue("FullName", typeof(string));
            this.Initialize();
        }

        protected FileSystemItem(string fullName, IVirtualFolder parent)
        {
            this._FullName = fullName;
            if (parent != null)
            {
                this.Parent = parent;
            }
            this.Initialize();
        }

        private void CacheIcon(Size size, Image icon, bool resetIconCache)
        {
            lock (this.GetIconCallback)
            {
                if (resetIconCache || (this.FIcons == null))
                {
                    this.FIcons = IconCollection.Create();
                }
                this.FIcons[size] = icon;
            }
        }

        protected override bool CacheProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 10:
                case 0x11:
                    return true;
            }
            return base.CacheProperty(propertyId);
        }

        public bool CanMoveTo(IVirtualFolder dest)
        {
            CustomFileSystemFolder folder = dest as CustomFileSystemFolder;
            if (folder == null)
            {
                return false;
            }
            string fullName = folder.FullName;
            string path = Path.Combine(fullName, this.Name);
            return ((Path.GetPathRoot(this.FullName).Equals(Path.GetPathRoot(fullName), ComparisonRule) && !System.IO.File.Exists(path)) && !System.IO.Directory.Exists(path));
        }

        public override bool CanSetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 6:
                case 7:
                case 8:
                case 9:
                    return true;
            }
            return base.CanSetProperty(propertyId);
        }

        public bool CheckAnyCapability(ItemCapability capability)
        {
            return ((this.HasCapabilities & capability) > ItemCapability.None);
        }

        public bool CheckCapability(ItemCapability capability)
        {
            return ((this.HasCapabilities & capability) == capability);
        }

        internal static void CheckItemName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (!((name != string.Empty) && string.IsNullOrEmpty(Path.GetDirectoryName(name))))
            {
                throw new ArgumentException("name is empty or contains directory path");
            }
        }

        private object ConvertDateTime(ItemCapability capability, Func<DateTime> getTime)
        {
            if (this.CheckCapability(capability))
            {
                try
                {
                    DateTime time = getTime();
                    if (time.ToFileTimeUtc() != 0L)
                    {
                        return time;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                this.SetCapability(capability, false);
            }
            return null;
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = null;
            if (!this.CheckAnyCapability(ItemCapability.Unreadable | ItemCapability.Deleted))
            {
                set = base.CreateAvailableSet();
            }
            if (set == null)
            {
                set = new VirtualPropertySet();
            }
            set[0] = true;
            set[2] = true;
            set[6] = true;
            set[7] = this.CheckCapability(ItemCapability.HasCreationTime);
            set[8] = this.CheckCapability(ItemCapability.HasLastWriteTime);
            set[9] = this.CheckCapability(ItemCapability.HasLastAccessTime);
            if ((this.Attributes & FileAttributes.ReparsePoint) > 0)
            {
                set[10] = true;
            }
            set[0x11] = true;
            return set;
        }

        public virtual ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            if (this.CheckCapability(ItemCapability.IsElevated))
            {
                return null;
            }
            return ShellContextMenuHelper.CreateContextMenu(owner, new string[] { this.FullName }, options, onExecuteVerb);
        }

        protected T CreateInfo<T>(string fileName) where T: FileSystemInfo
        {
            FileSystemInfo info;
            if (typeof(T) == typeof(DirectoryInfo))
            {
                info = ((IDirectoryProxy) this.Proxy).Get(fileName);
            }
            else if (typeof(T) == typeof(FileInfo))
            {
                info = ((IFileProxy) this.Proxy).Get(fileName);
            }
            else
            {
                IDirectoryProxy proxy = (IDirectoryProxy) this.Proxy;
                if (PathHelper.HasTrailingDirectorySeparator(fileName) || proxy.Exists(fileName))
                {
                    info = proxy.Get(fileName);
                }
                else
                {
                    IFileProxy proxy2 = (IFileProxy) this.Proxy;
                    if (!proxy2.Exists(fileName))
                    {
                        throw new FileNotFoundException();
                    }
                    info = proxy2.Get(fileName);
                }
            }
            if (RemotingServices.IsTransparentProxy(info))
            {
                LocalFileSystemCreator.Sponsor.Register(info);
            }
            return (T) info;
        }

        internal static IVirtualItem CreateShellLink(IVirtualFolder destFolder, string name, string target)
        {
            if (destFolder == null)
            {
                throw new ArgumentNullException("destFolder");
            }
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if (folder == null)
            {
                throw new ArgumentException("destFolder is not CustomFileSystemFolder");
            }
            using (ShellLink link = new ShellLink())
            {
                link.Path = target;
                CustomFileSystemFile file = folder.CreateFile(name) as CustomFileSystemFile;
                using (Stream stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.SequentialScan, 0L))
                {
                    link.Save(stream);
                }
                return new FileSystemShellLink((FileInfo) file.Info);
            }
        }

        private void DelayedGetIcon(Tuple<Size, Image, bool> state)
        {
            Image icon = this.ExtractIcon(state.Item1, state.Item3);
            if (state.Item2 != icon)
            {
                this.CacheIcon(state.Item1, icon, false);
                this.OnItemChanged(new VirtualItemChangedEventArgs(this, new VirtualPropertySet(new int[] { 0x15 })));
            }
        }

        public virtual void Delete(bool sendToBin)
        {
            if (sendToBin)
            {
                this.Proxy.MoveToRecycleBin(this.ComparableName);
            }
            else
            {
                FileAttributes attributes = this.RefreshedInfo.Attributes;
                if ((attributes & (FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly)) > 0)
                {
                    this.InternalSetAttributes(FileAttributes.Normal | (attributes & (FileAttributes.Encrypted | FileAttributes.Compressed)));
                }
                try
                {
                    this.Info.Delete();
                }
                catch (IOException exception)
                {
                    if (Marshal.GetLastWin32Error() == 5)
                    {
                        throw new UnauthorizedAccessException(exception.Message);
                    }
                    throw;
                }
            }
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, this.ComparableName);
            VirtualItem.RaiseVirtualItemChangedEvent(WatcherChangeTypes.Deleted, this);
        }

        public void Delete(string streamName)
        {
            new AlternateDataStreamInfo(this.FullName, streamName).Delete();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.ResetInfo();
            this.ResetParent();
        }

        public bool ElevateFrom(IElevatable other)
        {
            if (other == null)
            {
                throw new ArgumentNullException();
            }
            if (!this.CheckCapability(ItemCapability.IsElevated))
            {
                FileSystemItem item = other as FileSystemItem;
                if ((item != null) && item.CheckCapability(ItemCapability.IsElevated))
                {
                    this.ResetInfo();
                    this.Proxy = item.Proxy;
                    this.SetCapability(ItemCapability.IsElevated, true);
                    return true;
                }
            }
            return false;
        }

        public bool Equals(IVirtualItem other)
        {
            if (other != this)
            {
                FileSystemItem item = other as FileSystemItem;
                return ((item != null) && string.Equals(this.ComparableName, item.ComparableName, ComparisonRule));
            }
            return true;
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            LocalFileSystemCreator.ExecuteVerb(owner, this.ComparableName, verb);
            return true;
        }

        private Image ExtractIcon(Size size, bool canUseAlpha)
        {
            Image image5;
            Image itemIcon = this.GetItemIcon(size, false);
            Image source = itemIcon;
            if (source == null)
            {
                return source;
            }
            if (VirtualIcon.CheckIconOption(IconOptions.ShowOverlayIcons))
            {
                try
                {
                    Image defaultIcon;
                    if (this.CheckCapability(ItemCapability.Unreadable))
                    {
                        defaultIcon = ImageProvider.Default.GetDefaultIcon(DefaultIcon.OverlayUnreadable, size);
                    }
                    else
                    {
                        defaultIcon = ImageProvider.Default.GetItemOverlay(this.FullName, size);
                    }
                    if (defaultIcon != null)
                    {
                        lock ((image5 = itemIcon))
                        {
                            source = ImageHelper.MergeImages(new Image[] { itemIcon, defaultIcon });
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
            if ((!canUseAlpha || (this.Highlighter == null)) || !this.Highlighter.AlphaBlend)
            {
                return source;
            }
            lock ((image5 = itemIcon))
            {
                return ImageHelper.CreateBlendImage(source, this.Highlighter.BlendColor, this.Highlighter.BlendLevel);
            }
        }

        public override int GetHashCode()
        {
            switch (ComparisonRule)
            {
                case StringComparison.Ordinal:
                    return this.FullName.GetHashCode();

                case StringComparison.OrdinalIgnoreCase:
                    return this.FullName.ToUpper().GetHashCode();
            }
            throw new ApplicationException("Cannot generate hash. Unsupported StringComparison rule.");
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            if ((style & IconStyle.DefaultIcon) > 0)
            {
                return this.GetItemIcon(size, true);
            }
            return this.GetIcon(size, (style & IconStyle.CanUseDelayedExtract) > 0, (style & IconStyle.CanUseAlphaBlending) > 0);
        }

        private Image GetIcon(Size size, bool canUseDelayedExtract, bool canUseAlpha)
        {
            Image itemIcon = null;
            bool flag;
            bool flag2 = false;
            lock (this.GetIconCallback)
            {
                flag = (this.FIcons == null) || !ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Icon);
                if (!flag)
                {
                    flag2 = this.FIcons.TryGetValue(size, out itemIcon);
                }
                else
                {
                    ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Icon);
                }
            }
            if (flag || !flag2)
            {
                bool isSlowIcon = this.IsSlowIcon;
                bool flag4 = (VirtualIcon.CheckIconOption(IconOptions.ExtractIcons) && (!isSlowIcon || (isSlowIcon && !VirtualIcon.CheckIconOption(IconOptions.DisableExtractSlowIcons)))) && ((this.Highlighter == null) || (this.Highlighter.IconType == HighlighterIconType.ExtractedIcon));
                bool flag5 = ((flag4 && canUseDelayedExtract) && (this.ItemChanged != null)) && ((VirtualIcon.DelayedExtractMode == DelayedExtractMode.Always) || (isSlowIcon && (VirtualIcon.DelayedExtractMode == DelayedExtractMode.OnSlowDrivesOnly)));
                if (flag4)
                {
                    if (flag5)
                    {
                        itemIcon = this.GetItemIcon(size, true);
                        this.CacheIcon(size, itemIcon, flag);
                        VirtualIcon.ExtractIconQueue.Value.QueueWeakWorkItem<Tuple<Size, Image, bool>>(this.GetIconCallback, Tuple.Create<Size, Image, bool>(size, itemIcon, canUseAlpha));
                        return itemIcon;
                    }
                    itemIcon = this.ExtractIcon(size, canUseAlpha);
                }
                else
                {
                    if ((this.Highlighter != null) && (this.Highlighter.IconType == HighlighterIconType.HighlighterIcon))
                    {
                        itemIcon = this.Highlighter.GetIcon(size);
                    }
                    else
                    {
                        itemIcon = this.GetItemIcon(size, true);
                    }
                    if ((canUseAlpha && (this.Highlighter != null)) && this.Highlighter.AlphaBlend)
                    {
                        itemIcon = ImageHelper.CreateBlendImage(itemIcon, this.Highlighter.BlendColor, this.Highlighter.BlendLevel);
                    }
                }
                this.CacheIcon(size, itemIcon, flag);
            }
            return itemIcon;
        }

        protected abstract Image GetItemIcon(Size size, bool defaultIcon);
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FullName", this.FullName);
        }

        public override object GetProperty(int propertyId)
        {
            Func<DateTime> getTime = null;
            Func<DateTime> func2 = null;
            Func<DateTime> func3 = null;
            switch (propertyId)
            {
                case 0:
                    return this.Name;

                case 2:
                    return this.Extender.Type;

                case 6:
                    return this.Attributes;

                case 7:
                    if (getTime == null)
                    {
                        getTime = delegate {
                            return this.RefreshedInfo.CreationTime;
                        };
                    }
                    return this.ConvertDateTime(ItemCapability.HasCreationTime, getTime);

                case 8:
                    if (func2 == null)
                    {
                        func2 = delegate {
                            return this.RefreshedInfo.LastWriteTime;
                        };
                    }
                    return this.ConvertDateTime(ItemCapability.HasLastWriteTime, func2);

                case 9:
                    if (func3 == null)
                    {
                        func3 = delegate {
                            return this.RefreshedInfo.LastAccessTime;
                        };
                    }
                    return this.ConvertDateTime(ItemCapability.HasLastAccessTime, func3);

                case 10:
                    if ((this.Attributes & FileAttributes.ReparsePoint) <= 0)
                    {
                        return null;
                    }
                    return ReparsePoint.GetTarget(this.FullName);

                case 0x11:
                    return AlternateDataStreamInfo.GetStreams(this.FullName).Count<AlternateDataStreamInfo>();
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 0:
                case 2:
                case 6:
                    return PropertyAvailability.Normal;

                case 7:
                    return (this.CheckCapability(ItemCapability.HasCreationTime) ? PropertyAvailability.Normal : PropertyAvailability.None);

                case 8:
                    return (this.CheckCapability(ItemCapability.HasLastWriteTime) ? PropertyAvailability.Normal : PropertyAvailability.None);

                case 9:
                    return (this.CheckCapability(ItemCapability.HasLastAccessTime) ? PropertyAvailability.Normal : PropertyAvailability.None);
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public IEnumerable<string> GetStreamNames()
        {
            List<string> list = new List<string>();
            foreach (AlternateDataStreamInfo info in this.Proxy.EnumerateAlternateStreams(this.FullName))
            {
                if (!info.IsDefault)
                {
                    list.Add(info.StreamName);
                }
            }
            return list;
        }

        private void Initialize()
        {
            this.GetIconCallback = new Action<Tuple<Size, Image, bool>>(this.DelayedGetIcon);
            this.SetCapability(ItemCapability.HasLastAccessTime | ItemCapability.HasLastWriteTime | ItemCapability.HasCreationTime, true);
            this.FStoredChangeVector = ChangeVector.Value;
        }

        protected override object InternalClone()
        {
            FileSystemItem item = (FileSystemItem) base.InternalClone();
            item._FullName = this._FullName;
            item.Initialize();
            item._Proxy = this._Proxy;
            item.SetCapability(ItemCapability.IsElevated, this.CheckCapability(ItemCapability.IsElevated));
            return item;
        }

        protected void InternalSetAttributes(FileAttributes attributes)
        {
            try
            {
                this.Info.Attributes = attributes;
            }
            catch (ArgumentException exception)
            {
                if (Marshal.GetLastWin32Error() == 5)
                {
                    throw new UnauthorizedAccessException(exception.Message);
                }
                throw;
            }
        }

        protected IVirtualItem MoveTo(IVirtualFolder dest, Action<string> moveToHandler)
        {
            if (dest == null)
            {
                throw new ArgumentNullException("dest");
            }
            CustomFileSystemFolder folder = dest as CustomFileSystemFolder;
            if (folder == null)
            {
                throw new ArgumentException("dest is not CustomFileSystemFolder");
            }
            FileSystemItem item = (FileSystemItem) this.InternalClone();
            string str = Path.Combine(folder.FullName, this.Name);
            moveToHandler(str);
            this.ResetParent();
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, item.ComparableName);
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, str);
            return item;
        }

        protected void OnItemChanged(VirtualItemChangedEventArgs e)
        {
            if (this.ItemChanged != null)
            {
                this.ItemChanged(this, e);
            }
            VirtualItem.RaiseVirtualItemChangedEvent(e);
        }

        protected override void OnSlowPropertyComplete(int propertyId, object propertyValue)
        {
            base.OnSlowPropertyComplete(propertyId, propertyValue);
            this.OnItemChanged(new VirtualItemChangedEventArgs(this, new VirtualPropertySet(new int[] { propertyId })));
        }

        public Stream Open(string streamName, FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            AlternateDataStreamInfo alternateStream;
            if (this.CheckCapability(ItemCapability.IsElevated))
            {
                alternateStream = this.Proxy.GetAlternateStream(this.FullName, streamName);
                options &= ~FileOptions.Asynchronous;
            }
            else
            {
                alternateStream = new AlternateDataStreamInfo(this.FullName, streamName);
            }
            return alternateStream.Open(mode, access, share, options);
        }

        protected bool ProcessNotFoundException(IOException e)
        {
            bool flag = false;
            if (!flag)
            {
                if (this.Info is FileInfo)
                {
                    FileNotFoundException exception = e as FileNotFoundException;
                    flag = (exception != null) && string.Equals(this.ComparableName, exception.FileName, ComparisonRule);
                }
                else
                {
                    flag = (this.Info is DirectoryInfo) && (e is DirectoryNotFoundException);
                }
            }
            if (flag)
            {
                this.SetDeletedCapability(true);
            }
            return flag;
        }

        protected internal void RefreshInfo()
        {
            this.Info.Refresh();
            this.SetCapability(ItemCapability.ItemRefreshNeeded, false);
        }

        protected internal virtual void ResetInfo()
        {
            this._Proxy = null;
            this.SetCapability(ItemCapability.IsElevated, false);
        }

        protected void ResetParent()
        {
            this.SetCapability(ItemCapability.IsParentReal | ItemCapability.HasParent, false);
            this.FParent = null;
        }

        protected internal virtual void ResetVisualCache()
        {
            this._Name = null;
            lock (this.GetIconCallback)
            {
                this.FIcons = null;
                this.FExtender = null;
                this.SetCapability(ItemCapability.HasExtender, false);
            }
            base.ClearCache();
        }

        protected internal void SetCapability(ItemCapability capability, bool value)
        {
            if (value)
            {
                this.HasCapabilities |= capability;
            }
            else
            {
                this.HasCapabilities &= ~capability;
            }
            if (!(value || ((capability & ItemCapability.HasParent) <= ItemCapability.None)))
            {
                this.HasCapabilities &= ~ItemCapability.IsParentReal;
                this.FParent = null;
            }
        }

        protected virtual void SetDeletedCapability(bool value)
        {
            this.SetCapability(ItemCapability.Deleted, value);
            if (value)
            {
                this.OnItemChanged(new VirtualItemChangedEventArgs(WatcherChangeTypes.Deleted, this));
                base.ResetAvailableSet();
            }
        }

        protected void SetName(string newName, Action<string> moveToHandler)
        {
            if (!this.CheckAnyCapability(ItemCapability.Unreadable | ItemCapability.Deleted))
            {
                CheckItemName(newName);
                string comparableName = this.ComparableName;
                moveToHandler(Path.Combine(Path.GetDirectoryName(comparableName), newName));
                this.ResetVisualCache();
                LocalFileSystemCreator.RaiseFileChangedEvent(comparableName, newName);
            }
        }

        public override void SetProperty(int propertyId, object value)
        {
            if (propertyId == 6)
            {
                this.Attributes = (FileAttributes) value;
            }
            else
            {
                base.SetProperty(propertyId, value);
            }
            this.ResetVisualCache();
        }

        public virtual void ShowProperties(IWin32Window owner)
        {
            bool flag = false;
            if (!this.CheckCapability(ItemCapability.IsElevated))
            {
                flag = ShellContextMenuHelper.ExecuteVerb(owner, "properties", new string[] { this.FullName });
            }
            if (!flag)
            {
                using (PropertiesDialog dialog = new PropertiesDialog())
                {
                    dialog.Execute(owner, new IVirtualItem[] { this });
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.GetType().Name, this.FullName);
        }

        public FileAttributes Attributes
        {
            virtual get
            {
                try
                {
                    FileAttributes attributes = this.RefreshedInfo.Attributes;
                    if (attributes < 0)
                    {
                        this.RefreshInfo();
                        attributes = this.Info.Attributes;
                    }
                    return attributes;
                }
                catch (RemotingException)
                {
                    this.ResetInfo();
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException exception)
                {
                    if (!this.ProcessNotFoundException(exception))
                    {
                        Nomad.Trace.Error.TraceException(TraceEventType.Warning, exception);
                    }
                }
                return 0;
            }
            private set
            {
                if ((value & (FileAttributes.Encrypted | FileAttributes.Compressed)) == (FileAttributes.Encrypted | FileAttributes.Compressed))
                {
                    throw new IOException("Cannot set compressed and encrypted state simultaneously");
                }
                if ((value & (FileAttributes.Encrypted | FileAttributes.Compressed)) > 0)
                {
                    this.SetCapability(ItemCapability.ItemRefreshNeeded, true);
                }
                FileAttributes attributes = this.RefreshedInfo.Attributes;
                if (((value ^ attributes) & (FileAttributes.Encrypted | FileAttributes.Compressed)) > 0)
                {
                    if (((value & FileAttributes.Encrypted) > 0) && this.Compressed)
                    {
                        this.Compressed = false;
                    }
                    else if (((value & FileAttributes.Compressed) > 0) && this.Encrypted)
                    {
                        this.Encrypted = false;
                    }
                    this.Encrypted = (value & FileAttributes.Encrypted) > 0;
                    this.RefreshInfo();
                    this.Compressed = (value & FileAttributes.Compressed) > 0;
                }
                this.InternalSetAttributes(value);
                if (attributes != value)
                {
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                }
            }
        }

        public override VirtualPropertySet AvailableProperties
        {
            get
            {
                if (this.CheckAnyCapability(ItemCapability.Unreadable | ItemCapability.Deleted))
                {
                    return NamePropertySet;
                }
                return base.AvailableProperties;
            }
        }

        public bool CanElevate
        {
            get
            {
                if (!(OS.IsWinVista && !this.CheckCapability(ItemCapability.IsElevated)))
                {
                    return false;
                }
                switch (OS.ElevationType)
                {
                    case ElevationType.Full:
                        return false;

                    case ElevationType.Limited:
                        return true;
                }
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return !principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public virtual bool CanSendToBin
        {
            get
            {
                VolumeInfo itemVolume = this.ItemVolume;
                if (itemVolume != null)
                {
                    switch (itemVolume.DriveType)
                    {
                        case DriveType.Removable:
                        case DriveType.Fixed:
                            return true;
                    }
                }
                return false;
            }
        }

        public virtual string ComparableName
        {
            get
            {
                return this._FullName;
            }
        }

        protected virtual bool Compressed
        {
            get
            {
                return ((this.Attributes & FileAttributes.Compressed) > 0);
            }
            set
            {
            }
        }

        protected virtual bool Encrypted
        {
            get
            {
                return ((this.Attributes & FileAttributes.Encrypted) > 0);
            }
            set
            {
            }
        }

        public virtual bool Exists
        {
            get
            {
                if (this.CheckAnyCapability(ItemCapability.Unreadable | ItemCapability.Deleted))
                {
                    return false;
                }
                try
                {
                    this.RefreshInfo();
                }
                catch (RemotingException)
                {
                    this.ResetInfo();
                }
                return this.Info.Exists;
            }
        }

        protected VirtualItemVisualExtender Extender
        {
            get
            {
                lock (this.GetIconCallback)
                {
                    if (this.FExtender == null)
                    {
                        this.FExtender = new VirtualItemVisualExtender(this);
                        this.SetCapability(ItemCapability.HasExtender, true);
                    }
                    return this.FExtender;
                }
            }
        }

        public string FullName
        {
            get
            {
                return this._FullName;
            }
            protected set
            {
                if (this._FullName != value)
                {
                    this._FullName = value;
                    this.ResetInfo();
                }
            }
        }

        public bool HasAlternateStreams
        {
            get
            {
                return (this.IsSupported && this.Proxy.EnumerateAlternateStreams(this.FullName).Any<AlternateDataStreamInfo>(delegate (AlternateDataStreamInfo x) {
                    return !x.IsDefault;
                }));
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return this.Extender.Highlighter;
            }
        }

        protected internal abstract FileSystemInfo Info { get; }

        public virtual bool IsSlowIcon
        {
            get
            {
                VolumeInfo itemVolume = this.ItemVolume;
                return ((itemVolume == null) || itemVolume.IsSlowDrive);
            }
        }

        public bool IsSupported
        {
            get
            {
                if (!OS.IsWinNT)
                {
                    return false;
                }
                VolumeInfo itemVolume = this.ItemVolume;
                return ((itemVolume != null) && ((itemVolume.Capabilities & VolumeCapabilities.FileNamedStreams) > ((VolumeCapabilities) 0)));
            }
        }

        public override object this[int propertyId]
        {
            get
            {
                if (this.CheckAnyCapability(ItemCapability.Unreadable | ItemCapability.Deleted))
                {
                    return ((propertyId == 0) ? this.Name : null);
                }
                try
                {
                    try
                    {
                        switch (propertyId)
                        {
                            case 0:
                                return this.Name;

                            case 1:
                            case 2:
                            case 7:
                            case 8:
                            case 9:
                                return this.GetProperty(propertyId);

                            case 6:
                                return this.Attributes;
                        }
                        return base[propertyId];
                    }
                    catch (Exception exception)
                    {
                        exception.Data["FullName"] = this.FullName;
                        exception.Data["Property"] = VirtualProperty.Get(propertyId).PropertyName;
                        throw;
                    }
                }
                catch (RemotingException)
                {
                    this.ResetInfo();
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException exception2)
                {
                    if (!this.ProcessNotFoundException(exception2))
                    {
                        Nomad.Trace.Error.TraceException(TraceEventType.Warning, exception2);
                    }
                }
                return null;
            }
        }

        protected VolumeInfo ItemVolume
        {
            get
            {
                CustomFileSystemFolder parent = this.Parent as CustomFileSystemFolder;
                if ((parent != null) && this.CheckCapability(ItemCapability.IsParentReal))
                {
                    return parent.FolderVolume;
                }
                try
                {
                    string directoryName = Path.GetDirectoryName(this.ComparableName);
                    if (directoryName != null)
                    {
                        return VolumeCache.FromPath(directoryName);
                    }
                    return VolumeCache.Get(Path.GetPathRoot(this.ComparableName));
                }
                catch (IOException exception)
                {
                    if (!this.ProcessNotFoundException(exception))
                    {
                        throw;
                    }
                }
                return null;
            }
        }

        public virtual string Name
        {
            get
            {
                if (this._Name == null)
                {
                    string comparableName = this.ComparableName;
                    this._Name = comparableName.Substring(comparableName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                }
                return this._Name;
            }
        }

        public virtual IVirtualFolder Parent
        {
            get
            {
                if (!this.CheckCapability(ItemCapability.HasParent))
                {
                    string directoryName = Path.GetDirectoryName(this.ComparableName);
                    if (directoryName != null)
                    {
                        this.FParent = (IVirtualFolder) VirtualItem.FromFullName(directoryName, VirtualItemType.Folder);
                    }
                    this.SetCapability(ItemCapability.IsParentReal | ItemCapability.HasParent, true);
                }
                return this.FParent;
            }
            internal set
            {
                this.FParent = value;
                this.SetCapability(ItemCapability.HasParent, true);
                this.SetCapability(ItemCapability.IsParentReal, (this.FParent != null) && this.FParent.IsChild(this));
            }
        }

        protected internal IFileSystemProxy Proxy
        {
            get
            {
                return (this._Proxy ?? FileSystemProxy.Default);
            }
            set
            {
                if (value == FileSystemProxy.Default)
                {
                    value = null;
                }
                if ((this._Proxy != null) && (value != null))
                {
                    throw new InvalidOperationException();
                }
                this._Proxy = value;
            }
        }

        protected internal FileSystemInfo RefreshedInfo
        {
            get
            {
                FileSystemInfo info = this.Info;
                if (this.CheckCapability(ItemCapability.ItemRefreshNeeded))
                {
                    info.Refresh();
                }
                this.SetCapability(ItemCapability.ItemRefreshNeeded, false);
                return info;
            }
        }

        public virtual string ShortName
        {
            get
            {
                return this.Name;
            }
        }

        public virtual string ToolTip
        {
            get
            {
                return this.Extender.ToolTip;
            }
        }

        [Flags]
        public enum ItemCapability
        {
            CheckNetworkShare = 0x200000,
            Deleted = 0x100,
            DisableContentMap = 0x400000,
            GlobalFileChangedAssigned = 0x400,
            GlobalFolderChangedAssigned = 0x20000,
            HasContentFolder = 0x2000000,
            HasCreationTime = 0x2000,
            HasExtender = 2,
            HasExtension = 0x8000000,
            HasLastAccessTime = 0x8000,
            HasLastWriteTime = 0x4000,
            HasParent = 1,
            HasShellFolderShortcut = 0x80,
            HasShellItem = 0x4000000,
            HasSize = 8,
            HasTarget = 4,
            HasThumbnail = 0x10,
            HasVolume = 0x40000,
            IsElevated = 0x1000000,
            IsParentReal = 0x20,
            IsShellFolderShortcut = 0x40,
            IsShellLink = 0x800,
            IsUrlLink = 0x1000,
            ItemRefreshNeeded = 0x800000,
            None = 0,
            QueryRemoveAssigned = 0x80000,
            Unreadable = 0x10000,
            UseTargetIcon = 0x200,
            VolumeEventsAssigned = 0x100000
        }
    }
}

