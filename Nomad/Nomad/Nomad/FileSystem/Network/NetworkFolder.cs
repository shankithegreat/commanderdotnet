namespace Nomad.FileSystem.Network
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Microsoft.Win32.Network;
    using Microsoft.Win32.SafeHandles;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class NetworkFolder : CustomAsyncFolder, IPersistVirtualItem, IAsyncVirtualFolder, IVirtualCachedFolder, IVirtualFolder, IDisposable, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICreateVirtualLink, ISetOwnerWindow, ISerializable, IGetVirtualRoot
    {
        private const string CategoryNetworkFolder = "NetworkFolder";
        private const int DefaultBufferSize = 0x4000;
        public const string EntireNetworkParseName = "EntireNetwork";
        private const string EntryResource = "Resource";
        private VirtualPropertySet FAvailableSet;
        private IDictionary<Size, Image> FIcons;
        private IWin32Window FOwner;
        private IVirtualFolder FParent;
        private NETRESOURCE FResource;
        private SafeShellItem FShellItem;
        private int FStoredChangeVector;
        private string FToolTip;
        private NetworkField HasFields;
        private static Dictionary<NetworkFolder, IEnumerable<IVirtualItem>> NetworkCache = new Dictionary<NetworkFolder, IEnumerable<IVirtualItem>>();

        public event EventHandler<VirtualItemChangedEventArgs> OnChanged;

        internal NetworkFolder()
        {
            this.FResource.lpLocalName = ".";
            this.FResource.lpRemoteName = ".";
            this.Initialize(NetworkField.HasParent | NetworkField.IsRoot | NetworkField.ResourceValid, null);
        }

        internal NetworkFolder(NETRESOURCE resource, IVirtualFolder parent)
        {
            this.FResource = resource;
            this.Initialize(NetworkField.ResourceValid, parent);
        }

        protected NetworkFolder(SerializationInfo info, StreamingContext context)
        {
            this.FResource = (NETRESOURCE) info.GetValue("Resource", typeof(NETRESOURCE));
            if ((this.FResource.lpLocalName == ".") && (this.FResource.lpRemoteName == "."))
            {
                this.Initialize(NetworkField.HasParent | NetworkField.IsRoot | NetworkField.ResourceValid, null);
            }
            else
            {
                IVirtualFolder parent = null;
                if (this.FResource.dwDisplayType == RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK)
                {
                    parent = NetworkFileSystemCreator.NetworkRoot;
                }
                this.Initialize(NetworkField.ResourceValid, parent);
            }
        }

        public NetworkFolder(string computerName, IVirtualFolder parent)
        {
            this.FResource.lpRemoteName = PathHelper.ExcludeTrailingDirectorySeparator(computerName);
            this.FResource.dwType = RESOURCETYPE.RESOURCETYPE_DISK;
            this.FResource.dwDisplayType = RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER;
            this.FResource.dwUsage = RESOURCEUSAGE.RESOURCEUSAGE_CONTAINER;
            this.Initialize(0, parent);
        }

        public override IAsyncResult BeginGetContent()
        {
            base.StopAsync();
            lock (NetworkCache)
            {
                IEnumerable<IVirtualItem> enumerable;
                if (NetworkCache.TryGetValue(this, out enumerable))
                {
                    base.Content = new HashList<IVirtualItem>(enumerable);
                    return new AsyncFolderResult(this);
                }
            }
            return base.BeginGetContent();
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            if (!(destFolder is CustomFileSystemFolder))
            {
                return LinkType.None;
            }
            switch (this.Resource.dwDisplayType)
            {
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN:
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER:
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE:
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK:
                    return LinkType.Default;
            }
            return (this.CheckField(NetworkField.IsRoot) ? LinkType.Default : LinkType.None);
        }

        protected bool CheckField(NetworkField field)
        {
            return ((this.HasFields & field) == field);
        }

        public override void ClearContentCache()
        {
            lock (NetworkCache)
            {
                NetworkCache.Remove(this);
            }
            base.RaiseCacheContentChanged(EventArgs.Empty);
            base.ClearContentCache();
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            if (!this.CheckField(NetworkField.HasShellItem))
            {
                this.FShellItem = this.CreateShellItem(owner);
                this.HasFields |= NetworkField.HasShellItem;
            }
            return ((this.FShellItem == null) ? null : ShellContextMenuHelper.CreateContextMenu(owner, this.FShellItem.GetUIObjectOf<IContextMenu>(owner.Handle), options, onExecuteVerb));
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            CustomFileSystemFolder createFile = destFolder as CustomFileSystemFolder;
            if (createFile == null)
            {
                throw new ArgumentException("destFolder is not CustomFileSystemFolder");
            }
            if (type != LinkType.Default)
            {
                throw new ArgumentException("Unsupported link type", "type");
            }
            switch (this.Resource.dwDisplayType)
            {
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER:
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE:
                    return FileSystemItem.CreateShellLink(destFolder, name, this.Resource.lpRemoteName);
            }
            return this.CreatePidlLink(createFile, name);
        }

        private IVirtualItem CreatePidlLink(CustomFileSystemFolder createFile, string name)
        {
            if (!this.CheckField(NetworkField.HasShellItem))
            {
                this.FShellItem = this.CreateShellItem(this.FOwner);
                this.HasFields |= NetworkField.HasShellItem;
            }
            if (this.FShellItem == null)
            {
                return null;
            }
            using (ShellLink link = new ShellLink())
            {
                link.IdList = this.FShellItem.AbsolutePidl;
                IChangeVirtualFile file = createFile.CreateFile(name);
                using (Stream stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.SequentialScan, 0L))
                {
                    link.Save(stream);
                }
                return file;
            }
        }

        private SafeShellItem CreateShellItem(IWin32Window owner)
        {
            SafeShellItem item2;
            IntPtr hwndOwner = (owner != null) ? owner.Handle : IntPtr.Zero;
            switch (this.Resource.dwDisplayType)
            {
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER:
                case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE:
                    return new SafeShellItem(hwndOwner, this.FullName);
            }
            if (!(!OS.IsWinVista || this.CheckField(NetworkField.IsRoot)))
            {
                return null;
            }
            IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
            try
            {
                SafeShellItem item;
                ITEMIDLIST itemidlist = new ITEMIDLIST();
                if (OS.IsWinVista)
                {
                    using (item = new SafeShellItem(desktopFolder, hwndOwner, Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK)))
                    {
                        itemidlist.Append(item.RelativePidl);
                    }
                }
                else
                {
                    using (item = new SafeShellItem(desktopFolder, hwndOwner, Microsoft.Shell.Shell32.GetClsidFolderParseName(CLSID.CLSID_NETWORK_NEIGHBORHOOD)))
                    {
                        itemidlist.Append(item.RelativePidl);
                        IShellFolder folder = item.BindToFolder();
                        try
                        {
                            IntPtr ptr2 = folder.ParseDisplayName(hwndOwner, "EntireNetwork");
                            try
                            {
                                itemidlist.Append(ptr2);
                                if (!this.CheckField(NetworkField.IsRoot))
                                {
                                    string lpRemoteName;
                                    RESOURCEDISPLAYTYPE dwDisplayType = this.Resource.dwDisplayType;
                                    if (dwDisplayType != RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN)
                                    {
                                        if (dwDisplayType != RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK)
                                        {
                                            throw new InvalidOperationException("Cannot create link for this network resource.");
                                        }
                                        lpRemoteName = this.Resource.lpRemoteName;
                                    }
                                    else
                                    {
                                        lpRemoteName = this.Resource.lpProvider;
                                    }
                                    IShellFolder folder3 = folder.BindToFolder(ptr2);
                                    try
                                    {
                                        IntPtr ptr3 = folder3.FindObject(hwndOwner, lpRemoteName, SHCONTF.SHCONTF_FOLDERS);
                                        if (ptr3 == IntPtr.Zero)
                                        {
                                            throw new DirectoryNotFoundException();
                                        }
                                        try
                                        {
                                            itemidlist.Append(ptr3);
                                            if (this.Resource.dwDisplayType == RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN)
                                            {
                                                IShellFolder folder4 = folder3.BindToFolder(ptr3);
                                                try
                                                {
                                                    IntPtr ptr4 = folder4.FindObject(hwndOwner, this.Resource.lpRemoteName, SHCONTF.SHCONTF_FOLDERS);
                                                    if (ptr4 == IntPtr.Zero)
                                                    {
                                                        throw new DirectoryNotFoundException();
                                                    }
                                                    try
                                                    {
                                                        itemidlist.Append(ptr4);
                                                    }
                                                    finally
                                                    {
                                                        Marshal.FreeCoTaskMem(ptr4);
                                                    }
                                                }
                                                finally
                                                {
                                                    Marshal.ReleaseComObject(folder4);
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            Marshal.FreeCoTaskMem(ptr3);
                                        }
                                    }
                                    finally
                                    {
                                        Marshal.ReleaseComObject(folder3);
                                    }
                                }
                            }
                            finally
                            {
                                Marshal.FreeCoTaskMem(ptr2);
                            }
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(folder);
                        }
                    }
                }
                IntPtr pidl = Marshal.AllocCoTaskMem(itemidlist.Size);
                try
                {
                    itemidlist.ToPidl(pidl);
                    item2 = new SafeShellItem(pidl);
                }
                catch
                {
                    Marshal.FreeCoTaskMem(pidl);
                    throw;
                }
            }
            finally
            {
                Marshal.ReleaseComObject(desktopFolder);
            }
            return item2;
        }

        public bool Equals(IVirtualItem other)
        {
            NetworkFolder folder = other as NetworkFolder;
            return ((((folder != null) && (this.Resource.dwDisplayType == folder.Resource.dwDisplayType)) && string.Equals(this.Resource.lpProvider, folder.Resource.lpProvider, StringComparison.OrdinalIgnoreCase)) && string.Equals(this.Resource.lpRemoteName, folder.Resource.lpRemoteName, StringComparison.OrdinalIgnoreCase));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            if (!this.CheckField(NetworkField.HasShellItem))
            {
                this.FShellItem = this.CreateShellItem(owner);
                this.HasFields |= NetworkField.HasShellItem;
            }
            if (this.FShellItem != null)
            {
                SHELLEXECUTEINFO shellexecuteinfo;
                shellexecuteinfo = new SHELLEXECUTEINFO {
                    cbSize = Marshal.SizeOf(shellexecuteinfo),
                    fMask = SEE_MASK.SEE_MASK_IDLIST,
                    lpIDList = this.FShellItem.AbsolutePidl,
                    lpVerb = verb,
                    hwnd = (owner != null) ? owner.Handle : IntPtr.Zero,
                    nShow = SW.SW_SHOW
                };
                return Microsoft.Shell.Shell32.ShellExecuteEx(ref shellexecuteinfo);
            }
            return false;
        }

        public override IEnumerable<IVirtualItem> GetCachedContent()
        {
            List<IVirtualItem> list = null;
            lock (NetworkCache)
            {
                IEnumerable<IVirtualItem> enumerable;
                if (NetworkCache.TryGetValue(this, out enumerable))
                {
                    list = new List<IVirtualItem>(enumerable);
                }
            }
            if (list != null)
            {
                return list;
            }
            return base.GetCachedContent();
        }

        public IEnumerable<IVirtualFolder> GetFolders()
        {
            return new <GetFolders>d__a(-2) { <>4__this = this };
        }

        public override int GetHashCode()
        {
            int num = (this.Resource.dwDisplayType != RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_GENERIC) ? ((int) this.Resource.dwDisplayType) : 1;
            if (this.Resource.lpProvider != null)
            {
                num *= this.Resource.lpProvider.ToLower().GetHashCode();
            }
            if (this.Resource.lpRemoteName != null)
            {
                num *= this.Resource.lpRemoteName.ToLower().GetHashCode();
            }
            return num;
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            Image image;
            DefaultIcon entireNetwork;
            bool flag = !ChangeVector.Equals(this.FStoredChangeVector, ChangeVector.Icon);
            if (flag)
            {
                ChangeVector.CopyTo(ref this.FStoredChangeVector, ChangeVector.Icon);
            }
            if (((this.FIcons != null) && !flag) && this.FIcons.TryGetValue(size, out image))
            {
                return image;
            }
            if (this.CheckField(NetworkField.IsRoot))
            {
                entireNetwork = DefaultIcon.EntireNetwork;
            }
            else
            {
                switch (this.Resource.dwDisplayType)
                {
                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN:
                        entireNetwork = DefaultIcon.NetworkWorkgroup;
                        goto Label_00B9;

                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER:
                        entireNetwork = DefaultIcon.NetworkServer;
                        goto Label_00B9;

                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE:
                        entireNetwork = DefaultIcon.NetworkFolder;
                        goto Label_00B9;

                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK:
                        entireNetwork = DefaultIcon.NetworkProvider;
                        goto Label_00B9;
                }
                entireNetwork = DefaultIcon.UnknownFile;
            }
        Label_00B9:
            image = ImageProvider.Default.GetDefaultIcon(entireNetwork, size);
            if ((this.FIcons == null) || flag)
            {
                this.FIcons = IconCollection.Create();
            }
            this.FIcons[size] = image;
            return image;
        }

        private IEnumerable<NETRESOURCE> GetNetworkContent(NETRESOURCE resource, bool root)
        {
            return new <GetNetworkContent>d__0(-2) { <>4__this = this, <>3__resource = resource, <>3__root = root };
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Resource", this.Resource);
        }

        public string GetPrefferedLinkName(LinkType type)
        {
            return ((type == LinkType.Default) ? Path.ChangeExtension(this.Name, ".lnk") : null);
        }

        public PropertyAvailability GetPropertyAvailability(int property)
        {
            return (this.AvailableProperties[property] ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        private void Initialize(NetworkField hasFields, IVirtualFolder parent)
        {
            this.HasFields = hasFields;
            this.FParent = parent;
            if (this.FParent != null)
            {
                this.HasFields |= NetworkField.HasParent;
            }
            this.FStoredChangeVector = ChangeVector.Value;
        }

        public bool IsChild(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item is FileSystemItem)
            {
                string fullName = item.FullName;
                if (this.Resource.dwDisplayType == RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER)
                {
                    return fullName.StartsWith(PathHelper.IncludeTrailingDirectorySeparator(this.FullName), StringComparison.OrdinalIgnoreCase);
                }
                return (this.CheckField(NetworkField.IsRoot) && fullName.StartsWith(PathHelper.UncPrefix, StringComparison.Ordinal));
            }
            return (this.CheckField(NetworkField.IsRoot) && (item is NetworkFolder));
        }

        protected override void PopulateContent()
        {
            base.RaiseProgressChanged(0, this);
            foreach (NETRESOURCE netresource in this.GetNetworkContent(this.Resource, this.CheckField(NetworkField.IsRoot)))
            {
                IVirtualItem item;
                if (base.CancellationPending)
                {
                    break;
                }
                if (this.CheckField(NetworkField.IsRoot) && (netresource.dwDisplayType == RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK))
                {
                    NETINFOSTRUCT netinfostruct;
                    netinfostruct = new NETINFOSTRUCT {
                        cbStructure = (uint) Marshal.SizeOf(netinfostruct)
                    };
                    WNet.CheckWNetError(Winnetwk.WNetGetNetworkInformation(netresource.lpProvider, ref netinfostruct));
                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(string.Format(@"Network\Type\{0}", netinfostruct.wNetType)))
                    {
                        if ((key != null) && Convert.ToBoolean(key.GetValue("HideProvider")))
                        {
                            continue;
                        }
                    }
                }
                Debug.WriteLine("NextResource", "NetworkFolder");
                Debug.WriteLine(string.Format("> dwScope = {0}", netresource.dwScope), "NetworkFolder");
                Debug.WriteLine(string.Format("> dwType = {0}", netresource.dwType), "NetworkFolder");
                Debug.WriteLine(string.Format("> dwDisplayType = {0}", netresource.dwDisplayType), "NetworkFolder");
                Debug.WriteLine(string.Format("> dwUsage = {0}", netresource.dwUsage), "NetworkFolder");
                Debug.WriteLine(string.Format("> lpLocalName = {0}", netresource.lpLocalName), "NetworkFolder");
                Debug.WriteLine(string.Format("> lpRemoteName = {0}", netresource.lpRemoteName), "NetworkFolder");
                Debug.WriteLine(string.Format("> lpProvider = {0}", netresource.lpProvider), "NetworkFolder");
                if (((netresource.dwType == RESOURCETYPE.RESOURCETYPE_DISK) && (netresource.dwDisplayType == RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE)) && ((netresource.dwUsage & RESOURCEUSAGE.RESOURCEUSAGE_CONNECTABLE) > ((RESOURCEUSAGE) 0)))
                {
                    item = new NetworkShareFolder(netresource, this);
                }
                else
                {
                    item = new NetworkFolder(netresource, this);
                }
                base.NewItem(item);
            }
            if (!base.CancellationPending && (this.Resource.dwDisplayType != RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER))
            {
                IList<IVirtualItem> content = base.Content;
                lock (content)
                {
                    lock (NetworkCache)
                    {
                        NetworkCache[this] = new List<IVirtualItem>(content);
                    }
                }
                base.RaiseCacheContentChanged(EventArgs.Empty);
            }
            this.HasFields &= ~NetworkField.HasToolTip;
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
            if (!this.CheckField(NetworkField.HasShellItem))
            {
                this.FShellItem = this.CreateShellItem(owner);
                this.HasFields |= NetworkField.HasShellItem;
            }
            if (this.FShellItem != null)
            {
                ShellContextMenuHelper.ExecuteVerb(owner, "properties", null, this.FShellItem.GetUIObjectOf<IContextMenu>(owner.Handle));
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.GetType().Name, this.FullName);
        }

        private int UpdateResource()
        {
            int num3;
            int cb = 0x4000;
            IntPtr lpBuffer = Marshal.AllocHGlobal(cb);
            try
            {
                string str;
                int num2 = Winnetwk.WNetGetResourceInformation(ref this.FResource, lpBuffer, ref cb, out str);
                if (num2 == 0)
                {
                    this.FResource = (NETRESOURCE) Marshal.PtrToStructure(lpBuffer, typeof(NETRESOURCE));
                    this.HasFields |= NetworkField.ResourceValid;
                    this.FAvailableSet = null;
                }
                num3 = num2;
            }
            finally
            {
                Marshal.FreeHGlobal(lpBuffer);
            }
            return num3;
        }

        public FileAttributes Attributes
        {
            get
            {
                return (FileAttributes.Directory | (this.CheckField(NetworkField.IsRoot) ? ((FileAttributes) 0) : FileAttributes.Offline));
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                if (this.FAvailableSet == null)
                {
                    int[] properties = new int[1];
                    this.FAvailableSet = new VirtualPropertySet(properties);
                    this.FAvailableSet[11] = !this.CheckField(NetworkField.ResourceValid) || !string.IsNullOrEmpty(this.FResource.lpComment);
                }
                return this.FAvailableSet;
            }
        }

        public override Nomad.FileSystem.Virtual.CacheState CacheState
        {
            get
            {
                Nomad.FileSystem.Virtual.CacheState unknown = Nomad.FileSystem.Virtual.CacheState.Unknown;
                lock (NetworkCache)
                {
                    IEnumerable<IVirtualItem> enumerable;
                    if (NetworkCache.TryGetValue(this, out enumerable))
                    {
                        unknown |= Nomad.FileSystem.Virtual.CacheState.HasContent;
                        foreach (IVirtualItem item in enumerable)
                        {
                            unknown |= Nomad.FileSystem.Virtual.CacheState.HasItems;
                            if (item != null)
                            {
                                return (unknown | Nomad.FileSystem.Virtual.CacheState.HasFolders);
                            }
                        }
                    }
                    return unknown;
                }
                return unknown;
            }
        }

        public bool Exists
        {
            get
            {
                return (this.CheckField(NetworkField.ResourceValid) || (this.UpdateResource() == 0));
            }
        }

        public string FullName
        {
            get
            {
                UriBuilder builder = null;
                if (this.CheckField(NetworkField.IsRoot))
                {
                    builder = new UriBuilder(NetworkFileSystemCreator.UriScheme, ".");
                }
                else
                {
                    switch (this.Resource.dwDisplayType)
                    {
                        case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_DOMAIN:
                            builder = new UriBuilder(NetworkFileSystemCreator.UriScheme, ".") {
                                Path = Path.Combine(this.Resource.lpProvider, this.Resource.lpRemoteName) + Path.DirectorySeparatorChar
                            };
                            break;

                        case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK:
                            builder = new UriBuilder(NetworkFileSystemCreator.UriScheme, ".") {
                                Path = (string.IsNullOrEmpty(this.Resource.lpRemoteName) ? this.Resource.lpProvider : this.Resource.lpRemoteName) + Path.DirectorySeparatorChar
                            };
                            break;
                    }
                }
                if (builder != null)
                {
                    return builder.Uri.ToString();
                }
                return this.Resource.lpRemoteName;
            }
        }

        public VirtualHighligher Highlighter
        {
            get
            {
                return null;
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

                    case 11:
                        return this.Resource.lpComment;
                }
                return null;
            }
        }

        public override string Name
        {
            get
            {
                switch (this.Resource.dwDisplayType)
                {
                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SERVER:
                        if (!this.Resource.lpRemoteName.StartsWith(PathHelper.UncPrefix))
                        {
                            break;
                        }
                        return this.Resource.lpRemoteName.Substring(2);

                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE:
                    {
                        int num = this.Resource.lpRemoteName.LastIndexOf(Path.DirectorySeparatorChar);
                        return this.Resource.lpRemoteName.Substring(num + 1);
                    }
                    case RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_NETWORK:
                        if (!string.IsNullOrEmpty(this.Resource.lpRemoteName))
                        {
                            break;
                        }
                        return this.Resource.lpProvider;
                }
                return (this.CheckField(NetworkField.IsRoot) ? Resources.sNetworkLong : this.Resource.lpRemoteName);
            }
        }

        public IWin32Window Owner
        {
            get
            {
                return this.FOwner;
            }
            set
            {
                this.FOwner = value;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                if (!this.CheckField(NetworkField.HasParent))
                {
                    try
                    {
                        Debug.WriteLine("GetParent - Start", "NetworkFolder");
                        int cb = 0x4000;
                        IntPtr lpBuffer = Marshal.AllocHGlobal(cb);
                        try
                        {
                            WNet.CheckWNetError(Winnetwk.WNetGetResourceParent(ref this.Resource, lpBuffer, ref cb));
                            NETRESOURCE resource = (NETRESOURCE) Marshal.PtrToStructure(lpBuffer, typeof(NETRESOURCE));
                            Debug.WriteLine("GetParent - Resource Acquired", "NetworkFolder");
                            if (resource.lpRemoteName == null)
                            {
                                foreach (NETRESOURCE netresource3 in this.GetNetworkContent(resource, true))
                                {
                                    if (netresource3.lpRemoteName == resource.lpProvider)
                                    {
                                        this.FParent = new NetworkFolder(netresource3, NetworkFileSystemCreator.NetworkRoot);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                this.FParent = new NetworkFolder(resource, null);
                            }
                            Debug.WriteLine("GetParent - Finish", "NetworkFolder");
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(lpBuffer);
                        }
                    }
                    finally
                    {
                        if (this.FParent == null)
                        {
                            this.FParent = NetworkFileSystemCreator.NetworkRoot;
                        }
                        this.HasFields |= NetworkField.HasParent;
                    }
                }
                return this.FParent;
            }
            set
            {
                this.FParent = value;
                this.HasFields |= NetworkField.HasParent;
            }
        }

        protected NETRESOURCE Resource
        {
            get
            {
                if (!this.CheckField(NetworkField.ResourceValid))
                {
                    WNet.CheckWNetError(this.UpdateResource());
                }
                return this.FResource;
            }
        }

        public IVirtualFolder Root
        {
            get
            {
                if (this.CheckField(NetworkField.IsRoot))
                {
                    return this;
                }
                return NetworkFileSystemCreator.NetworkRoot;
            }
        }

        public string ShortName
        {
            get
            {
                if (this.CheckField(NetworkField.IsRoot))
                {
                    return Resources.sNetworkShort;
                }
                if (string.IsNullOrEmpty(this.Resource.lpLocalName))
                {
                    return this.Name;
                }
                return this.Resource.lpLocalName;
            }
        }

        public string ToolTip
        {
            get
            {
                if (!this.CheckField(NetworkField.HasToolTip))
                {
                    this.FToolTip = VirtualItemVisualExtender.GetItemTooltip(this);
                    this.HasFields |= NetworkField.HasToolTip;
                }
                return this.FToolTip;
            }
        }

        [CompilerGenerated]
        private sealed class <GetFolders>d__a : IEnumerable<IVirtualFolder>, IEnumerable, IEnumerator<IVirtualFolder>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IVirtualFolder <>2__current;
            public NetworkFolder <>4__this;
            public Dictionary<NetworkFolder, IEnumerable<IVirtualItem>> <>7__wrape;
            public IEnumerator<IVirtualFolder> <>7__wrapf;
            private int <>l__initialThreadId;
            public IEnumerable<IVirtualItem> <CachedContent>5__b;
            public IVirtualFolder <Item>5__d;
            public bool <Retrieved>5__c;

            [DebuggerHidden]
            public <GetFolders>d__a(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally10()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapf != null)
                {
                    this.<>7__wrapf.Dispose();
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
                        if (num != 3)
                        {
                            goto Label_00D8;
                        }
                        goto Label_00BA;
                    }
                    this.<>1__state = -1;
                    lock ((this.<>7__wrape = NetworkFolder.NetworkCache))
                    {
                        this.<Retrieved>5__c = NetworkFolder.NetworkCache.TryGetValue(this.<>4__this, out this.<CachedContent>5__b);
                    }
                    if (this.<Retrieved>5__c)
                    {
                        this.<>7__wrapf = this.<CachedContent>5__b.OfType<IVirtualFolder>().GetEnumerator();
                        this.<>1__state = 2;
                        while (this.<>7__wrapf.MoveNext())
                        {
                            this.<Item>5__d = this.<>7__wrapf.Current;
                            this.<>2__current = this.<Item>5__d;
                            this.<>1__state = 3;
                            return true;
                        Label_00BA:
                            this.<>1__state = 2;
                        }
                        this.<>m__Finally10();
                    }
                Label_00D8:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<IVirtualFolder> IEnumerable<IVirtualFolder>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new NetworkFolder.<GetFolders>d__a(0) { <>4__this = this.<>4__this };
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
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally10();
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

        [CompilerGenerated]
        private sealed class <GetNetworkContent>d__0 : IEnumerable<NETRESOURCE>, IEnumerable, IEnumerator<NETRESOURCE>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private NETRESOURCE <>2__current;
            public NETRESOURCE <>3__resource;
            public bool <>3__root;
            public NetworkFolder <>4__this;
            private int <>l__initialThreadId;
            public IntPtr <Buffer>5__5;
            public int <BufferSize>5__4;
            public uint <Count>5__3;
            public int <ErrorCode>5__2;
            public SafeNetEnumHandle <Handle>5__1;
            public NETRESOURCE resource;
            public bool root;

            [DebuggerHidden]
            public <GetNetworkContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally6()
            {
                this.<>1__state = -1;
                this.<Handle>5__1.Close();
            }

            private void <>m__Finally7()
            {
                this.<>1__state = 1;
                Marshal.FreeHGlobal(this.<Buffer>5__5);
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
                        goto Label_00E8;
                    }
                    if (num != 3)
                    {
                        goto Label_01D5;
                    }
                    goto Label_0168;
                Label_002A:
                    if (this.root)
                    {
                        this.<ErrorCode>5__2 = Winnetwk.WNetOpenEnum(RESOURCE.RESOURCE_GLOBALNET, RESOURCETYPE.RESOURCETYPE_DISK, 0, IntPtr.Zero, out this.<Handle>5__1);
                    }
                    else
                    {
                        this.<ErrorCode>5__2 = Winnetwk.WNetOpenEnum(RESOURCE.RESOURCE_GLOBALNET, RESOURCETYPE.RESOURCETYPE_DISK, 0, ref this.resource, out this.<Handle>5__1);
                    }
                    if (((this.<ErrorCode>5__2 != 5) || (this.<>4__this.FOwner == null)) || ((this.resource.dwType != RESOURCETYPE.RESOURCETYPE_DISK) && (this.resource.dwType != RESOURCETYPE.RESOURCETYPE_ANY)))
                    {
                        goto Label_00EF;
                    }
                    this.<ErrorCode>5__2 = NetworkFileSystemCreator.AddConnection(this.<>4__this.FOwner, this.resource);
                    if (this.<ErrorCode>5__2 == 0x4c7)
                    {
                        goto Label_01D5;
                    }
                Label_00E8:
                    flag2 = true;
                    goto Label_002A;
                Label_00EF:
                    if (this.<ErrorCode>5__2 != 0)
                    {
                        WNet.CheckWNetError(this.<ErrorCode>5__2);
                    }
                    this.<>1__state = 1;
                    this.<Count>5__3 = 1;
                    this.<BufferSize>5__4 = 0x4000;
                    this.<Buffer>5__5 = Marshal.AllocHGlobal(this.<BufferSize>5__4);
                    this.<>1__state = 2;
                    while (((this.<ErrorCode>5__2 = Winnetwk.WNetEnumResource(this.<Handle>5__1, ref this.<Count>5__3, this.<Buffer>5__5, ref this.<BufferSize>5__4)) == 0) && (this.<Count>5__3 == 1))
                    {
                        this.<>2__current = (NETRESOURCE) Marshal.PtrToStructure(this.<Buffer>5__5, typeof(NETRESOURCE));
                        this.<>1__state = 3;
                        return true;
                    Label_0168:
                        this.<>1__state = 2;
                    }
                    this.<>m__Finally7();
                    if (this.<ErrorCode>5__2 != 0x103)
                    {
                        WNet.CheckWNetError(this.<ErrorCode>5__2);
                    }
                    this.<>m__Finally6();
                Label_01D5:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<NETRESOURCE> IEnumerable<NETRESOURCE>.GetEnumerator()
            {
                NetworkFolder.<GetNetworkContent>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new NetworkFolder.<GetNetworkContent>d__0(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.resource = this.<>3__resource;
                d__.root = this.<>3__root;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Microsoft.Win32.Network.NETRESOURCE>.GetEnumerator();
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
                            switch (this.<>1__state)
                            {
                                case 2:
                                case 3:
                                    break;

                                default:
                                    break;
                            }
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally7();
                            }
                        }
                        finally
                        {
                            this.<>m__Finally6();
                        }
                        break;
                }
            }

            NETRESOURCE IEnumerator<NETRESOURCE>.Current
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

        [Flags]
        protected enum NetworkField
        {
            HasParent = 4,
            HasShellItem = 8,
            HasToolTip = 0x10,
            IsRoot = 2,
            ResourceValid = 1
        }
    }
}

