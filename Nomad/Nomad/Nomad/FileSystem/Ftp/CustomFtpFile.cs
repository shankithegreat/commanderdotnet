namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    public abstract class CustomFtpFile : FtpItem, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ISetOwnerWindow
    {
        private VirtualItemVisualExtender FExtender;

        protected CustomFtpFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected CustomFtpFile(FtpContext context, Uri absoluteUri, IVirtualFolder parent) : base(context, absoluteUri, parent)
        {
        }

        protected CustomFtpFile(FtpContext context, Uri baseUri, string name, DateTime lastWriteTime, IVirtualFolder parent) : base(context, baseUri, name, lastWriteTime, parent)
        {
        }

        public bool CanMoveTo(IVirtualFolder dest)
        {
            return false;
        }

        public override bool CanSetProperty(int propertyId)
        {
            return ((propertyId == 0) || base.CanSetProperty(propertyId));
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[6] = true;
            return set;
        }

        public ContextMenuStrip CreateContextMenuStrip(IWin32Window owner, ContextMenuOptions options, EventHandler<ExecuteVerbEventArgs> onExecuteVerb)
        {
            return null;
        }

        public void Delete(bool SendToBin)
        {
            base.ItemInfo.Context.DeleteFile(base.ItemInfo.AbsoluteUri);
            FtpContext.RaiseFtpChangedEvent(WatcherChangeTypes.Deleted, base.ItemInfo.AbsoluteUri, null);
            VirtualItem.RaiseVirtualItemChangedEvent(WatcherChangeTypes.Deleted, this);
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            return base.ItemInfo.ExecuteVerb(owner, verb);
        }

        public override object GetProperty(int propertyId)
        {
            return ((propertyId == 6) ? FileAttributes.Normal : base.GetProperty(propertyId));
        }

        public IVirtualItem MoveTo(IVirtualFolder dest)
        {
            throw new NotSupportedException();
        }

        protected internal override void ResetVisualCache()
        {
            this.FExtender = null;
        }

        public override void SetProperty(int propertyId, object value)
        {
            if (propertyId == 0)
            {
                this.Name = (string) value;
            }
            else
            {
                base.SetProperty(propertyId, value);
            }
        }

        public void ShowProperties(IWin32Window owner)
        {
            base.ItemInfo.ShowProperties(owner, this);
        }

        public FileAttributes Attributes
        {
            get
            {
                return FileAttributes.Normal;
            }
        }

        public bool CanSendToBin
        {
            get
            {
                return false;
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

        public VirtualHighligher Highlighter
        {
            get
            {
                return this.Extender.Highlighter;
            }
        }

        public string Name
        {
            get
            {
                return base.ItemInfo.Name;
            }
            set
            {
                base.ItemInfo.Rename(value);
            }
        }

        public IWin32Window Owner
        {
            get
            {
                return base.ItemInfo.Context.Owner;
            }
            set
            {
                base.ItemInfo.Context.Owner = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return this.Extender.ToolTip;
            }
        }
    }
}

