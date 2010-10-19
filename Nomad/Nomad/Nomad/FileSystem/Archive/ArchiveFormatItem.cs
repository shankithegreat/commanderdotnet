namespace Nomad.FileSystem.Archive
{
    using Nomad;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class ArchiveFormatItem : IVirtualLink, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private IChangeVirtualFile FArchiveFile;
        private FindFormatResult FFormatResult;
        private IVirtualFolder FParent;
        private string FToolTip;
        private bool OpenContentFailed;

        public ArchiveFormatItem(FindFormatResult findResult, IChangeVirtualFile archiveFile, IVirtualFolder parent)
        {
            this.FFormatResult = findResult;
            this.FArchiveFile = archiveFile;
            this.FParent = parent;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public bool Equals(IVirtualItem other)
        {
            ArchiveFormatItem item = other as ArchiveFormatItem;
            return (((item != null) && this.FArchiveFile.Equals(item.FArchiveFile)) && (this.FFormatResult.Format == item.FFormatResult.Format));
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return false;
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            return ImageProvider.Default.GetDefaultIcon(DefaultIcon.Folder, size);
        }

        public PropertyAvailability GetPropertyAvailability(int property)
        {
            switch (property)
            {
                case 0:
                case 2:
                case 11:
                case 13:
                    return PropertyAvailability.Normal;
            }
            return PropertyAvailability.None;
        }

        public void ShowProperties(IWin32Window owner)
        {
        }

        public FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Normal;
            }
        }

        public VirtualPropertySet AvailableProperties
        {
            get
            {
                int[] properties = new int[4];
                properties[1] = 2;
                properties[2] = 13;
                properties[3] = 11;
                return new VirtualPropertySet(properties);
            }
        }

        public string FullName
        {
            get
            {
                throw new NotImplementedException();
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

                    case 2:
                        return Resources.sTypeArchiveFormat;

                    case 11:
                        switch (this.FFormatResult.FindSource)
                        {
                            case FindFormatSource.Content:
                                return Resources.sArchiveDetectedByContent;

                            case FindFormatSource.Extension:
                                return Resources.sArchiveDetectedByExt;

                            case (FindFormatSource.Extension | FindFormatSource.Content):
                                return Resources.sArchiveDetectedByContentAndExt;
                        }
                        return null;

                    case 13:
                    {
                        byte num = 0;
                        if (!this.OpenContentFailed)
                        {
                            if ((this.FFormatResult.FindSource & FindFormatSource.Content) <= 0)
                            {
                                if (this.FFormatResult.FindSource == FindFormatSource.Extension)
                                {
                                    num = 0x55;
                                }
                                return num;
                            }
                            num = 0xff;
                            if (this.FFormatResult.Offset > 0)
                            {
                                num = (byte) (num - ((byte) (this.FFormatResult.Offset / 0x400)));
                            }
                        }
                        return num;
                    }
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                return this.FFormatResult.Format.Name;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.FParent;
            }
        }

        public string ShortName
        {
            get
            {
                return this.Name;
            }
        }

        public IVirtualItem Target
        {
            get
            {
                if (!this.OpenContentFailed)
                {
                    string fullName = this.FArchiveFile.FullName;
                    Stream archiveStream = this.FArchiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite, FileOptions.RandomAccess, 0L);
                    IEnumerable<ISimpleItem> archiveContent = this.FFormatResult.Format.OpenArchiveContent(archiveStream, fullName);
                    if (archiveContent != null)
                    {
                        return new ArchiveFolder(string.Empty, new Uri(fullName), archiveContent, this.FParent);
                    }
                    this.OpenContentFailed = true;
                    archiveStream.Close();
                }
                return null;
            }
        }

        public string ToolTip
        {
            get
            {
                if (this.FToolTip == null)
                {
                    this.FToolTip = VirtualItemVisualExtender.GetItemTooltip(this);
                }
                return this.FToolTip;
            }
        }
    }
}

