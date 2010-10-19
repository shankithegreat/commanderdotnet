namespace Nomad.FileSystem.LocalFile
{
    using Microsoft.Shell;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.Network;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Shell;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [Serializable]
    public class FileSystemShellLink : CustomFileSystemFile, IChangeVirtualItem, IPersistVirtualItem, ISetVirtualProperty, IChangeVirtualFile, IVirtualFile, IVirtualLink, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICloneable
    {
        private const string EntryDescription = "Description";
        private const string EntryHotkey = "Hotkey";
        private const string EntryResolve = "Resolve";
        private const string EntryTarget = "Target";
        private const string EntryTargetPath = "TargetPath";
        private IResolveLink FResolveLink;
        private const uint SL_CustomizeFolder = 0xefff0001;

        public FileSystemShellLink(FileInfo info) : base(info, null)
        {
        }

        public FileSystemShellLink(string fileName) : base(fileName, null)
        {
        }

        public FileSystemShellLink(FileInfo info, IVirtualFolder parent) : base(info, parent)
        {
        }

        protected FileSystemShellLink(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SimpleResolveLink link = new SimpleResolveLink();
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (name != null)
                {
                    if (!(name == "Target"))
                    {
                        if (name == "TargetPath")
                        {
                            goto Label_008A;
                        }
                        if (name == "Description")
                        {
                            goto Label_009F;
                        }
                        if (name == "Hotkey")
                        {
                            goto Label_00B4;
                        }
                        if (name == "Resolve")
                        {
                            goto Label_00C9;
                        }
                    }
                    else
                    {
                        link.Target = (IVirtualItem) current.Value;
                    }
                }
                continue;
            Label_008A:
                link.TargetPath = (string) current.Value;
                continue;
            Label_009F:
                link.Description = (string) current.Value;
                continue;
            Label_00B4:
                link.Hotkey = (Keys) current.Value;
                continue;
            Label_00C9:
                this.FResolveLink = (IResolveLink) current.Value;
            }
            if (this.FResolveLink != null)
            {
                base.SetCapability(FileSystemItem.ItemCapability.HasTarget, true);
            }
            else if (link.HasTarget)
            {
                this.FResolveLink = link;
                base.SetCapability(FileSystemItem.ItemCapability.HasTarget, true);
            }
        }

        public FileSystemShellLink(string fileName, IVirtualFolder parent) : base(fileName, parent)
        {
        }

        public override bool CanSetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 0x16:
                case 0x17:
                    this.TargetNeeded();
                    return base.CheckAnyCapability(FileSystemItem.ItemCapability.IsUrlLink | FileSystemItem.ItemCapability.IsShellLink);
            }
            return base.CanSetProperty(propertyId);
        }

        public object Clone()
        {
            return this.InternalClone();
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[11] = true;
            set[10] = true;
            return set;
        }

        public override Process Execute(IWin32Window owner)
        {
            if ((this.Target == null) && base.CheckCapability(FileSystemItem.ItemCapability.IsShellLink))
            {
                try
                {
                    using (ShellLink link = new ShellLink(base.FullName))
                    {
                        link.Resolve(owner, 0);
                        if (link.Modified)
                        {
                            link.Save(base.FullName);
                            this.FResolveLink = null;
                            base.SetCapability(FileSystemItem.ItemCapability.IsShellLink | FileSystemItem.ItemCapability.HasTarget, false);
                        }
                    }
                }
                catch (ExternalException)
                {
                }
            }
            return base.Execute(owner);
        }

        protected override Image GetItemIcon(Size size, bool defaultIcon)
        {
            Image icon = null;
            if (!(!base.CheckCapability(FileSystemItem.ItemCapability.UseTargetIcon) || defaultIcon))
            {
                icon = VirtualIcon.GetIcon(this.FResolveLink.Target, size);
            }
            return (icon ?? base.GetItemIcon(size, defaultIcon));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (base.CheckCapability(FileSystemItem.ItemCapability.HasTarget) && (this.FResolveLink != null))
            {
                info.AddValue("Resolve", this.FResolveLink);
            }
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 10:
                    this.TargetNeeded();
                    return ((this.FResolveLink != null) ? this.FResolveLink.TargetPath : null);

                case 11:
                {
                    this.TargetNeeded();
                    IResolveLink2 fResolveLink = this.FResolveLink as IResolveLink2;
                    return ((fResolveLink != null) ? fResolveLink.Description : null);
                }
                case 0x16:
                    return this.Hotkey;

                case 0x17:
                    return this.CustomizeFolder;
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 10:
                case 11:
                    return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        protected internal override void ResetVisualCache()
        {
            base.ResetVisualCache();
            base.SetCapability(FileSystemItem.ItemCapability.IsUrlLink | FileSystemItem.ItemCapability.IsShellLink | FileSystemItem.ItemCapability.HasTarget, false);
        }

        public override void SetProperty(int propertyId, object value)
        {
            switch (propertyId)
            {
                case 0x16:
                    this.Hotkey = (Keys) value;
                    break;

                case 0x17:
                    this.CustomizeFolder = value as ICustomizeFolder;
                    break;

                default:
                    base.SetProperty(propertyId, value);
                    break;
            }
        }

        private void TargetNeeded()
        {
            if (!base.CheckCapability(FileSystemItem.ItemCapability.HasTarget))
            {
                try
                {
                    using (ShellLink link = new ShellLink(base.FullName))
                    {
                        this.FResolveLink = VirtualItem.ResolveShellLink(link);
                        if (this.FResolveLink != null)
                        {
                            base.SetCapability(FileSystemItem.ItemCapability.UseTargetIcon, true);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(link.Path))
                            {
                                if (this.FResolveLink == null)
                                {
                                    this.FResolveLink = NetworkFileSystemCreator.ResolveShellLink(link);
                                }
                                if (this.FResolveLink == null)
                                {
                                    this.FResolveLink = ShellFileSystemCreator.ResolveShellLink(link);
                                }
                            }
                            if (this.FResolveLink == null)
                            {
                                this.FResolveLink = ResolveShellLink.Create(link);
                            }
                        }
                        base.SetCapability(FileSystemItem.ItemCapability.IsShellLink, true);
                    }
                }
                catch (ExternalException)
                {
                }
                if (this.FResolveLink == null)
                {
                    this.FResolveLink = FtpFileSystemCreator.ResolveLink(base.FullName);
                    if (this.FResolveLink != null)
                    {
                        base.SetCapability(FileSystemItem.ItemCapability.IsUrlLink, true);
                    }
                }
                base.SetCapability(FileSystemItem.ItemCapability.HasTarget, true);
            }
        }

        public ICustomizeFolder CustomizeFolder
        {
            get
            {
                this.TargetNeeded();
                if (base.CheckCapability(FileSystemItem.ItemCapability.IsShellLink))
                {
                    using (ShellLink link = new ShellLink(base.FullName))
                    {
                        byte[] block = link.GetBlock(0xefff0001);
                        if (block != null)
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            using (MemoryStream stream = new MemoryStream(block))
                            {
                                return (formatter.Deserialize(stream) as ICustomizeFolder);
                            }
                        }
                    }
                }
                if (base.CheckCapability(FileSystemItem.ItemCapability.IsUrlLink))
                {
                    UrlIni ini = new UrlIni(base.FullName);
                    ini.Read();
                    return ini;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.TargetNeeded();
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsShellLink))
                    {
                        if (!(value is SimpleCustomizeFolder))
                        {
                            value = new SimpleCustomizeFolder(value);
                        }
                        using (ShellLink link = new ShellLink(base.FullName))
                        {
                            IconLocation icon = value.Icon;
                            if (icon != null)
                            {
                                link.SetIconLocation(icon.IconFileName, icon.IconIndex);
                                value.Icon = null;
                            }
                            using (MemoryStream stream = new MemoryStream())
                            {
                                new BinaryFormatter().Serialize(stream, value);
                                link.AddBlock(0xefff0001, stream.GetBuffer(), 0, (int) stream.Length);
                            }
                            link.Save(base.FullName);
                        }
                    }
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsUrlLink))
                    {
                        UrlIni dest = new UrlIni(base.FullName);
                        dest.Read();
                        dest.Set(value);
                        dest.Write();
                    }
                }
            }
        }

        public Keys Hotkey
        {
            get
            {
                this.TargetNeeded();
                return ((this.FResolveLink != null) ? this.FResolveLink.Hotkey : Keys.None);
            }
            set
            {
                if (value != Keys.None)
                {
                    this.TargetNeeded();
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsShellLink))
                    {
                        using (ShellLink link = new ShellLink(base.FullName))
                        {
                            link.Hotkey = value;
                            link.Save(base.FullName);
                        }
                    }
                    if (base.CheckCapability(FileSystemItem.ItemCapability.IsUrlLink))
                    {
                        UrlIni ini = new UrlIni(base.FullName);
                        ini.Read();
                        ini.Hotkey = value;
                        ini.Write();
                    }
                }
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
                base.SetName(value);
            }
        }

        public IVirtualItem Target
        {
            get
            {
                this.TargetNeeded();
                try
                {
                    return ((this.FResolveLink != null) ? this.FResolveLink.Target : null);
                }
                catch (FileNotFoundException)
                {
                    return null;
                }
                catch (DirectoryNotFoundException)
                {
                    return null;
                }
            }
        }
    }
}

