namespace Nomad.FileSystem.LocalFile
{
    using Microsoft;
    using Microsoft.IO;
    using Nomad;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading;

    [Serializable]
    public class FileSystemDrive : EjectableFileSystemFolder, ISetVirtualProperty, IPersistVirtualItem, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICreateVirtualFile, ICreateVirtualFolder, ICloneable
    {
        private DriveReady CachedDriveReady;
        private int CachedTickCount;
        private static Dictionary<string, WeakReference> DriveMap = new Dictionary<string, WeakReference>(StringComparer.OrdinalIgnoreCase);
        private string DriveName;
        private const string EntryDriveInfo = "DriveInfo";
        private DriveInfo FDriveInfo;
        private WeakReference FThumbnail;
        private int FThumbnailWidth;
        private static Func<DriveInfo, DriveChars, VolumeInfo, DriveReady> GetDriveReadyHandler = new Func<DriveInfo, DriveChars, VolumeInfo, DriveReady>(FileSystemDrive.GetDriveReady);

        private FileSystemDrive(DriveInfo info) : base(info.RootDirectory, null)
        {
            this.FThumbnailWidth = -1;
            this.FDriveInfo = info;
            this.DriveName = this.FDriveInfo.Name.Substring(0, 2).ToUpper();
            base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, true);
            this.Initialize();
        }

        private FileSystemDrive(string driveName) : this(new DriveInfo(driveName))
        {
        }

