namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons.Net;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public sealed class FtpFile : CustomFtpFile, IChangeVirtualItem, IPersistVirtualItem, ISetVirtualProperty, IChangeVirtualFile, IVirtualFile, IVirtualItemUI, IVirtualItem, ISimpleItem, IGetVirtualProperty, IEquatable<IVirtualItem>, ICreateVirtualLink, ICloneable
    {
        private const string EntryIsExists = "IsExists";
        private const string EntrySize = "Size";
        private bool? FIsExists;
        private long? FSize;

        public FtpFile(Uri absoluteUri) : base(new FtpContext(), absoluteUri, null)
        {
        }

        private FtpFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (name != null)
                {
                    if (!(name == "Size"))
                    {
                        if (name == "IsExists")
                        {
                            goto Label_005B;
                        }
                    }
                    else
                    {
                        this.FSize = new long?((long) current.Value);
                    }
                }
                continue;
            Label_005B:
                this.FIsExists = new bool?((bool) current.Value);
            }
        }

        internal FtpFile(FtpContext context, Uri absoluteUri, IVirtualFolder parent) : base(context, absoluteUri, parent)
        {
        }

        internal FtpFile(FtpContext context, Uri absoluteUri, bool? isExists, IVirtualFolder parent) : base(context, absoluteUri, parent)
        {
            this.FIsExists = isExists;
        }

        internal FtpFile(FtpContext context, Uri baseUri, string name, long? size, DateTime lastWriteTime, IVirtualFolder parent) : base(context, baseUri, name, lastWriteTime, parent)
        {
            this.FSize = size;
            this.FIsExists = true;
        }

        public LinkType CanCreateLinkIn(IVirtualFolder destFolder)
        {
            return ((destFolder is ICreateVirtualFile) ? LinkType.Default : LinkType.None);
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            VirtualPropertySet set = base.CreateAvailableSet();
            set[1] = true;
            set[2] = true;
            set[3] = true;
            return set;
        }

        public IVirtualItem CreateLink(IVirtualFolder destFolder, string name, LinkType type)
        {
            if (type != LinkType.Default)
            {
                throw new ArgumentException("Unsupported link type", "type");
            }
            return base.ItemInfo.CreateShortCut(destFolder, name);
        }

        public Image GetIcon(Size size, IconStyle style)
        {
            return base.Extender.GetIcon(size, (style & IconStyle.CanUseAlphaBlending) > 0);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (this.FSize.HasValue)
            {
                info.AddValue("Size", this.FSize.Value);
            }
            if (this.FIsExists.HasValue)
            {
                info.AddValue("IsExists", this.FIsExists.Value);
            }
        }

        public string GetPrefferedLinkName(LinkType type)
        {
            return ((type == LinkType.Default) ? Path.ChangeExtension(base.Name, ".url") : null);
        }

        public override object GetProperty(int propertyId)
        {
            switch (propertyId)
            {
                case 2:
                    return base.Extender.Type;

                case 3:
                    return this.FSize;
            }
            return base.GetProperty(propertyId);
        }

        public override PropertyAvailability GetPropertyAvailability(int propertyId)
        {
            switch (propertyId)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return PropertyAvailability.Normal;
            }
            return base.GetPropertyAvailability(propertyId);
        }

        public Stream Open(FileMode mode, FileAccess access, FileShare share, FileOptions options, long startOffset)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(base.ItemInfo.AbsoluteUri);
            request.UseBinary = true;
            request.UsePassive = base.ItemInfo.Context.UsePassive;
            if (base.ItemInfo.Context.Credentials != null)
            {
                request.Credentials = base.ItemInfo.Context.Credentials;
            }
            switch (mode)
            {
                case FileMode.CreateNew:
                    if ((access != FileAccess.Write) || Convert.ToBoolean(this.FIsExists))
                    {
                        break;
                    }
                    request.Method = "STOR";
                    return new WebRequestCreateFileStreamWrapper(request, base.ItemInfo.AbsoluteUri);

                case FileMode.Create:
                    if (access != FileAccess.Write)
                    {
                        break;
                    }
                    request.Method = "STOR";
                    if (!Convert.ToBoolean(this.FIsExists))
                    {
                        return new WebRequestCreateFileStreamWrapper(request, base.ItemInfo.AbsoluteUri);
                    }
                    return new WebRequestStreamWrapper(request);

                case FileMode.Open:
                    if (access != FileAccess.Read)
                    {
                        break;
                    }
                    request.ContentOffset = startOffset;
                    request.Method = "RETR";
                    return new WebRequestStreamWrapper((FtpWebResponse) request.GetResponse());

                case FileMode.Append:
                    if (access != FileAccess.Write)
                    {
                        break;
                    }
                    request.Method = "APPE";
                    return new WebRequestStreamWrapper(request);
            }
            throw new ArgumentException("Unsupported file mode and/or file access combination");
        }

        public bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public bool Exists
        {
            get
            {
                if (!this.FIsExists.HasValue)
                {
                    try
                    {
                        this.FSize = new long?(base.ItemInfo.Context.GetFileSize(base.ItemInfo.AbsoluteUri));
                        this.FIsExists = true;
                    }
                    catch (WebException exception)
                    {
                        switch (exception.Status)
                        {
                            case WebExceptionStatus.NameResolutionFailure:
                            case WebExceptionStatus.ConnectFailure:
                            case WebExceptionStatus.ReceiveFailure:
                            case WebExceptionStatus.SendFailure:
                            case WebExceptionStatus.RequestCanceled:
                            case WebExceptionStatus.ConnectionClosed:
                            case WebExceptionStatus.Timeout:
                                this.FIsExists = false;
                                goto Label_00DA;

                            case WebExceptionStatus.ProtocolError:
                                if (((FtpWebResponse) exception.Response).StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                                {
                                    throw;
                                }
                                this.FIsExists = false;
                                goto Label_00DA;
                        }
                        throw;
                    }
                }
            Label_00DA:
                return this.FIsExists.Value;
            }
        }

        public string Extension
        {
            get
            {
                return base.ItemInfo.Extension;
            }
        }
    }
}

