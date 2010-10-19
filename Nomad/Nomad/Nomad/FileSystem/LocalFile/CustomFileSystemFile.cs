namespace Nomad.FileSystem.LocalFile
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Microsoft.Win32.Security;
    using Nomad;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Windows.Forms;

    public abstract class CustomFileSystemFile : FileSystemItem, IUpdateVirtualProperty, ISetVirtualProperty, IVirtualFileExecute, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, IElevatable
    {
        private string _Extension;
        private System.IO.FileInfo _FileInfo;
        private FileTimeSetter _FileTimeSetter;

        protected CustomFileSystemFile(System.IO.FileInfo info, IVirtualFolder parent) : base((info != null) ? info.FullName : null, parent)
        {
            this._FileInfo = info;
        }

        protected CustomFileSystemFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected CustomFileSystemFile(string fileName, IVirtualFolder parent) : base(fileName, parent)
        {
        }

        public void BeginUpdate()
        {
            if (!((this._FileTimeSetter != null) || base.CheckCapability(FileSystemItem.ItemCapability.IsElevated)))
            {
                this._FileTimeSetter = new FileTimeSetter(base.FullName, false);
                GC.SuppressFinalize(this._FileTimeSetter);
            }
        }

        protected override bool CacheProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 4:
                case 5:
                case 15:
                case 0x10:
                case 20:
                    return true;
            }
            return base.CacheProperty(propertyId);
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if ((folder != null) && folder.Exists)
            {
                LinkType type = LinkType.Default;
                if (OS.IsWin2k)
                {
                    VolumeInfo folderVolume = folder.FolderVolume;
                    if ((folderVolume == null) || !string.Equals(folderVolume.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase))
                    {
                        return type;
                    }
                    VolumeInfo itemVolume = base.ItemVolume;
                    if ((itemVolume != null) && string.Equals(itemVolume.Name, folderVolume.Name, FileSystemItem.ComparisonRule))
                    {
                        type |= LinkType.HardLink;
                    }
                    if (OS.IsWinVista)
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
                case 0:
                    return true;

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

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[1] = true;
            set[3] = true;
            set[4] = true;
            if (this.Compressed)
            {
                set[5] = true;
                set[20] = true;
            }
            if (OS.IsWinNT)
            {
                VolumeInfo itemVolume = base.ItemVolume;
                if (itemVolume == null)
                {
                    return set;
                }
                set[0x10] = string.Equals(itemVolume.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase);
                if ((itemVolume.Capabilities & VolumeCapabilities.FilePersistentAcls) > ((VolumeCapabilities) 0))
                {
                    set[14] = true;
                    set[15] = true;
                }
            }
            return set;
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            IVirtualItem item;
            CustomFileSystemFolder folder = destFolder as CustomFileSystemFolder;
            if (folder == null)
            {
                throw new ArgumentException("destFolder is not CustomFileSystemFolder");
            }
            if ((type == LinkType.Default) && base.CheckCapability(FileSystemItem.ItemCapability.IsElevated))
            {
                folder.ResetInfo();
                folder.Proxy = base.Proxy;
                folder.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
            }
            string fileName = Path.Combine(folder.FullName, name);
            LinkType type2 = type;
            if (type2 != LinkType.Default)
            {
                if (type2 != LinkType.HardLink)
                {
                    if (type2 != LinkType.SymbolicLink)
                    {
                        throw new ArgumentException("Unsupported link type", "type");
                    }
                    ((IFileProxy) base.Proxy).CreateSymbolicLink(fileName, base.FullName, false);
                    item = new FileSystemFile(fileName);
                    goto Label_00E3;
                }
            }
            else
            {
                return FileSystemItem.CreateShellLink(folder, name, base.FullName);
            }
            ((IFileProxy) base.Proxy).CreateHardLink(fileName, base.FullName);
            item = new FileSystemFile(fileName);
        Label_00E3:
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, fileName);
            return item;
        }

        public override void Delete(bool sendToBin)
        {
            try
            {
                base.Delete(sendToBin);
            }
            catch (Exception exception)
            {
                this.ProcessFileInUseException(exception, Resources.sErrorDeletingFileInUse, Resources.sErrorDeletingFileInUseBy);
                throw;
            }
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
            IFileProxy proxy = activator.Create<IFileProxy>("filesystemproxy");
            try
            {
                base.Proxy = proxy;
                this._FileInfo = base.CreateInfo<System.IO.FileInfo>(base.FullName);
                base.SetCapability(FileSystemItem.ItemCapability.IsElevated, true);
                flag = true;
            }
            catch (RemotingException)
            {
            }
            return flag;
        }

        public void EndUpdate()
        {
            if ((this._FileTimeSetter != null) && this._FileTimeSetter.Set())
            {
                base.SetCapability(FileSystemItem.ItemCapability.ItemRefreshNeeded, true);
                LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
            }
            this._FileTimeSetter = null;
        }

        public virtual Process Execute(IWin32Window owner)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(base.FullName) {
                WorkingDirectory = Path.GetDirectoryName(base.FullName)
            };
            return LocalFileSystemCreator.Execute(owner, startInfo);
        }

        public Process ExecuteEx(IWin32Window owner, string arguments, ExecuteAsUser runAs, string userName, SecureString password)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(base.FullName) {
                WorkingDirectory = Path.GetDirectoryName(base.FullName)
            };
            if (!string.IsNullOrEmpty(arguments))
            {
                startInfo.Arguments = arguments;
            }
            switch (runAs)
            {
                case ExecuteAsUser.Administrator:
                    startInfo.Verb = "runas";
                    break;

                case ExecuteAsUser.SpecifiedUser:
                    if (!string.IsNullOrEmpty(userName))
                    {
                        startInfo.UserName = userName;
                        startInfo.Password = password;
                        startInfo.UseShellExecute = false;
                    }
                    break;
            }
            return LocalFileSystemCreator.Execute(owner, startInfo);
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            if (defaultIcon || base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
            {
                return ImageProvider.Default.GetDefaultFileIcon(PathHelper.NormalizeInvalidPath(base.FullName), size);
            }
            return ImageProvider.Default.GetFileIcon(base.FullName, size);
        }

        public string GetPrefferedLinkName(LinkType type)
        {
            switch (type)
            {
                case LinkType.Default:
                    return Path.ChangeExtension(this.Name, ".lnk");

                case LinkType.HardLink:
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
                    return this.Extension;

                case 3:
                    return this.FileInfo.Length;

                case 4:
                {
                    long allocationSize = this.AllocationSize;
                    if (allocationSize < 0L)
                    {
                        return null;
                    }
                    return allocationSize;
                }
                case 5:
                    return Microsoft.IO.File.GetCompressedFileSize(base.FullName);

                case 14:
                    return this.FileInfo.GetAccessControl(AccessControlSections.All);

                case 15:
                    return this.Owner;

                case 0x10:
                    return this.NumberOfHardLinks;

                case 20:
                {
                    CompressionFormat compressedState = Microsoft.IO.File.GetCompressedState(base.FullName);
                    switch (compressedState)
                    {
                        case CompressionFormat.None:
                            return null;

                        case CompressionFormat.Default:
                        case CompressionFormat.LZNT1:
                            return compressedState;
                    }
                    return "Unknown";
                }
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 1:
                case 3:
                case 4:
                    return PropertyAvailability.Normal;

                case 5:
                case 20:
                    return (this.Compressed ? PropertyAvailability.Normal : PropertyAvailability.None);
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public IVirtualItem MoveTo(IVirtualFolder dest)
        {
            return base.MoveTo(dest, delegate (string destFileName) {
                this.FileInfo.MoveTo(destFileName);
            });
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            Stream stream2;
            if ((mode != FileMode.Open) && (startOffset > 0L))
            {
                throw new ArgumentException("startOffset have sense only in open file mode");
            }
            try
            {
                Stream stream;
                WatcherChangeTypes changeType = 0;
                switch (mode)
                {
                    case FileMode.CreateNew:
                        changeType = WatcherChangeTypes.Created;
                        break;

                    case FileMode.Create:
                        changeType = this.Info.Exists ? WatcherChangeTypes.Changed : WatcherChangeTypes.Created;
                        break;

                    case FileMode.OpenOrCreate:
                        if (!this.Info.Exists)
                        {
                            changeType = WatcherChangeTypes.Created;
                        }
                        break;

                    case FileMode.Truncate:
                        changeType = WatcherChangeTypes.Changed;
                        break;
                }
                if (!((options == FileOptions.None) || base.CheckCapability(FileSystemItem.ItemCapability.IsElevated)))
                {
                    stream = new FileStream(base.FullName, mode, access, share, 0x8000, options);
                }
                else
                {
                    stream = this.FileInfo.Open(mode, access, share);
                    if (RemotingServices.IsTransparentProxy(stream))
                    {
                        LocalFileSystemCreator.Sponsor.Register(stream);
                    }
                }
                if (startOffset > 0L)
                {
                    stream.Seek(startOffset, SeekOrigin.Begin);
                }
                if (changeType != 0)
                {
                    LocalFileSystemCreator.RaiseFileChangedEvent(changeType, this.ComparableName);
                }
                stream2 = stream;
            }
            catch (Exception exception)
            {
                exception.Data["FullName"] = base.FullName;
                this.ProcessFileInUseException(exception, Resources.sErrorAccessFileInUse, Resources.sErrorAccessFileInUseBy);
                throw;
            }
            return stream2;
        }

        private void ProcessFileInUseException(Exception e, string str1, string str2)
        {
            if (FileInUseException.IsFileInUseError(e))
            {
                string appNameUsingFile = FileInUseException.GetAppNameUsingFile(base.FullName);
                if (string.IsNullOrEmpty(appNameUsingFile))
                {
                    throw new FileInUseException(string.Format(str1, base.FullName), base.FullName, (e is FileInUseException) ? e.InnerException : e);
                }
                throw new FileInUseException(string.Format(str2, base.FullName, appNameUsingFile), base.FullName, appNameUsingFile, (e is FileInUseException) ? e.InnerException : e);
            }
        }

        protected internal override void ResetInfo()
        {
            if ((this._FileInfo != null) && RemotingServices.IsTransparentProxy(this._FileInfo))
            {
                LocalFileSystemCreator.Sponsor.Unregister(this._FileInfo);
            }
            this._FileInfo = null;
            base.ResetInfo();
        }

        protected internal override void ResetVisualCache()
        {
            base.SetCapability(FileSystemItem.ItemCapability.HasExtension, false);
            base.ResetVisualCache();
        }

        protected void SetAccessControl(FileSecurity security)
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
            FileSecurity fileSecurity = new FileSecurity();
            fileSecurity.SetSecurityDescriptorBinaryForm(security.GetSecurityDescriptorBinaryForm(), all);
            this.FileInfo.SetAccessControl(fileSecurity);
        }

        protected void SetName(string newName)
        {
            base.SetName(newName, delegate (string destFileName) {
                this.FileInfo.MoveTo(destFileName);
            });
        }

        public override void SetProperty(int propertyId, object value)
        {
            switch (propertyId)
            {
                case 7:
                    if (this._FileTimeSetter == null)
                    {
                        ((IFileProxy) base.Proxy).SetCreationTime(base.FullName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FileTimeSetter.CreationTime = (DateTime) value;
                    break;

                case 8:
                    if (this._FileTimeSetter == null)
                    {
                        ((IFileProxy) base.Proxy).SetLastWriteTime(base.FullName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FileTimeSetter.LastWriteTime = (DateTime) value;
                    break;

                case 9:
                    if (this._FileTimeSetter == null)
                    {
                        ((IFileProxy) base.Proxy).SetLastAccessTime(base.FullName, (DateTime) value);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.ComparableName);
                        break;
                    }
                    this._FileTimeSetter.LastAccessTime = (DateTime) value;
                    break;

                case 14:
                    this.SetAccessControl((FileSecurity) value);
                    break;

                case 0:
                    this.SetName((string) value);
                    break;

                default:
                    base.SetProperty(propertyId, value);
                    break;
            }
        }

        protected long AllocationSize
        {
            get
            {
                if (OS.IsWinVista)
                {
                    using (SafeFileHandle handle = Microsoft.IO.File.OpenReadAttributes(base.FullName))
                    {
                        FILE_STANDARD_INFO file_standard_info;
                        if (!Windows.GetFileInformationByHandleEx(handle, out file_standard_info))
                        {
                            throw new Win32IOException();
                        }
                        return file_standard_info.AllocationSize;
                    }
                }
                VolumeInfo itemVolume = base.ItemVolume;
                if (itemVolume != null)
                {
                    uint clusterSize = itemVolume.ClusterSize;
                    long num2 = this.Compressed ? Microsoft.IO.File.GetCompressedFileSize(base.FullName) : this.FileInfo.Length;
                    return (long) (((num2 / ((ulong) clusterSize)) + Math.Sign((long) (num2 % ((ulong) clusterSize)))) * clusterSize);
                }
                return -1L;
            }
        }

        public override FileAttributes Attributes
        {
            get
            {
                if (base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                {
                    return FileAttributes.Normal;
                }
                return base.Attributes;
            }
        }

        public bool CanExecuteEx
        {
            get
            {
                return (!base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted) && PathHelper.IsExecutableFile(this.Name));
            }
        }

        public bool CanSeek
        {
            get
            {
                return true;
            }
        }

        protected override bool Compressed
        {
            set
            {
                if (value != this.Compressed)
                {
                    try
                    {
                        ((IFileProxy) base.Proxy).SetCompressedState(base.FullName, value);
                        base.ResetAvailableSet();
                    }
                    catch (Exception exception)
                    {
                        this.ProcessFileInUseException(exception, Resources.sErrorAccessFileInUse, Resources.sErrorAccessFileInUseBy);
                        throw;
                    }
                }
            }
        }

        protected override bool Encrypted
        {
            set
            {
                if (value != this.Encrypted)
                {
                    try
                    {
                        if (value)
                        {
                            this.FileInfo.Encrypt();
                        }
                        else
                        {
                            this.FileInfo.Decrypt();
                        }
                    }
                    catch (IOException exception)
                    {
                        this.ProcessFileInUseException(exception, Resources.sErrorAccessFileInUse, Resources.sErrorAccessFileInUseBy);
                        throw;
                    }
                }
            }
        }

        public string Extension
        {
            get
            {
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasExtension))
                {
                    string name = this.Name;
                    int startIndex = name.LastIndexOf('.');
                    this._Extension = ((startIndex > 0) && (startIndex < (name.Length - 1))) ? name.Substring(startIndex) : null;
                    base.SetCapability(FileSystemItem.ItemCapability.HasExtension, true);
                }
                return this._Extension;
            }
        }

        protected System.IO.FileInfo FileInfo
        {
            get
            {
                if (this._FileInfo == null)
                {
                    this._FileInfo = base.CreateInfo<System.IO.FileInfo>(base.FullName);
                }
                return this._FileInfo;
            }
        }

        protected internal override FileSystemInfo Info
        {
            get
            {
                return this.FileInfo;
            }
        }

        protected int NumberOfHardLinks
        {
            get
            {
                using (SafeFileHandle handle = Microsoft.IO.File.OpenReadAttributes(base.FullName))
                {
                    BY_HANDLE_FILE_INFORMATION by_handle_file_information;
                    if (!Windows.GetFileInformationByHandle(handle, out by_handle_file_information))
                    {
                        throw new Win32IOException();
                    }
                    return (int) by_handle_file_information.nNumberOfLinks;
                }
            }
        }

        protected string Owner
        {
            get
            {
                try
                {
                    return this.FileInfo.GetAccessControl(AccessControlSections.Owner).GetOwner(typeof(NTAccount)).ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}

