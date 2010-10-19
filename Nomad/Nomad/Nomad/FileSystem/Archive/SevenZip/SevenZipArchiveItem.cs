namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.Commons;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading;
    using System.Windows.Forms;

    public class SevenZipArchiveItem : CustomPropertyProvider, ISimpleItem, IGetVirtualProperty, IGetStream, ISetOwnerWindow, ISequenceableItem
    {
        private FileAttributes? CachedAttributes;
        private Dictionary<ItemPropId, object> CachedProperties;
        private SevenZipSharedArchiveContext Context;
        private string FName;
        private IWin32Window FOwner;
        public readonly uint Index;

        internal SevenZipArchiveItem(SevenZipSharedArchiveContext context, uint index)
        {
            this.Context = context;
            this.Index = index;
        }

        public void CacheAllProperties()
        {
            this.Context.EnterArchiveLock();
            try
            {
                FileAttributes attributes = this.Attributes;
                this.GetProperty(ItemPropId.kpidPath);
                this.GetProperty(ItemPropId.kpidSize);
                this.GetProperty(ItemPropId.kpidPackedSize);
                this.GetProperty(ItemPropId.kpidCreationTime);
                this.GetProperty(ItemPropId.kpidLastAccessTime);
                this.GetProperty(ItemPropId.kpidLastWriteTime);
                this.GetProperty(ItemPropId.kpidCRC);
                this.GetProperty(ItemPropId.kpidComment);
                this.GetProperty(ItemPropId.kpidMethod);
                VirtualPropertySet availableProperties = this.AvailableProperties;
            }
            finally
            {
                Monitor.Exit(this.Context.ArchiveLock);
            }
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = new VirtualPropertySet();
            this.Context.EnterArchiveLock();
            try
            {
                uint numberOfProperties = this.Context.GetNumberOfProperties();
                for (uint i = 0; i < numberOfProperties; i++)
                {
                    string str;
                    ItemPropId id;
                    ushort num3;
                    this.Context.GetPropertyInfo(i, out str, out id, out num3);
                    switch (id)
                    {
                        case ItemPropId.kpidPath:
                            set[0] = true;
                            break;

                        case ItemPropId.kpidIsFolder:
                        case ItemPropId.kpidAttributes:
                        case ItemPropId.kpidEncrypted:
                            set[6] = true;
                            break;

                        case ItemPropId.kpidSize:
                            set[3] = true;
                            break;

                        case ItemPropId.kpidPackedSize:
                            set[5] = true;
                            break;

                        case ItemPropId.kpidCreationTime:
                            set[7] = true;
                            break;

                        case ItemPropId.kpidLastAccessTime:
                            set[9] = true;
                            break;

                        case ItemPropId.kpidLastWriteTime:
                            set[8] = true;
                            break;

                        case ItemPropId.kpidCRC:
                            set[0x18] = true;
                            break;

                        case ItemPropId.kpidMethod:
                            set[20] = true;
                            break;

                        case ItemPropId.kpidComment:
                            set[11] = true;
                            break;
                    }
                }
            }
            finally
            {
                Monitor.Exit(this.Context.ArchiveLock);
            }
            return set;
        }

        private object GetProperty(ItemPropId PropId)
        {
            object obj2 = null;
            if ((this.CachedProperties == null) || !this.CachedProperties.TryGetValue(PropId, out obj2))
            {
                try
                {
                    PropVariant variant = new PropVariant();
                    try
                    {
                        this.Context.GetProperty(this.Index, PropId, ref variant);
                        obj2 = variant.Value;
                    }
                    finally
                    {
                        variant.Clear();
                    }
                    if (this.CachedProperties == null)
                    {
                        this.CachedProperties = new Dictionary<ItemPropId, object>();
                    }
                    this.CachedProperties[PropId] = obj2;
                }
                catch (Exception exception)
                {
                    exception.Data["SevenZip_ItemPropId"] = PropId;
                    throw;
                }
            }
            return obj2;
        }

        private object GetSize(ItemPropId PropId)
        {
            object property = this.GetProperty(PropId);
            if (property != null)
            {
                return Convert.ToInt64(property);
            }
            return null;
        }

        public Stream GetStream()
        {
            return new ReadWriteExtractCallback(this.Context) { Owner = this.Owner }.GetItemStream(this);
        }

        public FileAttributes Attributes
        {
            get
            {
                if (!this.CachedAttributes.HasValue)
                {
                    this.Context.EnterArchiveLock();
                    try
                    {
                        this.CachedAttributes = new FileAttributes?((FileAttributes) (((0x800 | Convert.ToUInt32(this.GetProperty(ItemPropId.kpidAttributes))) | (Convert.ToBoolean(this.GetProperty(ItemPropId.kpidIsFolder)) ? 0x10 : 0)) | (this.Encrypted ? 0x4000 : 0)));
                    }
                    finally
                    {
                        Monitor.Exit(this.Context.ArchiveLock);
                    }
                }
                return this.CachedAttributes.Value;
            }
        }

        public bool Encrypted
        {
            get
            {
                return Convert.ToBoolean(this.GetProperty(ItemPropId.kpidEncrypted));
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

                    case 3:
                        return this.GetSize(ItemPropId.kpidSize);

                    case 5:
                        return this.GetSize(ItemPropId.kpidPackedSize);

                    case 6:
                        return this.Attributes;

                    case 7:
                        return (DateTime?) this.GetProperty(ItemPropId.kpidCreationTime);

                    case 8:
                        return (DateTime?) this.GetProperty(ItemPropId.kpidLastWriteTime);

                    case 9:
                        return (DateTime?) this.GetProperty(ItemPropId.kpidLastAccessTime);

                    case 11:
                        return (string) this.GetProperty(ItemPropId.kpidComment);

                    case 20:
                        return (string) this.GetProperty(ItemPropId.kpidMethod);

                    case 0x18:
                        return this.GetProperty(ItemPropId.kpidCRC);
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (this.FName == null)
                {
                    this.FName = (string) this.GetProperty(ItemPropId.kpidPath);
                }
                return this.FName;
            }
            internal set
            {
                this.FName = value;
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

        public ISequenceContext SequenceContext
        {
            get
            {
                return this.Context;
            }
        }
    }
}

