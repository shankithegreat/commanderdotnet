namespace Nomad.FileSystem.Ftp
{
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Security.Permissions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    [DebuggerDisplay("{GetType().Name}, {ItemInfo.AbsoluteUri}")]
    public abstract class FtpItem : ExtensiblePropertyProvider, ISerializable
    {
        private const string EntryItemInfo = "ItemInfo";
        private static Regex FListPaternDos;
        private static Regex FListPatternUnix;
        internal readonly FtpItemInfo ItemInfo;

        protected FtpItem(SerializationInfo info, StreamingContext context)
        {
            this.ItemInfo = (FtpItemInfo) info.GetValue("ItemInfo", typeof(FtpItemInfo));
        }

        protected FtpItem(FtpContext context, Uri absoluteUri, IVirtualFolder parent)
        {
            this.ItemInfo = new FtpItemInfo(context, absoluteUri, parent);
        }

        protected FtpItem(FtpContext context, Uri baseUri, string name, DateTime lastWriteTime, IVirtualFolder parent)
        {
            this.ItemInfo = new FtpItemInfo(context, baseUri, name, lastWriteTime, parent);
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            return this.ItemInfo.CreateAvailableSet();
        }

        public bool Equals(IVirtualItem other)
        {
            FtpItem item = other as FtpItem;
            return ((item != null) && this.ItemInfo.AbsoluteUri.Equals(item.ItemInfo.AbsoluteUri));
        }

        public override bool Equals(object obj)
        {
            FtpItem item = obj as FtpItem;
            if (item != null)
            {
                return this.ItemInfo.AbsoluteUri.Equals(item.ItemInfo.AbsoluteUri);
            }
            return base.Equals(obj);
        }

        public static IVirtualItem FromUri(FtpContext context, Uri absoluteUri, VirtualItemType type, IVirtualFolder parent)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (absoluteUri == null)
            {
                throw new ArgumentNullException("absoluteUri");
            }
            if (!absoluteUri.IsAbsoluteUri)
            {
                throw new ArgumentException("This operation is not supported for a relative URI.");
            }
            if (type == VirtualItemType.Unknown)
            {
                if (absoluteUri.AbsolutePath.EndsWith("/", StringComparison.Ordinal))
                {
                    type = VirtualItemType.Folder;
                }
                else
                {
                    try
                    {
                        context.GetFileSize(absoluteUri);
                        type = VirtualItemType.File;
                    }
                    catch
                    {
                        try
                        {
                            context.PrintWorkingDirectory(absoluteUri);
                            type = VirtualItemType.Folder;
                        }
                        catch
                        {
                            throw new ArgumentException(string.Format("Cannot determine file type from uri ({0})", absoluteUri));
                        }
                    }
                }
            }
            switch (type)
            {
                case VirtualItemType.Folder:
                    return new FtpFolder(context, absoluteUri, parent);

                case VirtualItemType.File:
                    try
                    {
                        Uri baseUri = FtpItemInfo.GetParent(absoluteUri);
                        IVirtualItem item = null;
                        foreach (string str in context.ListDirectoryDetails(absoluteUri))
                        {
                            item = ParseListString(context, baseUri, str, parent);
                            break;
                        }
                        if (item != null)
                        {
                            return item;
                        }
                    }
                    catch (WebException)
                    {
                    }
                    return new FtpFile(context, absoluteUri, parent);
            }
            throw new InvalidEnumArgumentException("type", (int) type, typeof(VirtualItemType));
        }

        public static IVirtualItem FromUrlFile(Stream urlFileContent, IVirtualFolder parent)
        {
            FtpContext context = new FtpContext();
            Uri absoluteUri = null;
            string userName = null;
            byte[] source = null;
            using (IniReader reader = new IniReader(new StreamReader(urlFileContent, Encoding.ASCII)))
            {
                while (reader.Read())
                {
                    if (reader.ElementType != IniElementType.KeyValuePair)
                    {
                        continue;
                    }
                    if (reader.CheckCurrentKey("InternetShortcut", "URL"))
                    {
                        absoluteUri = new Uri(reader.CurrentValue);
                    }
                    switch (reader.CheckCurrentKey("FTP", new string[] { "UsePassive", "UseCache", "UsePrefetch", "StoreCredential", "UploadFileNameCasing", "Encoding" }))
                    {
                        case 0:
                            context.UsePassive = Convert.ToBoolean(reader.CurrentValue);
                            break;

                        case 1:
                            context.UseCache = Convert.ToBoolean(reader.CurrentValue);
                            break;

                        case 2:
                            context.UsePrefetch = Convert.ToBoolean(reader.CurrentValue);
                            break;

                        case 3:
                            context.StoreCredential = Convert.ToBoolean(reader.CurrentValue);
                            break;

                        case 4:
                            context.UploadFileNameCasing = (CharacterCasing) System.Enum.Parse(typeof(CharacterCasing), reader.CurrentValue);
                            break;

                        case 5:
                            context.ListEncoding = Encoding.GetEncoding(reader.CurrentValue);
                            break;
                    }
                    switch (reader.CheckCurrentKey("Credential", new string[] { "UserName", "Password" }))
                    {
                        case 0:
                            userName = reader.CurrentValue;
                            break;

                        case 1:
                            source = ByteArrayHelper.Parse(reader.CurrentValue);
                            break;
                    }
                }
            }
            if (userName != null)
            {
                context.Credentials = new NetworkCredential(userName, (source == null) ? string.Empty : SimpleEncrypt.DecryptString(source, DES.Create()));
            }
            if ((absoluteUri != null) && (absoluteUri.Scheme == Uri.UriSchemeFtp))
            {
                return FromUri(context, absoluteUri, VirtualItemType.Unknown, parent);
            }
            return null;
        }

        public override int GetHashCode()
        {
            return this.ItemInfo.AbsoluteUri.GetHashCode();
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemInfo", this.ItemInfo);
        }

        public override object GetProperty(int propertyId)
        {
            return this.ItemInfo[propertyId];
        }

        internal static IVirtualItem ParseListString(FtpContext context, Uri baseUri, string line, IVirtualFolder parent)
        {
            Match match;
            int num;
            if (context.ListPattern == null)
            {
                match = ListPatternUnix.Match(line);
                if (match.Success)
                {
                    context.ListPattern = ListPatternUnix;
                }
                else
                {
                    match = ListPatternDos.Match(line);
                    if (match.Success)
                    {
                        context.ListPattern = ListPatternDos;
                    }
                }
            }
            else
            {
                match = context.ListPattern.Match(line);
            }
            if (!match.Success)
            {
                return null;
            }
            if (match.Groups["month"].Success)
            {
                num = Convert.ToInt32(match.Groups["month"].Value);
            }
            else
            {
                string str = match.Groups["monthname"].Value;
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                num = 1;
                foreach (string str2 in CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedMonthNames)
                {
                    if (str2.Equals(str))
                    {
                        break;
                    }
                    num++;
                }
            }
            int day = Convert.ToInt32(match.Groups["day"].Value);
            int year = 0;
            if (match.Groups["year"].Success)
            {
                year = Convert.ToInt32(match.Groups["year"].Value);
            }
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            else if (year < 100)
            {
                year = 0x7d0 + year;
            }
            DateTime lastWriteTime = new DateTime(year, num, day);
            if (match.Groups["hour"].Success)
            {
                int hours = Convert.ToInt32(match.Groups["hour"].Value);
                if (match.Groups["ampm"].Success && match.Groups["ampm"].Value.Equals("pm", StringComparison.OrdinalIgnoreCase))
                {
                    hours += 12;
                }
                lastWriteTime = lastWriteTime.Add(new TimeSpan(hours, Convert.ToInt32(match.Groups["min"].Value), 0));
            }
            string name = match.Groups["name"].Value;
            switch (name)
            {
                case ".":
                case "..":
                    return null;
            }
            switch (match.Groups["type"].Value.ToLower())
            {
                case "d":
                    return new FtpFolder(context, baseUri, name, lastWriteTime, parent);

                case "l":
                    return new FtpLink(context, baseUri, name, match.Groups["linktarget"].Value, lastWriteTime, parent);
            }
            return new FtpFile(context, baseUri, name, new long?(Convert.ToInt64(match.Groups["size"].Value)), lastWriteTime, parent);
        }

        protected internal virtual void ResetVisualCache()
        {
        }

        public override string ToString()
        {
            return this.ItemInfo.AbsoluteUri.ToString();
        }

        public string FullName
        {
            get
            {
                return this.ItemInfo.FullName;
            }
        }

        private static Regex ListPatternDos
        {
            get
            {
                if (FListPaternDos == null)
                {
                    FListPaternDos = new Regex(FtpSettings.Default.ListPatternDos, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
                return FListPaternDos;
            }
        }

        private static Regex ListPatternUnix
        {
            get
            {
                if (FListPatternUnix == null)
                {
                    FListPatternUnix = new Regex(FtpSettings.Default.ListPatternUnix, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
                return FListPatternUnix;
            }
        }

        public IVirtualFolder Parent
        {
            get
            {
                return this.ItemInfo.Parent;
            }
        }

        public string ShortName
        {
            get
            {
                return this.ItemInfo.Name;
            }
        }
    }
}

