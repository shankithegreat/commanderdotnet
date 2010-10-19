namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons;
    using Nomad.Commons.Net;
    using Nomad.Dialogs;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Permissions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class FtpContext : Nomad.Commons.Net.Ftp, ISetOwnerWindow, ISerializable
    {
        private static List<WeakReference> ChangedListeners;
        private static ReaderWriterLock ChangedLock;
        private readonly Guid ContextId;
        private static Dictionary<Guid, WeakReference> DeserializationContexts;
        private const string EntryBookmarkCredential = "StoreCredential";
        private const string EntryContextId = "ContextId";
        private const string EntryEncoding = "Encoding";
        private const string EntryPassword = "Password";
        private const string EntryUploadFileNameCasing = "UploadFileNameCasing";
        private const string EntryUseCache = "UseCache";
        private const string EntryUsePassive = "UsePassive";
        private const string EntryUsePrefetch = "UsePrefetch";
        private const string EntryUserName = "UserName";
        private Cache<Uri, IEnumerable<IVirtualItem>> FCache;
        private IWin32Window FOwner;
        private EventHandler<FtpChangedEventArg> FtpChangedHandler;
        public Regex ListPattern;
        public bool StoreCredential;
        public CharacterCasing UploadFileNameCasing;
        public bool UseCache;
        public bool UsePrefetch;

        internal static  event EventHandler<FtpChangedEventArg> FtpChanged
        {
            add
            {
                if (ChangedListeners == null)
                {
                    ChangedListeners = new List<WeakReference>();
                    ChangedLock = new ReaderWriterLock();
                }
                ChangedLock.AcquireWriterLock(-1);
                try
                {
                    ChangedListeners.Add(new WeakReference(value));
                }
                finally
                {
                    ChangedLock.ReleaseWriterLock();
                }
            }
            remove
            {
                Predicate<WeakReference> match = null;
                if (ChangedListeners != null)
                {
                    ChangedLock.AcquireWriterLock(-1);
                    try
                    {
                        if (match == null)
                        {
                            match = delegate (WeakReference weakHandler) {
                                return !weakHandler.IsAlive || (weakHandler.Target == value);
                            };
                        }
                        ChangedListeners.RemoveAll(match);
                    }
                    finally
                    {
                        ChangedLock.ReleaseWriterLock();
                    }
                }
            }
        }

        public FtpContext()
        {
            this.ContextId = Guid.NewGuid();
            this.InitializeContext(FtpSettings.Default);
            this.FtpChangedHandler = new EventHandler<FtpChangedEventArg>(this.OnFtpChanged);
            FtpChanged += this.FtpChangedHandler;
        }

        protected FtpContext(SerializationInfo info, StreamingContext context)
        {
            this.ContextId = (Guid) info.GetValue("ContextId", typeof(Guid));
            base.UsePassive = info.GetBoolean("UsePassive");
            this.UseCache = info.GetBoolean("UseCache");
            this.UsePrefetch = info.GetBoolean("UsePrefetch");
            this.StoreCredential = info.GetBoolean("StoreCredential");
            this.UploadFileNameCasing = (CharacterCasing) info.GetInt32("UploadFileNameCasing");
            string userName = null;
            string password = string.Empty;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (name != null)
                {
                    if (!(name == "UserName"))
                    {
                        if (name == "Password")
                        {
                            goto Label_00E1;
                        }
                        if (name == "Encoding")
                        {
                            goto Label_00FA;
                        }
                    }
                    else
                    {
                        userName = (string) current.Value;
                    }
                }
                continue;
            Label_00E1:
                password = SimpleEncrypt.DecryptString((byte[]) current.Value, DES.Create());
                continue;
            Label_00FA:
                base.ListEncoding = Encoding.GetEncoding(Convert.ToInt32(current.Value));
            }
            if (userName != null)
            {
                base.Credentials = new NetworkCredential(userName, password);
            }
            this.FtpChangedHandler = new EventHandler<FtpChangedEventArg>(this.OnFtpChanged);
            FtpChanged += this.FtpChangedHandler;
        }

        internal static FtpContext GetDeserializationContext(FtpContext context)
        {
            WeakReference reference;
            if (DeserializationContexts == null)
            {
                DeserializationContexts = new Dictionary<Guid, WeakReference>();
                DeserializationContexts.Add(context.ContextId, new WeakReference(context));
                return context;
            }
            if (DeserializationContexts.TryGetValue(context.ContextId, out reference) && reference.IsAlive)
            {
                return (FtpContext) reference.Target;
            }
            DeserializationContexts[context.ContextId] = new WeakReference(context);
            return context;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ContextId", this.ContextId);
            info.AddValue("UsePassive", base.UsePassive);
            info.AddValue("UseCache", this.UseCache);
            info.AddValue("UsePrefetch", this.UsePrefetch);
            info.AddValue("StoreCredential", this.StoreCredential);
            info.AddValue("UploadFileNameCasing", (int) this.UploadFileNameCasing);
            info.AddValue("Encoding", base.ListEncoding.CodePage);
            if ((base.Credentials != null) && this.StoreCredential)
            {
                NetworkCredential credential = base.Credentials.GetCredential(null, string.Empty);
                info.AddValue("UserName", credential.UserName);
                info.AddValue("Password", SimpleEncrypt.EncryptString(credential.Password, DES.Create()));
            }
        }

        internal void InitializeContext(FtpSettings settings)
        {
            base.UsePassive = settings.UsePassive;
            this.UseCache = settings.UseCache;
            this.UsePrefetch = settings.UsePrefetch;
            this.StoreCredential = settings.StoreCredential;
            this.UploadFileNameCasing = settings.UploadFileNameCasing;
            if (!string.IsNullOrEmpty(settings.Encoding))
            {
                base.ListEncoding = Encoding.GetEncoding(settings.Encoding);
            }
            else
            {
                base.ListEncoding = Encoding.Default;
            }
        }

        protected override void OnCredentialsNeeded(CredentialsNeededEventArgs e)
        {
            SecureString str2;
            string userName = string.Empty;
            if (BasicLoginDialog.GetLogin(this.FOwner, string.Format(Resources.sFtpLoginNeeded, e.Uri), ref userName, out str2))
            {
                e.Credentials = new NetworkCredential(userName, SimpleEncrypt.DecryptString(str2));
            }
        }

        private void OnFtpChanged(object sender, FtpChangedEventArg e)
        {
            this.Cache.Lock.AcquireWriterLock(-1);
            try
            {
                this.Cache.Remove(e.BaseUri);
                if (e.ChangeType != WatcherChangeTypes.Created)
                {
                    this.Cache.Remove(e.AbsoluteUri);
                }
            }
            finally
            {
                this.Cache.Lock.ReleaseWriterLock();
            }
        }

        internal static void RaiseFtpChangedEvent(WatcherChangeTypes changeType, Uri uri, string newName)
        {
            if (ChangedListeners != null)
            {
                List<EventHandler<FtpChangedEventArg>> list;
                FtpChangedEventArg arg;
                Uri parent = FtpItemInfo.GetParent(uri);
                if (parent == null)
                {
                    throw new ArgumentException("Cannot get parent from this uri");
                }
                ChangedLock.AcquireReaderLock(-1);
                try
                {
                    list = new List<EventHandler<FtpChangedEventArg>>(ChangedListeners.Count);
                    foreach (WeakReference reference in ChangedListeners)
                    {
                        if (reference.IsAlive)
                        {
                            list.Add((EventHandler<FtpChangedEventArg>) reference.Target);
                        }
                    }
                }
                finally
                {
                    ChangedLock.ReleaseReaderLock();
                }
                if (changeType == WatcherChangeTypes.Renamed)
                {
                    arg = new FtpChangedEventArg(uri, parent, newName);
                }
                else
                {
                    arg = new FtpChangedEventArg(changeType, uri, parent);
                }
                foreach (EventHandler<FtpChangedEventArg> handler in list)
                {
                    if (handler != null)
                    {
                        handler(null, arg);
                    }
                }
            }
        }

        public Cache<Uri, IEnumerable<IVirtualItem>> Cache
        {
            get
            {
                if (this.FCache == null)
                {
                    this.FCache = new Cache<Uri, IEnumerable<IVirtualItem>>();
                }
                return this.FCache;
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
    }
}