        protected FileSystemDrive(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.FThumbnailWidth = -1;
            this.FDriveInfo = (DriveInfo) info.GetValue("DriveInfo", typeof(DriveInfo));
            this.DriveName = this.FDriveInfo.Name.Substring(0, 2).ToUpper();
            base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, true);
            this.Initialize();
        }

        protected override bool CacheProperty(int propertyId)
        {
            return ((propertyId == 10) || base.CacheProperty(propertyId));
        }

        public override bool CanSetProperty(int propertyId)
        {
            if (propertyId == 30)
            {
                VolumeInfo folderVolume = base.FolderVolume;
                return ((folderVolume == null) || !folderVolume.IsReadOnly);
            }
            return false;
        }

        public object Clone()
        {
            return this.InternalClone();
        }

        public static FileSystemDrive Create(string driveName)
        {
            FileSystemDrive target;
            WeakReference reference;
            if (DriveMap.TryGetValue(driveName, out reference) && reference.IsAlive)
            {
                target = (FileSystemDrive) reference.Target;
                if (!target.CheckCapability(FileSystemItem.ItemCapability.Deleted))
                {
                    return target;
                }
            }
            target = new FileSystemDrive(driveName);
            DriveMap[driveName] = new WeakReference(target);
            return target;
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[0x1b] = true;
            set[0x1a] = true;
            set[0x1c] = true;
            set[0x1d] = true;
            set[30] = true;
            set[0x15] = base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail);
            if (!base.IsPropertyCached(10))
            {
                set[10] = true;
            }
            return set;
        }

        private static DriveReady GetDriveReady(DriveInfo drive, DriveChars driveChar, VolumeInfo volume)
        {
            if (volume != null)
            {
                DriveReady ready;
                try
                {
                    switch (volume.VolumeType)
                    {
                        case VolumeType.Removable:
                            if ((driveChar & (DriveChars.B | DriveChars.A)) <= DriveChars.None)
                            {
                                break;
                            }
                            return DriveReady.TimeOut;

                        case VolumeType.Fixed:
                        case VolumeType.CDRom:
                        case VolumeType.Ram:
                            return (volume.IsReady ? DriveReady.Ready : DriveReady.NotReady);

                        case VolumeType.Network:
                        case VolumeType.DVDRom:
                            goto Label_0081;

                        case VolumeType.Floppy3:
                        case VolumeType.Floppy5:
                            return DriveReady.TimeOut;

                        default:
                            goto Label_0081;
                    }
                    ready = volume.IsReady ? DriveReady.Ready : DriveReady.NotReady;
                }
                catch (InvalidOperationException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
                return ready;
            }
        Label_0081:
            if ((driveChar & (DriveChars.B | DriveChars.A)) > DriveChars.None)
            {
                return DriveReady.TimeOut;
            }
            return (drive.IsReady ? DriveReady.Ready : DriveReady.NotReady);
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            if (defaultIcon)
            {
                return ImageProvider.Default.GetDefaultIcon(DefaultIcon.Drive, size);
            }
            return ImageProvider.Default.GetDriveIcon(this.FDriveInfo, size);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DriveInfo", this.FDriveInfo);
        }

        public override string GetPrefferedLinkName(LinkType type)
        {
            if (type == LinkType.Default)
            {
                string volumeLabel = this.VolumeLabel;
                if (string.IsNullOrEmpty(volumeLabel))
                {
                    volumeLabel = this.DriveName.Substring(0, 1);
                }
                return (volumeLabel + ".lnk");
            }
            return null;
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 0x1a:
                case 0x1b:
                case 0x1c:
                case 30:
                    if (this.IsDriveReady != DriveReady.Ready)
                    {
                        throw new DeviceNotReadyException();
                    }
                    break;
            }
            switch (propertyId)
            {
                case 0x15:
                    return (base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail) ? this.Thumbnail : null);

                case 0x1a:
                    return this.FDriveInfo.TotalSize;

                case 0x1b:
                    return this.FDriveInfo.TotalFreeSpace;

                case 0x1c:
                    return this.FDriveInfo.DriveFormat;

                case 0x1d:
                {
                    VolumeInfo folderVolume = base.FolderVolume;
                    if (folderVolume == null)
                    {
                        return null;
                    }
                    return folderVolume.VolumeType;
                }
                case 30:
                    return this.VolumeLabel;

                case 10:
                    return this.VolumeTarget;
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 0x1a:
                case 0x1b:
                case 0x1c:
                case 0x1d:
                case 30:
                    return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        protected override VolumeInfo GetVolume()
        {
            return VolumeCache.Get(this.FDriveInfo.Name);
        }

        private void Initialize()
        {
            base.SetCapability(FileSystemItem.ItemCapability.HasLastAccessTime | FileSystemItem.ItemCapability.HasLastWriteTime | FileSystemItem.ItemCapability.HasCreationTime, false);
        }

        protected override object InternalClone()
        {
            FileSystemDrive drive = (FileSystemDrive) base.InternalClone();
            drive.FDriveInfo = this.FDriveInfo;
            drive.DriveName = this.DriveName;
            drive.Initialize();
            drive.FThumbnail = this.FThumbnail;
            drive.FThumbnailWidth = this.FThumbnailWidth;
            drive.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, base.CheckCapability(FileSystemItem.ItemCapability.HasThumbnail));
            return drive;
        }

        public override void SetProperty(int propertyId, object value)
        {
            if (propertyId == 30)
            {
                this.VolumeLabel = (string) value;
            }
            this.ResetVisualCache();
        }

        public override FileAttributes Attributes
        {
            get
            {
                FileAttributes attributes;
                switch (this.IsDriveReady)
                {
                    case DriveReady.NotReady:
                        return (FileAttributes.Offline | FileAttributes.Directory | FileAttributes.Hidden);

                    case DriveReady.TimeOut:
                        return (FileAttributes.Directory | FileAttributes.Hidden);
                }
                try
                {
                    attributes = base.RefreshedInfo.Attributes;
                    if (this.FDriveInfo.DriveType == DriveType.Fixed)
                    {
                        attributes &= ~(FileAttributes.System | FileAttributes.Hidden);
                    }
                    else if (attributes < 0)
                    {
                        attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                }
                catch (SystemException exception)
                {
                    if (!(exception is IOException) && !(exception is UnauthorizedAccessException))
                    {
                        throw;
                    }
                    attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
                return attributes;
            }
        }

        public override string ComparableName
        {
            get
            {
                return (this.DriveName + Path.DirectorySeparatorChar);
            }
        }

        public override bool Exists
        {
            get
            {
                return (this.IsDriveReady == DriveReady.Ready);
            }
        }

        private DriveReady IsDriveReady
        {
            get
            {
                DriveReady timeOut;
                WaitOrTimerCallback callBack = null;
                if (base.CheckAnyCapability(FileSystemItem.ItemCapability.Unreadable | FileSystemItem.ItemCapability.Deleted))
                {
                    return DriveReady.NotReady;
                }
                if (Math.Abs((int) (Environment.TickCount - this.CachedTickCount)) < 0x5dc)
                {
                    return this.CachedDriveReady;
                }
                DriveChars chars = (DriveChars) Enum.Parse(typeof(DriveChars), this.DriveName.Substring(0, 1), true);
                IAsyncResult AsyncReadyResult = GetDriveReadyHandler.BeginInvoke(this.FDriveInfo, chars, base.FolderVolume, null, null);
                if (AsyncReadyResult.AsyncWaitHandle.WaitOne(500, false))
                {
                    timeOut = GetDriveReadyHandler.EndInvoke(AsyncReadyResult);
                }
                else
                {
                    timeOut = DriveReady.TimeOut;
                    if (callBack == null)
                    {
                        callBack = delegate (object d1, bool d2) {
                            GetDriveReadyHandler.EndInvoke(AsyncReadyResult);
                        };
                    }
                    ThreadPool.RegisterWaitForSingleObject(AsyncReadyResult.AsyncWaitHandle, callBack, null, -1, true);
                }
                this.CachedDriveReady = timeOut;
                this.CachedTickCount = Environment.TickCount;
                return timeOut;
            }
        }

        public override bool IsSlowIcon
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get
            {
                string volumeLabel = null;
                if (this.FDriveInfo.DriveType == DriveType.Network)
                {
                    volumeLabel = this[10] as string;
                }
                if (string.IsNullOrEmpty(volumeLabel))
                {
                    volumeLabel = this.VolumeLabel;
                }
                if (string.IsNullOrEmpty(volumeLabel))
                {
                    return this.DriveName;
                }
                return string.Concat(new object[] { volumeLabel, " (", this.DriveName, ')' });
            }
        }

        public override IVirtualFolder Root
        {
            get
            {
                return this;
            }
        }

        public override string ShortName
        {
            get
            {
                return this.DriveName;
            }
        }

        private Image Thumbnail
        {
            get
            {
                try
                {
                    if (this.IsDriveReady != DriveReady.Ready)
                    {
                        return null;
                    }
                    int width = (int) ((this.FDriveInfo.TotalFreeSpace * 0x30L) / this.FDriveInfo.TotalSize);
                    if (((this.FThumbnail != null) && this.FThumbnail.IsAlive) && (width == this.FThumbnailWidth))
                    {
                        return (Image) this.FThumbnail.Target;
                    }
                    Image icon = base.GetIcon(new Size(0x30, 0x30), 0);
                    Bitmap image = new Bitmap(icon.Width, icon.Height + 8);
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        graphics.Clear(Color.Transparent);
                        graphics.DrawImage(icon, 0, 0);
                        Rectangle rect = new Rectangle(0, image.Height - 7, image.Width - 1, 6);
                        graphics.FillRectangle(Brushes.Blue, rect.Left, rect.Top, rect.Width - width, rect.Height);
                        graphics.FillRectangle(Brushes.Fuchsia, rect.Right - width, rect.Top, width, rect.Height);
                        graphics.DrawRectangle(Pens.Black, rect);
                        graphics.DrawLine(Pens.Black, rect.Right - width, rect.Top, rect.Right - width, rect.Bottom);
                    }
                    this.FThumbnailWidth = width;
                    this.FThumbnail = new WeakReference(image);
                    return image;
                }
                catch
                {
                    base.SetCapability(FileSystemItem.ItemCapability.HasThumbnail, false);
                    base.ResetAvailableSet();
                }
                return null;
            }
        }

        public override string ToolTip
        {
            get
            {
                switch (this.IsDriveReady)
                {
                    case DriveReady.Ready:
                        return base.Extender.ToolTip;

                    case DriveReady.TimeOut:
                        return Resources.sTimeOutElapsed;
                }
                return Resources.sToolTipDeviceNotReady;
            }
        }

        private string VolumeLabel
        {
            get
            {
                try
                {
                    if (this.IsDriveReady != DriveReady.Ready)
                    {
                        return null;
                    }
                    string volumeLabel = this.FDriveInfo.VolumeLabel;
                    if (string.IsNullOrEmpty(volumeLabel) || (this.FDriveInfo.DriveType == DriveType.CDRom))
                    {
                        string path = Path.Combine(this.FDriveInfo.RootDirectory.FullName, "autorun.inf");
                        if (System.IO.File.Exists(path))
                        {
                            volumeLabel = Ini.ReadValue(path, "autorun", "label");
                        }
                    }
                    if (string.IsNullOrEmpty(volumeLabel))
                    {
                        return null;
                    }
                    if (volumeLabel.Length > 0x20)
                    {
                        return volumeLabel.Substring(0, 0x20);
                    }
                    return volumeLabel;
                }
                catch (InvalidDataException)
                {
                    return null;
                }
                catch (IOException)
                {
                    return null;
                }
                catch (UnauthorizedAccessException)
                {
                    return null;
                }
            }
            set
            {
                this.FDriveInfo.VolumeLabel = string.IsNullOrEmpty(value) ? null : value;
            }
        }

        private string VolumeTarget
        {
            get
            {
                string remoteName = null;
                if (this.FDriveInfo.DriveType == DriveType.Network)
                {
                    remoteName = NetworkFileSystemCreator.GetRemoteName(this.DriveName);
                }
                if (string.IsNullOrEmpty(remoteName) && OS.IsWinNT)
                {
                    try
                    {
                        remoteName = VolumeInfo.ParseDosDevice(VolumeInfo.QueryDosDevice(this.DriveName));
                    }
                    catch
                    {
                    }
                }
                return remoteName;
            }
        }

        private enum DriveReady
        {
            NotReady,
            Ready,
            TimeOut
        }
    }
}

