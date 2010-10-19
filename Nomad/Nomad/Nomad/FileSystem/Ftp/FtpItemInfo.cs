namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Controls.Option;
    using Nomad.Dialogs;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.Properties;
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Security.Permissions;
    using System.Text;
    using System.Windows.Forms;

    [Serializable]
    internal class FtpItemInfo : ISerializable
    {
        public readonly FtpContext Context;
        private const string EntryAbsolureUri = "AbsoluteUri";
        private const string EntryContext = "Context";
        private const string EntryLastWriteTime = "LastWriteTime";
        private const string EntryName = "Name";
        private DateTime? FLastWriteTime;
        private string FName;
        private IVirtualFolder FParent;
        private FtpItemCapability HasCapabilities;

        protected FtpItemInfo(SerializationInfo info, StreamingContext context)
        {
            this.Context = FtpContext.GetDeserializationContext((FtpContext) info.GetValue("Context", typeof(FtpContext)));
            this.AbsoluteUri = (Uri) info.GetValue("AbsoluteUri", typeof(Uri));
            this.FName = info.GetString("Name");
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                if (current.Name == "LastWriteTime")
                {
                    this.FLastWriteTime = new DateTime?((DateTime) current.Value);
                    break;
                }
            }
            this.Initialize();
        }

        internal FtpItemInfo(FtpContext context, Uri absoluteUri, IVirtualFolder parent)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }
            if (absoluteUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException("Ftp uri scheme expected");
            }
            if (!absoluteUri.IsAbsoluteUri)
            {
                throw new ArgumentException("Relative uri is not supported");
            }
            this.Context = context;
            this.AbsoluteUri = absoluteUri;
            string[] segments = this.AbsoluteUri.Segments;
            if (segments.Length > 1)
            {
                this.FName = context.DecodeString(PathHelper.ExcludeTrailingDirectorySeparator(Uri.UnescapeDataString(segments[segments.Length - 1])));
            }
            else
            {
                UriBuilder builder = new UriBuilder(this.AbsoluteUri) {
                    UserName = string.Empty,
                    Password = string.Empty
                };
                if (this.AbsoluteUri.IsDefaultPort)
                {
                    builder.Port = -1;
                }
                this.FName = PathHelper.ExcludeTrailingDirectorySeparator(builder.ToString());
            }
            this.FParent = parent;
            this.SetCapability(FtpItemCapability.HasParent, (this.FParent != null) || (segments.Length < 2));
            this.Initialize();
        }

        internal FtpItemInfo(FtpContext context, Uri baseUri, string name, DateTime lastWriteTime, IVirtualFolder parent)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }
            if (baseUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException("Ftp uri scheme expected");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.Context = context;
            this.FLastWriteTime = new DateTime?(lastWriteTime);
            this.FName = name;
            this.AbsoluteUri = new Uri(baseUri, Path.Combine(baseUri.AbsolutePath, context.EncodeString(this.FName)));
            this.FParent = parent;
            this.SetCapability(FtpItemCapability.HasParent, this.FParent != null);
            this.Initialize();
        }

        public bool CheckCapability(FtpItemCapability capability)
        {
            return ((this.HasCapabilities & capability) == capability);
        }

        public VirtualPropertySet CreateAvailableSet()
        {
            int[] properties = new int[1];
            VirtualPropertySet set = new VirtualPropertySet(properties);
            if (this.FLastWriteTime.HasValue)
            {
                set[8] = true;
            }
            return set;
        }

        public IChangeVirtualFile CreateShortCut(IVirtualFolder destFolder, string name)
        {
            if (destFolder == null)
            {
                throw new ArgumentNullException("destFolder");
            }
            ICreateVirtualFile file = destFolder as ICreateVirtualFile;
            if (file == null)
            {
                throw new ArgumentException("destFolder does not implements ICreateVirtualFile");
            }
            IChangeVirtualFile file2 = file.CreateFile(name);
            using (Stream stream = file2.Open(FileMode.Create, FileAccess.Write, FileShare.Read, FileOptions.None, 0L))
            {
                using (IniWriter writer = new IniWriter(new StreamWriter(stream, Encoding.ASCII)))
                {
                    writer.WriteSection("InternetShortcut");
                    writer.WriteValue("URL", this.AbsoluteUri.AbsoluteUri);
                    writer.WriteLine();
                    writer.WriteSection("FTP");
                    writer.WriteValue("UsePassive", this.Context.UsePassive.ToString());
                    writer.WriteValue("UseCache", this.Context.UseCache.ToString());
                    writer.WriteValue("UsePrefetch", this.Context.UsePrefetch.ToString());
                    writer.WriteValue("StoreCredential", this.Context.StoreCredential.ToString());
                    writer.WriteValue("UploadFileNameCasing", this.Context.UploadFileNameCasing.ToString());
                    writer.WriteValue("Encoding", this.Context.ListEncoding.WebName);
                    if (this.Context.StoreCredential && (this.Context.Credentials != null))
                    {
                        NetworkCredential credential = this.Context.Credentials.GetCredential(null, string.Empty);
                        writer.WriteLine();
                        writer.WriteSection("Credential");
                        writer.WriteValue("UserName", credential.UserName);
                        writer.WriteValue("Password", ByteArrayHelper.ToString(SimpleEncrypt.EncryptString(credential.Password, DES.Create())));
                    }
                }
            }
            return (IChangeVirtualFile) VirtualItem.FromFullName(file2.FullName, VirtualItemType.File);
        }

        public bool ExecuteVerb(IWin32Window owner, string verb)
        {
            LocalFileSystemCreator.ExecuteVerb(owner, this.AbsoluteUri.ToString(), verb);
            return true;
        }

        public static FtpItemInfo GetItemInfo(IVirtualItem item)
        {
            FtpItem item2 = item as FtpItem;
            if (item2 != null)
            {
                return item2.ItemInfo;
            }
            FtpFolder folder = item as FtpFolder;
            if (folder != null)
            {
                return folder.ItemInfo;
            }
            return null;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Context", this.Context);
            info.AddValue("AbsoluteUri", this.AbsoluteUri);
            info.AddValue("Name", this.FName);
            if (this.FLastWriteTime.HasValue)
            {
                info.AddValue("LastWriteTime", this.FLastWriteTime.Value);
            }
        }

        public static Uri GetParent(Uri absoluteUri)
        {
            string[] segments = absoluteUri.Segments;
            if (segments.Length > 1)
            {
                StringBuilder builder = new StringBuilder(segments[0]);
                for (int i = 1; i < (segments.Length - 1); i++)
                {
                    builder.Append(segments[i]);
                }
                return new Uri(absoluteUri, builder.ToString());
            }
            return null;
        }

        private void Initialize()
        {
            if (!this.CheckCapability(FtpItemCapability.IsPathEncoded))
            {
                string components = this.AbsoluteUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                this.SetCapability(FtpItemCapability.IsPathEncoded, (this.Context.ListEncoding != Encoding.Default) && !string.Equals(components, this.Context.DecodeString(components)));
            }
        }

        public void Rename(string newName)
        {
            this.Context.Rename(this.AbsoluteUri, newName);
            FtpContext.RaiseFtpChangedEvent(WatcherChangeTypes.Renamed, this.AbsoluteUri, newName);
        }

        protected void SetCapability(FtpItemCapability capability, bool value)
        {
            if (value)
            {
                this.HasCapabilities |= capability;
            }
            else
            {
                this.HasCapabilities &= ~capability;
            }
            if (!(value || ((capability & FtpItemCapability.HasParent) <= 0)))
            {
                this.FParent = null;
            }
        }

        public void ShowProperties(IWin32Window owner, IVirtualItem item)
        {
            Encoding listEncoding = this.Context.ListEncoding;
            using (PropertiesDialog dialog = new PropertiesDialog())
            {
                FtpSettings settings = new FtpSettings {
                    UsePassive = this.Context.UsePassive,
                    UseCache = this.Context.UseCache,
                    UsePrefetch = this.Context.UsePrefetch,
                    StoreCredential = this.Context.StoreCredential,
                    UploadFileNameCasing = this.Context.UploadFileNameCasing,
                    Encoding = (this.Context.ListEncoding == Encoding.Default) ? string.Empty : this.Context.ListEncoding.WebName
                };
                dialog.AddNewTab(Resources.sFtpContext, new FtpOptionControl(settings));
                if (dialog.Execute(owner, new IVirtualItem[] { item }))
                {
                    this.Context.InitializeContext(settings);
                }
            }
            if (!((listEncoding == this.Context.ListEncoding) && this.Context.UseCache))
            {
                this.Context.Cache.Clear();
            }
        }

        public Uri AbsoluteUri { get; internal set; }

        public string Extension
        {
            get
            {
                return Path.GetExtension(this.Name);
            }
        }

        public string FullName
        {
            get
            {
                string str = this.AbsoluteUri.ToString();
                if (this.CheckCapability(FtpItemCapability.IsPathEncoded))
                {
                    return this.Context.DecodeString(str);
                }
                return str;
            }
        }

        public virtual object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0:
                        return this.Name;

                    case 1:
                        return this.Extension;

                    case 8:
                        return this.FLastWriteTime;
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                return this.FName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                this.FName = value;
                string a = this.Context.EncodeString(value);
                if (!string.Equals(a, value))
                {
                    this.SetCapability(FtpItemCapability.IsPathEncoded, true);
                }
                if (PathHelper.HasTrailingDirectorySeparator(this.AbsoluteUri.AbsolutePath))
                {
                    a = a + '/';
                }
                this.AbsoluteUri = new Uri(this.AbsoluteUri, Path.Combine(GetParent(this.AbsoluteUri).AbsolutePath, a));
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                if (!this.CheckCapability(FtpItemCapability.HasParent))
                {
                    Uri parent = GetParent(this.AbsoluteUri);
                    if (parent != null)
                    {
                        this.FParent = new FtpFolder(this.Context, parent);
                    }
                    this.SetCapability(FtpItemCapability.HasParent, true);
                }
                return this.FParent;
            }
        }
    }
}

