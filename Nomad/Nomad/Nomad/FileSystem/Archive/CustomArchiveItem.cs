namespace Nomad.FileSystem.Archive
{
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    public abstract class CustomArchiveItem : ExtensiblePropertyProvider, IPersistVirtualItem, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ISequenceable, ISerializable
    {
        protected static StringComparison ComparisonRule = StringComparison.OrdinalIgnoreCase;
        private static Dictionary<Uri, WeakReference> DeserializationContentMap;
        private static object DeserializationLock = new object();
        private const string EntryArchiveUri = "ArchiveUri";
        private const string EntryFormatClass = "FormatClass";
        protected const string EntryItemName = "ItemName";
        protected readonly Uri FArchiveUri;
        private VirtualItemVisualExtender FExtender;
        protected readonly ArchiveFormatInfo FFormatInfo;
        protected ISimpleItem FItem;

        protected CustomArchiveItem(SerializationInfo info, StreamingContext context)
        {
            this.FArchiveUri = (Uri) info.GetValue("ArchiveUri", typeof(Uri));
            Guid classId = (Guid) info.GetValue("FormatClass", typeof(Guid));
            this.FFormatInfo = ArchiveFormatManager.GetFormat(classId);
            if (this.FFormatInfo == null)
            {
                throw new SerializationException(string.Format("Archive class id ({0}) was not registered", classId));
            }
        }

        protected CustomArchiveItem(ISimpleItem item, Uri archiveUri, ArchiveFormatInfo formatInfo)
        {
            this.FItem = item;
            this.FArchiveUri = archiveUri;
            this.FFormatInfo = formatInfo;
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            int[] properties = new int[2];
            properties[1] = 2;
            VirtualPropertySet set = new VirtualPropertySet(properties);
            if (this.FItem != null)
            {
                set.Or(this.FItem.AvailableProperties);
            }
            return set;
        }

        private static IEnumerable<ISimpleItem> CreateContent(Uri archiveUri, ArchiveFormatInfo formatInfo)
        {
            IChangeVirtualFile file = VirtualItem.FromFullName(archiveUri.ToString(), VirtualItemType.File) as IChangeVirtualFile;
            if (file == null)
            {
                throw new SerializationException(string.Format("File '{0}' cannot be opened", archiveUri));
            }
            Stream archiveStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.RandomAccess, 0L);
            IEnumerable<ISimpleItem> enumerable = formatInfo.OpenArchiveContent(archiveStream, Path.GetFileName(archiveUri.AbsolutePath));
            if (enumerable != null)
            {
                return enumerable;
            }
            archiveStream.Close();
            return null;
        }

        public virtual bool Equals(IVirtualItem other)
        {
            CustomArchiveItem item = other as CustomArchiveItem;
            return ((((item != null) && (base.GetType() == item.GetType())) && (this.FFormatInfo.ClassId == item.FFormatInfo.ClassId)) && (this.FArchiveUri == item.FArchiveUri));
        }

        protected static IVirtualItem FromName(Uri archiveUri, IEnumerable<ISimpleItem> content, string name)
        {
            if (string.IsNullOrEmpty(name) || ((name.Length == 1) && ((name[0] == Path.DirectorySeparatorChar) || (name[0] == Path.AltDirectorySeparatorChar))))
            {
                return new ArchiveFolder(string.Empty, archiveUri, content, null);
            }
            bool flag = PathHelper.HasTrailingDirectorySeparator(name);
            name = PathHelper.ExcludeTrailingDirectorySeparator(name);
            foreach (ISimpleItem item in content)
            {
                if (item.Name.Equals(name, ComparisonRule))
                {
                    FileAttributes? nullable2;
                    FileAttributes? nullable = (FileAttributes?) item[6];
                    if (nullable.HasValue && ((((nullable2 = nullable) = nullable2.HasValue ? new FileAttributes?(((FileAttributes) nullable2.GetValueOrDefault()) & FileAttributes.Directory) : null).GetValueOrDefault() > 0) && nullable2.HasValue))
                    {
                        return new ArchiveFolder(item, archiveUri, content, null);
                    }
                    return new ArchiveFile(item, archiveUri, ((IGetArchiveFormatInfo) content).FormatInfo, (IVirtualFolder) FromName(archiveUri, content, PathHelper.IncludeTrailingDirectorySeparator(Path.GetDirectoryName(item.Name))));
                }
            }
            if (flag)
            {
                return new ArchiveFolder(name, archiveUri, content, null);
            }
            return null;
        }

        protected static IEnumerable<ISimpleItem> GetDeserializationContent(Uri archiveUri, ArchiveFormatInfo formatInfo)
        {
            object obj2;
            lock ((obj2 = DeserializationLock))
            {
                if (DeserializationContentMap != null)
                {
                    WeakReference reference;
                    if (DeserializationContentMap.TryGetValue(archiveUri, out reference) && reference.IsAlive)
                    {
                        return (IEnumerable<ISimpleItem>) reference.Target;
                    }
                }
                else
                {
                    DeserializationContentMap = new Dictionary<Uri, WeakReference>();
                }
            }
            IEnumerable<ISimpleItem> target = CreateContent(archiveUri, formatInfo);
            if (target == null)
            {
                throw new SerializationException(string.Format("Cannot open archive '{0}'. Either archive not found or unsupported format.", archiveUri));
            }
            lock ((obj2 = DeserializationLock))
            {
                DeserializationContentMap[archiveUri] = new WeakReference(target);
            }
            return target;
        }

        public override int GetHashCode()
        {
            switch (ComparisonRule)
            {
                case StringComparison.Ordinal:
                    return this.FullName.GetHashCode();

                case StringComparison.OrdinalIgnoreCase:
                    return this.FullName.ToLower().GetHashCode();
            }
            throw new InvalidOperationException("Cannot generate hash. Unsupported StringComparison rule.");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ArchiveUri", this.FArchiveUri);
            info.AddValue("FormatClass", this.FFormatInfo.ClassId);
            info.AddValue("ItemName", (this.FItem != null) ? this.FItem.Name : null);
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 0:
                    return this.Name;

                case 1:
                    return Path.GetExtension(this.Name);

                case 2:
                    return this.Extender.Type;

                case 6:
                    return this.Attributes;
            }
            if ((this.FItem != null) && this.FItem.IsPropertyAvailable(propertyId))
            {
                return this.FItem[propertyId];
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 0:
                case 2:
                    return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        protected static void RememberDeserializationContext(Uri archiveUri, IEnumerable<ISimpleItem> content)
        {
            lock (DeserializationLock)
            {
                if (DeserializationContentMap == null)
                {
                    DeserializationContentMap = new Dictionary<Uri, WeakReference>();
                    DeserializationContentMap.Add(archiveUri, new WeakReference(content));
                }
                else
                {
                    WeakReference reference;
                    if (!(DeserializationContentMap.TryGetValue(archiveUri, out reference) && reference.IsAlive))
                    {
                        DeserializationContentMap[archiveUri] = new WeakReference(content);
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.ToString(), this.FullName);
        }

        public virtual FileAttributes Attributes
        {
            get
            {
                return ((this.FItem != null) ? ((FileAttributes) this.FItem[6]) : FileAttributes.Normal);
            }
        }

        public bool Exists
        {
            get
            {
                if (this.FArchiveUri.IsFile)
                {
                    return File.Exists(this.FArchiveUri.LocalPath);
                }
                return true;
            }
        }

        protected VirtualItemVisualExtender Extender
        {
            get
            {
                if (this.FExtender == null)
                {
                    this.FExtender = new VirtualItemVisualExtender(this);
                }
                return this.FExtender;
            }
        }

        public abstract string FullName { get; }

        public VirtualHighligher Highlighter
        {
            get
            {
                return this.Extender.Highlighter;
            }
        }

        public abstract string Name { get; }

        public abstract IVirtualFolder Parent { get; }

        public ISequenceableItem SequenceableItem
        {
            get
            {
                return (this.FItem as ISequenceableItem);
            }
        }

        public string ShortName
        {
            get
            {
                return this.Name;
            }
        }
    }
}

