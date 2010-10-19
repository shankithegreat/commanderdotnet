namespace Nomad.FileSystem.Archive.SevenZip
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.Drawing;
    using Nomad.Commons.Threading;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.Remoting;
    using System.Security;
    using System.Threading;

    public class SevenZipFormatInfo : PersistArchiveFormatInfo
    {
        public string[] AddExtension;
        private AsyncCheckContentHandler AsyncCheckContent;
        private ArchiveFormatCapabilities FCapabilities;
        private Guid FClassId;
        private static Dictionary<Guid, KnownSevenZipFormat> FFormatClassMap;
        private static List<SevenZipFormatInfo> FFormats;
        private int FormatImageIndex = -2;
        private int FormatIndex;
        private string FormatName;
        private SevenZipFormatCapabilities FSevenZipCapabilities;
        private GetHandlerPropertyDelegate GetHandlerProperty;
        private GetHandlerProperty2Delegate GetHandlerProperty2;
        private Microsoft.Win32.SafeHandles.SafeLibraryHandle Library;
        private Version LibraryVersion;
        protected byte[] StartSignature;

        private SevenZipFormatInfo(Microsoft.Win32.SafeHandles.SafeLibraryHandle library, Version version, int formatIndex)
        {
            this.Library = library;
            this.LibraryVersion = version;
            this.FormatIndex = formatIndex;
            IntPtr procAddress = Windows.GetProcAddress(this.Library, "GetHandlerProperty");
            if (procAddress != IntPtr.Zero)
            {
                this.GetHandlerProperty = (GetHandlerPropertyDelegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetHandlerPropertyDelegate));
            }
            procAddress = Windows.GetProcAddress(this.Library, "GetHandlerProperty2");
            if (procAddress != IntPtr.Zero)
            {
                this.GetHandlerProperty2 = (GetHandlerProperty2Delegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetHandlerProperty2Delegate));
            }
            if ((this.GetHandlerProperty == null) && (this.GetHandlerProperty2 == null))
            {
                throw new ApplicationException("Library is not valid 7-Zip archive library");
            }
            PropVariant variant = new PropVariant();
            try
            {
                this.GetFormatProperty(ArchivePropId.kName, ref variant);
                this.FormatName = (string) variant.Value;
                this.GetFormatProperty(ArchivePropId.kClassID, ref variant);
                this.FClassId = new Guid(BstrToByte(variant.p));
                this.GetFormatProperty(ArchivePropId.kStartSignature, ref variant);
                if (variant.VarType == VarEnum.VT_BSTR)
                {
                    this.StartSignature = BstrToByte(variant.p);
                }
                this.GetFormatProperty(ArchivePropId.kUpdate, ref variant);
                this.FCapabilities = Convert.ToBoolean(variant.Value) ? ArchiveFormatCapabilities.CreateArchive : ((ArchiveFormatCapabilities) 0);
            }
            finally
            {
                variant.Clear();
            }
            KnownSevenZipFormat knownFormat = this.KnownFormat;
            switch (knownFormat)
            {
                case KnownSevenZipFormat.Arj:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(CheckArj.Check);
                    break;

                case KnownSevenZipFormat.BZip2:
                case KnownSevenZipFormat.Deb:
                case KnownSevenZipFormat.GZip:
                case KnownSevenZipFormat.NTFS:
                case KnownSevenZipFormat.Rpm:
                case KnownSevenZipFormat.VHD:
                case KnownSevenZipFormat.Xz:
                case KnownSevenZipFormat.Z:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(this.CheckStartContent);
                    break;

                case KnownSevenZipFormat.FAT:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(CheckFileSystem.CheckFAT);
                    break;

                case KnownSevenZipFormat.MBR:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(CheckFileSystem.CheckMBR);
                    break;

                case KnownSevenZipFormat.PE:
                    this.StartSignature = new byte[] { 0x4d, 90 };
                    this.AsyncCheckContent = new AsyncCheckContentHandler(this.CheckStartContent);
                    break;

                case KnownSevenZipFormat.Rar:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(CheckRar.Check);
                    break;

                case KnownSevenZipFormat.Lzh:
                    this.AsyncCheckContent = new AsyncCheckContentHandler(CheckLzh.Check);
                    break;
            }
            this.FCapabilities |= this.CanCheckContent ? 1 : 0;
            switch (knownFormat)
            {
                case KnownSevenZipFormat.BZip2:
                case KnownSevenZipFormat.ELF:
                case KnownSevenZipFormat.GZip:
                case KnownSevenZipFormat.Lzma:
                case KnownSevenZipFormat.MachO:
                case KnownSevenZipFormat.MsLz:
                case KnownSevenZipFormat.Mub:
                case KnownSevenZipFormat.PE:
                case KnownSevenZipFormat.Split:
                case KnownSevenZipFormat.Xz:
                case KnownSevenZipFormat.Z:
                    break;

                default:
                    this.FCapabilities |= ArchiveFormatCapabilities.MultiFileArchive;
                    break;
            }
            switch (knownFormat)
            {
                case KnownSevenZipFormat.ELF:
                case KnownSevenZipFormat.FLV:
                case KnownSevenZipFormat.MachO:
                case KnownSevenZipFormat.PE:
                case KnownSevenZipFormat.Split:
                case KnownSevenZipFormat.SWF:
                case KnownSevenZipFormat.SWFc:
                    this.FSevenZipCapabilities |= SevenZipFormatCapabilities.Internal;
                    break;
            }
            if ((this.FCapabilities & ArchiveFormatCapabilities.CreateArchive) > 0)
            {
                switch (knownFormat)
                {
                    case KnownSevenZipFormat.SevenZip:
                    case KnownSevenZipFormat.Tar:
                    case KnownSevenZipFormat.Zip:
                        this.FCapabilities |= ArchiveFormatCapabilities.UpdateArchive;
                        break;
                }
                switch (knownFormat)
                {
                    case KnownSevenZipFormat.SevenZip:
                    case KnownSevenZipFormat.Zip:
                    {
                        this.FCapabilities |= ArchiveFormatCapabilities.EncryptContent;
                    }
                }
                switch (knownFormat)
                {
                    case KnownSevenZipFormat.SevenZip:
                        this.FSevenZipCapabilities = SevenZipFormatCapabilities.EncryptFileNames | SevenZipFormatCapabilities.SFX | SevenZipFormatCapabilities.MultiThread | SevenZipFormatCapabilities.Solid;
                        break;

                    case KnownSevenZipFormat.BZip2:
                    case KnownSevenZipFormat.Xz:
                        this.FSevenZipCapabilities = SevenZipFormatCapabilities.AppendExt | SevenZipFormatCapabilities.MultiThread;
                        break;

                    case KnownSevenZipFormat.GZip:
                        this.FSevenZipCapabilities = SevenZipFormatCapabilities.AppendExt;
                        break;

                    case KnownSevenZipFormat.Zip:
                        this.FSevenZipCapabilities = SevenZipFormatCapabilities.MultiThread;
                        break;
                }
                switch (knownFormat)
                {
                    case KnownSevenZipFormat.Xz:
                    case KnownSevenZipFormat.Zip:
                    case KnownSevenZipFormat.Tar:
                    case KnownSevenZipFormat.SevenZip:
                    case KnownSevenZipFormat.BZip2:
                    case KnownSevenZipFormat.GZip:
                        goto Label_04A5;
                }
                this.FCapabilities &= ~ArchiveFormatCapabilities.CreateArchive;
            }
        Label_04A5:
            base.BeginInit();
            try
            {
                base.ResetComponentSettings();
                base.LoadComponentSettings();
            }
            finally
            {
                base.EndInit();
            }
        }

        private static byte[] BstrToByte(IntPtr ptr)
        {
            int length = Marshal.ReadInt32(ptr, -4);
            byte[] destination = new byte[length];
            Marshal.Copy(ptr, destination, 0, length);
            return destination;
        }

        public int CheckContent(byte[] data, int dataLength)
        {
            if (this.CanCheckContent)
            {
                if (this.AsyncCheckContent != null)
                {
                    return this.AsyncCheckContent(data, dataLength);
                }
                return ByteArrayHelper.IndexOf(this.StartSignature, data, dataLength);
            }
            return -1;
        }

        private int CheckStartContent(byte[] data, int dataLength)
        {
            if (((this.CanCheckContent && (this.StartSignature.Length > 0)) && (dataLength > this.StartSignature.Length)) && ByteArrayHelper.Equals(data, this.StartSignature, this.StartSignature.Length))
            {
                return 0;
            }
            return -1;
        }

        internal IntPtr CreateInArchive()
        {
            return this.CreateObject(typeof(IInArchive).GUID);
        }

        private IntPtr CreateObject(Guid interfaceId)
        {
            CreateObjectDelegate delegateForFunctionPointer = (CreateObjectDelegate) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.Library, "CreateObject"), typeof(CreateObjectDelegate));
            if (delegateForFunctionPointer != null)
            {
                IntPtr ptr;
                delegateForFunctionPointer(ref this.FClassId, ref interfaceId, out ptr);
                return ptr;
            }
            return IntPtr.Zero;
        }

        public static IEnumerable<FindFormatResult> FindFormat(Stream stream)
        {
            return new <FindFormat>d__3(-2) { <>3__stream = stream };
        }

        public static Guid GetClassIdFromKnownFormat(KnownSevenZipFormat format)
        {
            foreach (KeyValuePair<Guid, KnownSevenZipFormat> pair in FormatClassMap)
            {
                if (((KnownSevenZipFormat) pair.Value) == format)
                {
                    return pair.Key;
                }
            }
            return Guid.Empty;
        }

        public Image GetFormatImage(Size desiredSize)
        {
            if (this.FormatImageIndex < -1)
            {
                this.FormatImageIndex = -1;
                foreach (string str in StringHelper.SplitString(WindowsWrapper.LoadString(this.Library, 100) ?? string.Empty, new char[] { ' ' }))
                {
                    int index = str.IndexOf(':');
                    if ((index > 0) && str.Substring(0, index).Equals(this.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!int.TryParse(str.Substring(index + 1), out this.FormatImageIndex))
                        {
                            this.FormatImageIndex = -1;
                        }
                        break;
                    }
                }
            }
            if (this.FormatImageIndex >= 0)
            {
                IntPtr hIcon = Windows.LoadImage(this.Library, (IntPtr) this.FormatImageIndex, IMAGE.IMAGE_ICON, desiredSize.Width, desiredSize.Height, LR.LR_DEFAULTCOLOR);
                if (hIcon != IntPtr.Zero)
                {
                    try
                    {
                        return ImageHelper.IconToBitmap(hIcon);
                    }
                    finally
                    {
                        Windows.DestroyIcon(hIcon);
                    }
                }
            }
            return null;
        }

        private void GetFormatProperty(ArchivePropId propId, ref PropVariant value)
        {
            value.Clear();
            if (this.FormatIndex < 0)
            {
                this.GetHandlerProperty(propId, ref value);
            }
            else
            {
                this.GetHandlerProperty2((uint) this.FormatIndex, propId, ref value);
            }
        }

        protected override void OnResetComponentSettings()
        {
            base.OnResetComponentSettings();
            base.Disabled = (this.SevenZipCapabilities & SevenZipFormatCapabilities.Internal) > 0;
            switch (this.KnownFormat)
            {
                case KnownSevenZipFormat.Chm:
                case KnownSevenZipFormat.Compound:
                case KnownSevenZipFormat.FLV:
                case KnownSevenZipFormat.Nsis:
                case KnownSevenZipFormat.PE:
                case KnownSevenZipFormat.SWF:
                case KnownSevenZipFormat.SWFc:
                    base.HideFormat = true;
                    break;

                default:
                    base.HideFormat = false;
                    break;
            }
            PropVariant variant = new PropVariant();
            try
            {
                this.GetFormatProperty(ArchivePropId.kExtension, ref variant);
                string str = (string) variant.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    base.Extension = str.Split(new char[] { ' ' });
                }
                this.GetFormatProperty(ArchivePropId.kAddExtension, ref variant);
                string str2 = (string) variant.Value;
                if (str2 != null)
                {
                    this.AddExtension = str2.Split(new char[] { ' ' });
                }
            }
            finally
            {
                variant.Clear();
            }
        }

        public override IEnumerable<ISimpleItem> OpenArchiveContent(Stream archiveStream, string archiveName)
        {
            IEnumerable<ISimpleItem> enumerable;
            if (archiveStream == null)
            {
                throw new ArgumentNullException("archiveStream");
            }
            if (archiveName == null)
            {
                throw new ArgumentNullException("archiveName");
            }
            if (archiveName == string.Empty)
            {
                throw new ArgumentException("archiveName is empty");
            }
            if (!archiveStream.CanSeek)
            {
                return null;
            }
            IntPtr inArchivePtr = this.CreateInArchive();
            try
            {
                enumerable = this.OpenArchiveContent(inArchivePtr, archiveStream, true, archiveName);
            }
            catch
            {
                Marshal.Release(inArchivePtr);
                throw;
            }
            return enumerable;
        }

        public IEnumerable<ISimpleItem> OpenArchiveContent(IntPtr InArchivePtr, Stream archiveStream, bool autoClose, string archiveName)
        {
            if (archiveStream == null)
            {
                throw new ArgumentNullException("archiveStream");
            }
            if (archiveName == null)
            {
                throw new ArgumentNullException("archiveName");
            }
            if (archiveName == string.Empty)
            {
                throw new ArgumentException("archiveName is empty");
            }
            if (archiveStream.CanSeek)
            {
                StreamWrapper wrapper;
                archiveStream.Position = 0L;
                if (!(!autoClose || RemotingServices.IsTransparentProxy(archiveStream)))
                {
                    wrapper = new InStreamTimedWrapper(archiveStream);
                }
                else
                {
                    wrapper = new InStreamWrapper(archiveStream);
                }
                SevenZipSharedArchiveContext context = new SevenZipSharedArchiveContext(InArchivePtr, this, wrapper);
                int errorCode = context.Open(null);
                if (errorCode == 0)
                {
                    return new SevenZipArchiveContent(context, archiveName);
                }
                context.Dispose();
                int num2 = errorCode;
                if (num2 == -2147467260)
                {
                    throw new AbortException();
                }
                if (num2 != 1)
                {
                    throw Marshal.GetExceptionForHR(errorCode);
                }
                if (context.ArchivePassword[0] != null)
                {
                    throw new UnauthorizedAccessException(string.Format(Resources.sErrorInvalidArchivePassword, archiveName));
                }
            }
            return null;
        }

        private static bool ProcessFormatsPath(string formatsPath)
        {
            bool flag = false;
            if (Directory.Exists(formatsPath))
            {
                foreach (string str in Directory.GetFiles(formatsPath, "*.dll", SearchOption.TopDirectoryOnly))
                {
                    flag |= RegisterFormatLib(str);
                }
            }
            return flag;
        }

        private static bool RegisterFormatLib(string formatLibPath)
        {
            Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule = Windows.LoadLibrary(formatLibPath);
            if (!hModule.IsInvalid)
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(formatLibPath);
                Version version = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart);
                if (Windows.GetProcAddress(hModule, "GetHandlerProperty2") != IntPtr.Zero)
                {
                    IntPtr procAddress = Windows.GetProcAddress(hModule, "GetNumberOfFormats");
                    if (procAddress != IntPtr.Zero)
                    {
                        uint num;
                        GetNumberOfFormatsDelegate delegateForFunctionPointer = (GetNumberOfFormatsDelegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(GetNumberOfFormatsDelegate));
                        if (delegateForFunctionPointer(out num) == 0)
                        {
                            for (int i = 0; i < num; i++)
                            {
                                FFormats.Add(new SevenZipFormatInfo(hModule, version, i));
                            }
                        }
                        return true;
                    }
                }
                if (Windows.GetProcAddress(hModule, "GetHandlerProperty") != IntPtr.Zero)
                {
                    FFormats.Add(new SevenZipFormatInfo(hModule, version, -1));
                    return true;
                }
                hModule.Close();
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.ToString(), this.Name);
        }

        public bool CanCheckContent
        {
            get
            {
                return (((this.StartSignature != null) && (this.StartSignature.Length > 0)) || (this.AsyncCheckContent != null));
            }
        }

        public override ArchiveFormatCapabilities Capabilities
        {
            get
            {
                return this.FCapabilities;
            }
        }

        public override Guid ClassId
        {
            get
            {
                return this.FClassId;
            }
        }

        private static Dictionary<Guid, KnownSevenZipFormat> FormatClassMap
        {
            get
            {
                if (FFormatClassMap == null)
                {
                    FFormatClassMap = new Dictionary<Guid, KnownSevenZipFormat>();
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110070000"), KnownSevenZipFormat.SevenZip);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110040000"), KnownSevenZipFormat.Arj);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110020000"), KnownSevenZipFormat.BZip2);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110080000"), KnownSevenZipFormat.Cab);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e90000"), KnownSevenZipFormat.Chm);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e50000"), KnownSevenZipFormat.Compound);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110ed0000"), KnownSevenZipFormat.Cpio);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110ec0000"), KnownSevenZipFormat.Deb);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e40000"), KnownSevenZipFormat.Dmg);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110de0000"), KnownSevenZipFormat.ELF);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110da0000"), KnownSevenZipFormat.FAT);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110d60000"), KnownSevenZipFormat.FLV);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110ef0000"), KnownSevenZipFormat.GZip);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e30000"), KnownSevenZipFormat.HFS);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e70000"), KnownSevenZipFormat.Iso);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110060000"), KnownSevenZipFormat.Lzh);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-0001100a0000"), KnownSevenZipFormat.Lzma);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-0001100b0000"), KnownSevenZipFormat.Lzma86);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110df0000"), KnownSevenZipFormat.MachO);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110db0000"), KnownSevenZipFormat.MBR);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110d50000"), KnownSevenZipFormat.MsLz);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e20000"), KnownSevenZipFormat.Mub);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110090000"), KnownSevenZipFormat.Nsis);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110d90000"), KnownSevenZipFormat.NTFS);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110dd0000"), KnownSevenZipFormat.PE);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110030000"), KnownSevenZipFormat.Rar);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110eb0000"), KnownSevenZipFormat.Rpm);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110ea0000"), KnownSevenZipFormat.Split);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110d70000"), KnownSevenZipFormat.SWF);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110d80000"), KnownSevenZipFormat.SWFc);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110ee0000"), KnownSevenZipFormat.Tar);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e00000"), KnownSevenZipFormat.Udf);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110dc0000"), KnownSevenZipFormat.VHD);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e60000"), KnownSevenZipFormat.Wim);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110e10000"), KnownSevenZipFormat.Xar);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-0001100c0000"), KnownSevenZipFormat.Xz);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110050000"), KnownSevenZipFormat.Z);
                    FFormatClassMap.Add(new Guid("23170f69-40c1-278a-1000-000110010000"), KnownSevenZipFormat.Zip);
                }
                return FFormatClassMap;
            }
        }

        public static ICollection<SevenZipFormatInfo> Formats
        {
            get
            {
                if (FFormats == null)
                {
                    FFormats = new List<SevenZipFormatInfo>();
                    if (!ProcessFormatsPath(Path.Combine(SettingsManager.SpecialFolders.Plugins, "Formats")))
                    {
                        string sevenZipInstallPath = SevenZipInstallPath;
                        if (!string.IsNullOrEmpty(sevenZipInstallPath))
                        {
                            ProcessFormatsPath(sevenZipInstallPath);
                        }
                    }
                }
                return FFormats;
            }
        }

        public Version FormatVersion
        {
            get
            {
                return this.LibraryVersion;
            }
        }

        public KnownSevenZipFormat KnownFormat
        {
            get
            {
                KnownSevenZipFormat format;
                if (FormatClassMap.TryGetValue(this.ClassId, out format))
                {
                    return format;
                }
                return KnownSevenZipFormat.Unknown;
            }
        }

        public override string Name
        {
            get
            {
                return this.FormatName;
            }
        }

        public SevenZipFormatCapabilities SevenZipCapabilities
        {
            get
            {
                return this.FSevenZipCapabilities;
            }
        }

        public static string SevenZipInstallPath
        {
            get
            {
                string str;
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\7-Zip"))
                    {
                        str = (key != null) ? (key.GetValue("Path") as string) : null;
                    }
                }
                catch (SecurityException)
                {
                    str = null;
                }
                return str;
            }
        }

        public EncryptionMethod[] SupportedEncryption
        {
            get
            {
                if ((this.Capabilities & ArchiveFormatCapabilities.EncryptContent) > 0)
                {
                    switch (this.KnownFormat)
                    {
                        case KnownSevenZipFormat.SevenZip:
                            return new EncryptionMethod[1];

                        case KnownSevenZipFormat.Zip:
                        {
                            List<EncryptionMethod> list = new List<EncryptionMethod>(2);
                            if ((this.LibraryVersion.Major > 4) || ((this.LibraryVersion.Major == 4) && (this.LibraryVersion.Minor >= 0x2b)))
                            {
                                list.Add(EncryptionMethod.AES256);
                            }
                            list.Add(EncryptionMethod.ZipCrypto);
                            return list.ToArray();
                        }
                    }
                }
                return new EncryptionMethod[0];
            }
        }

        public CompressionLevel[] SupportedLevels
        {
            get
            {
                if ((this.Capabilities & ArchiveFormatCapabilities.CreateArchive) > 0)
                {
                    switch (this.KnownFormat)
                    {
                        case KnownSevenZipFormat.Xz:
                        case KnownSevenZipFormat.BZip2:
                            return new CompressionLevel[] { CompressionLevel.Fastest, CompressionLevel.Fast, CompressionLevel.Normal, CompressionLevel.Maximum, CompressionLevel.Ultra };

                        case KnownSevenZipFormat.Zip:
                        case KnownSevenZipFormat.SevenZip:
                        {
                            CompressionLevel[] levelArray2 = new CompressionLevel[6];
                            levelArray2[1] = CompressionLevel.Fastest;
                            levelArray2[2] = CompressionLevel.Fast;
                            levelArray2[3] = CompressionLevel.Normal;
                            levelArray2[4] = CompressionLevel.Maximum;
                            levelArray2[5] = CompressionLevel.Ultra;
                            return levelArray2;
                        }
                        case KnownSevenZipFormat.Tar:
                            return new CompressionLevel[1];

                        case KnownSevenZipFormat.GZip:
                            return new CompressionLevel[] { CompressionLevel.Fastest, CompressionLevel.Normal, CompressionLevel.Maximum, CompressionLevel.Ultra };
                    }
                }
                return new CompressionLevel[0];
            }
        }

        public CompressionMethod[] SupportedMethods
        {
            get
            {
                if ((this.Capabilities & ArchiveFormatCapabilities.CreateArchive) > 0)
                {
                    List<CompressionMethod> list;
                    switch (this.KnownFormat)
                    {
                        case KnownSevenZipFormat.Xz:
                            return new CompressionMethod[] { CompressionMethod.LZMA2 };

                        case KnownSevenZipFormat.Zip:
                            list = new List<CompressionMethod>(5) {
                                CompressionMethod.Deflate,
                                CompressionMethod.Deflate64,
                                CompressionMethod.BZip2
                            };
                            if ((this.LibraryVersion.Major > 4) || ((this.LibraryVersion.Major == 4) && (this.LibraryVersion.Minor >= 0x3d)))
                            {
                                list.Add(CompressionMethod.LZMA);
                            }
                            if ((this.LibraryVersion.Major > 9) || ((this.LibraryVersion.Major == 9) && (this.LibraryVersion.Minor >= 11)))
                            {
                                list.Add(CompressionMethod.PPMd);
                            }
                            return list.ToArray();

                        case KnownSevenZipFormat.Tar:
                            return new CompressionMethod[1];

                        case KnownSevenZipFormat.SevenZip:
                            list = new List<CompressionMethod>(4) {
                                CompressionMethod.LZMA
                            };
                            if (this.LibraryVersion.Major >= 9)
                            {
                                list.Add(CompressionMethod.LZMA2);
                            }
                            list.Add(CompressionMethod.PPMd);
                            list.Add(CompressionMethod.BZip2);
                            return list.ToArray();

                        case KnownSevenZipFormat.BZip2:
                            return new CompressionMethod[] { CompressionMethod.BZip2 };

                        case KnownSevenZipFormat.GZip:
                            return new CompressionMethod[] { CompressionMethod.Deflate };
                    }
                }
                return new CompressionMethod[0];
            }
        }

        [CompilerGenerated]
        private sealed class <FindFormat>d__3 : IEnumerable<FindFormatResult>, System.Collections.IEnumerable, IEnumerator<FindFormatResult>, System.Collections.IEnumerator, IDisposable
        {
            private int <>1__state;
            private FindFormatResult <>2__current;
            public Stream <>3__stream;
            public FindFormatResult[] <>7__wrap6;
            public int <>7__wrap7;
            private int <>l__initialThreadId;
            public FindFormatResult <NextResult>5__4;
            public SevenZipFormatInfo.<>c__DisplayClass1 CS$<>8__locals2;
            public Stream stream;

            [DebuggerHidden]
            public <FindFormat>d__3(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.CS$<>8__locals2 = new SevenZipFormatInfo.<>c__DisplayClass1();
                            this.CS$<>8__locals2.Buffer = new byte[0x20000];
                            this.CS$<>8__locals2.Readed = this.stream.Read(this.CS$<>8__locals2.Buffer, 0, this.CS$<>8__locals2.Buffer.Length);
                            this.CS$<>8__locals2.Result = new FindFormatResult[SevenZipFormatInfo.Formats.Count];
                            Parallel.For(0, SevenZipFormatInfo.FFormats.Count - 1, new Action<int>(this.CS$<>8__locals2.<FindFormat>b__0));
                            this.<>1__state = 1;
                            this.<>7__wrap6 = this.CS$<>8__locals2.Result;
                            this.<>7__wrap7 = 0;
                            while (this.<>7__wrap7 < this.<>7__wrap6.Length)
                            {
                                this.<NextResult>5__4 = this.<>7__wrap6[this.<>7__wrap7];
                                if (this.<NextResult>5__4 == null)
                                {
                                    goto Label_011C;
                                }
                                this.<>2__current = this.<NextResult>5__4;
                                this.<>1__state = 2;
                                return true;
                            Label_0115:
                                this.<>1__state = 1;
                            Label_011C:
                                this.<>7__wrap7++;
                            }
                            this.<>m__Finally5();
                            break;

                        case 2:
                            goto Label_0115;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<FindFormatResult> IEnumerable<FindFormatResult>.GetEnumerator()
            {
                SevenZipFormatInfo.<FindFormat>d__3 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new SevenZipFormatInfo.<FindFormat>d__3(0);
                }
                d__.stream = this.<>3__stream;
                return d__;
            }

            [DebuggerHidden]
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Archive.Common.FindFormatResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void System.Collections.IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        this.<>m__Finally5();
                        break;
                }
            }

            FindFormatResult IEnumerator<FindFormatResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        private delegate int AsyncCheckContentHandler(byte[] array, int arrayLength);
    }
}

