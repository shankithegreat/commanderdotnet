namespace Nomad.FileSystem.Ftp
{
    using Nomad;
    using Nomad.Commons.Drawing;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public sealed class FtpLink : CustomFtpFile, IChangeVirtualItem, IPersistVirtualItem, ISetVirtualProperty, IVirtualItemUI, IVirtualLink, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>
    {
        private const string EntryLinkTarget = "Target";
        private const string EntrySymbolicLink = "SymbolicLink";
        private IVirtualItem FLinkTarget;
        private Uri FSymbolicLink;

        private FtpLink(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.FSymbolicLink = (Uri) info.GetValue("SymbolicLink", typeof(Uri));
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                if (current.Name == "Target")
                {
                    this.FLinkTarget = (IVirtualItem) current.Value;
                    break;
                }
            }
        }

        public FtpLink(FtpContext context, Uri baseUri, string name, string linkTarget, DateTime lastWriteTime, IVirtualFolder parent) : base(context, baseUri, name, lastWriteTime, parent)
        {
            this.FSymbolicLink = new Uri(baseUri, Path.Combine(baseUri.AbsolutePath, base.ItemInfo.Context.EncodeString(linkTarget)));
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[10] = true;
            return set;
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            if (base.Extender.HasIcon(size))
            {
                return base.Extender.GetIcon(size, (style & IconStyle.CanUseAlphaBlending) > 0);
            }
            Image icon = ((IVirtualItemUI) this.Target).GetIcon(size, style);
            Image defaultIcon = ImageProvider.Default.GetDefaultIcon(DefaultIcon.OverlayLink, icon.Size);
            if (defaultIcon != null)
            {
                icon = ImageHelper.MergeImages(new Image[] { icon, defaultIcon });
            }
            base.Extender.SetIcon(icon, size);
            return icon;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SymbolicLink", this.FSymbolicLink);
            if (this.FLinkTarget != null)
            {
                info.AddValue("Target", this.FLinkTarget);
            }
        }

        public override object GetProperty(int propertyId)
        {
            if (propertyId == 10)
            {
                return base.ItemInfo.Context.DecodeString(this.FSymbolicLink.ToString());
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            return ((propertyId == 10) ? PropertyAvailability.Normal : base.GetPropertyAvailability(propertyId));
        }

        public bool Exists
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public IVirtualItem Target
        {
            get
            {
                if (this.FLinkTarget == null)
                {
                    this.FLinkTarget = FtpItem.FromUri(base.ItemInfo.Context, this.FSymbolicLink, VirtualItemType.Unknown, null);
                }
                return this.FLinkTarget;
            }
        }
    }
}

