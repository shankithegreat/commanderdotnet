namespace Nomad.FileSystem.Network
{
    using Microsoft.IO;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Microsoft.Win32.Network;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Dialogs;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Serializable]
    internal class NetworkShareFolder : CustomFileSystemFolder, IPersistVirtualItem, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICreateVirtualFile, ICreateVirtualFolder, ISetOwnerWindow, ICloneable
    {
        private IWin32Window FOwner;
        private SafeShellItem FShellItem;

        public NetworkShareFolder(string folderPath) : this(folderPath, null)
        {
        }

        internal NetworkShareFolder(NETRESOURCE resource, IVirtualFolder parent) : base(resource.lpRemoteName, parent)
        {
            if (((resource.dwType != RESOURCETYPE.RESOURCETYPE_DISK) || (resource.dwDisplayType != RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE)) || ((resource.dwUsage & RESOURCEUSAGE.RESOURCEUSAGE_CONNECTABLE) == ((RESOURCEUSAGE) 0)))
            {
                throw new ArgumentException();
            }
            CheckFolderName(resource.lpRemoteName);
            base.AddPropertyToCache(11, string.IsNullOrEmpty(resource.lpComment) ? null : resource.lpComment);
        }

        public NetworkShareFolder(string folderPath, IVirtualFolder parent) : base(folderPath, parent)
        {
            CheckFolderName(folderPath);
            base.SetCapability(FileSystemItem.ItemCapability.CheckNetworkShare, true);
        }

        protected override void BeginGetContent()
        {
            base.BeginGetContent();
            if (base.CheckCapability(FileSystemItem.ItemCapability.CheckNetworkShare))
            {
                if (this.FOwner != null)
                {
                    Debug.WriteLine("CheckNetworkShare - Start", "NetworkShareFolder");
                    if ((Windows.GetFileAttributes(Path.Combine(base.FullName, "test")) == -1) && (Marshal.GetLastWin32Error() == 0x52e))
                    {
                        NETRESOURCE resource = new NETRESOURCE {
                            lpRemoteName = this.ComparableName,
                            dwType = RESOURCETYPE.RESOURCETYPE_DISK,
                            dwDisplayType = RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE,
                            dwUsage = RESOURCEUSAGE.RESOURCEUSAGE_CONNECTABLE
                        };
                        NetworkFileSystemCreator.AddConnection(this.FOwner, resource);
                    }
                    Debug.WriteLine("CheckNetworkShare - Finish", "NetworkShareFolder");
                }
                base.SetCapability(FileSystemItem.ItemCapability.CheckNetworkShare, false);
            }
        }

        protected override bool CacheProperty(int propertyId)
        {
            return (propertyId == 11);
        }

        public override bool CanSetProperty(int property)
        {
            return false;
        }

        private static void CheckFolderName(string folderName)
        {
            if (!(folderName.StartsWith(PathHelper.UncPrefix) && (folderName.IndexOf(Path.DirectorySeparatorChar, 2) >= 0)))
            {
                throw new ArgumentException();
            }
        }

        public object Clone()
        {
            return this.InternalClone();
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            set[0] = true;
            set[3] = base.CheckCapability(FileSystemItem.ItemCapability.HasSize);
            set[11] = true;
            return set;
        }

        public override ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            if (!base.CheckCapability(FileSystemItem.ItemCapability.HasShellItem))
            {
                this.CreateShellItem(owner);
            }
            return ((this.FShellItem == null) ? null : ShellContextMenuHelper.CreateContextMenu(owner, this.FShellItem.GetUIObjectOf<IContextMenu>(owner.Handle), options, onExecuteVerb));
        }

        private void CreateShellItem(IWin32Window owner)
        {
            this.FShellItem = new SafeShellItem(owner.Handle, base.FullName);
            base.SetCapability(FileSystemItem.ItemCapability.HasShellItem, true);
        }

        private static string GetDescription(string shareName)
        {
            string str2;
            int cb = 0x4000;
            IntPtr lpBuffer = Marshal.AllocHGlobal(cb);
            try
            {
                string str;
                NETRESOURCE lpNetResource = new NETRESOURCE {
                    lpRemoteName = PathHelper.ExcludeTrailingDirectorySeparator(shareName),
                    dwType = RESOURCETYPE.RESOURCETYPE_DISK,
                    dwDisplayType = RESOURCEDISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE,
                    dwUsage = RESOURCEUSAGE.RESOURCEUSAGE_CONNECTABLE
                };
                if (Winnetwk.WNetGetResourceInformation(ref lpNetResource, lpBuffer, ref cb, out str) == 0)
                {
                    lpNetResource = (NETRESOURCE) Marshal.PtrToStructure(lpBuffer, typeof(NETRESOURCE));
                    return (string.IsNullOrEmpty(lpNetResource.lpComment) ? null : lpNetResource.lpComment);
                }
                str2 = null;
            }
            finally
            {
                Marshal.FreeHGlobal(lpBuffer);
            }
            return str2;
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            return ImageProvider.Default.GetDefaultIcon(DefaultIcon.NetworkFolder, size);
        }

        public override object GetProperty(int propertyId)
        {
            if (propertyId == 11)
            {
                return GetDescription(this.ComparableName);
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            if (propertyId == 0)
            {
                return PropertyAvailability.Normal;
            }
            return (this.AvailableProperties[propertyId] ? PropertyAvailability.Normal : PropertyAvailability.None);
        }

        protected override VolumeInfo GetVolume()
        {
            return VolumeCache.Get(this.ComparableName);
        }

        protected override object InternalClone()
        {
            NetworkShareFolder folder = (NetworkShareFolder) base.InternalClone();
            folder.FOwner = this.FOwner;
            folder.FShellItem = this.FShellItem;
            folder.SetCapability(FileSystemItem.ItemCapability.HasShellItem, base.CheckCapability(FileSystemItem.ItemCapability.HasShellItem));
            return folder;
        }

        public override void ShowProperties(IWin32Window owner)
        {
            if (!base.CheckCapability(FileSystemItem.ItemCapability.HasShellItem))
            {
                this.CreateShellItem(owner);
            }
            if (!((this.FShellItem != null) && ShellContextMenuHelper.ExecuteVerb(owner, "properties", null, this.FShellItem.GetUIObjectOf<IContextMenu>(owner.Handle))))
            {
                using (PropertiesDialog dialog = new PropertiesDialog())
                {
                    dialog.Execute(owner, new IVirtualItem[] { this });
                }
            }
        }

        public override FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Directory;
            }
        }

        public override bool IsSlowIcon
        {
            get
            {
                return false;
            }
        }

        IWin32Window ISetOwnerWindow.Owner
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

        public override IVirtualFolder Parent
        {
            get
            {
                if (!base.CheckCapability(FileSystemItem.ItemCapability.HasParent))
                {
                    IVirtualFolder folder;
                    int index = base.FullName.IndexOf(Path.DirectorySeparatorChar, 2);
                    int length = base.FullName.IndexOf(Path.DirectorySeparatorChar, index + 1);
                    if ((length < 0) || (length == (base.FullName.Length - 1)))
                    {
                        folder = new NetworkFolder(base.FullName.Substring(0, index), null);
                    }
                    else
                    {
                        folder = new NetworkShareFolder(base.FullName.Substring(0, length));
                    }
                    base.Parent = folder;
                }
                return base.Parent;
            }
        }
    }
}

