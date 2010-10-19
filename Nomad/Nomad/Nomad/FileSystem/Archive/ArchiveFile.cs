namespace Nomad.FileSystem.Archive
{
    using Nomad.Commons;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class ArchiveFile : ArchiveItem, IChangeVirtualFile, IVirtualFile, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ISetOwnerWindow
    {
        protected ArchiveFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ArchiveFile(ISimpleItem item, Uri archiveUri, ArchiveFormatInfo formatInfo, IVirtualFolder parent) : base(item, archiveUri, formatInfo, parent)
        {
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[1] = true;
            return set;
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            if (propertyId == 1)
            {
                return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            if ((mode != FileMode.Open) || (access != FileAccess.Read))
            {
                throw new ArgumentException("Only open file mode and read file access supported.");
            }
            Stream stream = ((IGetStream) base.FItem).GetStream();
            if (startOffset > 0L)
            {
                stream.Seek(startOffset, SeekOrigin.Current);
            }
            return stream;
        }

        public bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(this.Name);
            }
        }

        public IWin32Window Owner
        {
            get
            {
                if (base.FItem is ISetOwnerWindow)
                {
                    return ((ISetOwnerWindow) base.FItem).Owner;
                }
                return null;
            }
            set
            {
                if (base.FItem is ISetOwnerWindow)
                {
                    ((ISetOwnerWindow) base.FItem).Owner = value;
                }
            }
        }
    }
}

