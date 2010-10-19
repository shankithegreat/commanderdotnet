namespace Nomad.FileSystem.Archive.Wcx
{
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;
    using Nomad;
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class WcxFormatInfo : PersistArchiveFormatInfo, IDisposable
    {
        private CanYouHandleThisFileHandler FCanYouHandleThisFile;
        private Guid? FClassId;
        private CloseArchiveHandler FCloseArchive;
        private ConfigurePackerHandler FConfigurePacker;
        private static string FDefaultIniName;
        private DeleteFilesHandler FDeleteFiles;
        private static Dictionary<string, WcxFormatInfo> FFormats;
        private string FName;
        private OpenArchiveHandler FOpenArchive;
        private PackFilesHandler FPackFiles;
        private bool FUsePipes;
        private Microsoft.Win32.SafeHandles.SafeLibraryHandle Library;
        private int LockCount;
        private bool LockPack;
        public readonly PK_CAPS WcxCapabilities;

        private WcxFormatInfo(Microsoft.Win32.SafeHandles.SafeLibraryHandle library, string name)
        {
            this.Library = library;
            IntPtr procAddress = Windows.GetProcAddress(this.Library, "PackSetDefaultParams");
            if (procAddress != IntPtr.Zero)
            {
                PackSetDefaultParamsHandler delegateForFunctionPointer = (PackSetDefaultParamsHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(PackSetDefaultParamsHandler));
                if (delegateForFunctionPointer != null)
                {
                    PackDefaultParamStruct struct2;
                    struct2 = new PackDefaultParamStruct {
                        size = Marshal.SizeOf(struct2),
                        PluginInterfaceVersionHi = 2,
                        PluginInterfaceVersionLow = 10,
                        DefaultIniName = DefaultIniName
                    };
                    delegateForFunctionPointer(ref struct2);
                }
            }
            this.FOpenArchive = (OpenArchiveHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.Library, "OpenArchive"), typeof(OpenArchiveHandler));
            this.FCloseArchive = (CloseArchiveHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.Library, "CloseArchive"), typeof(CloseArchiveHandler));
            this.ReadHeader = (ReadHeaderHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.Library, "ReadHeader"), typeof(ReadHeaderHandler));
            this.ProcessFile = (ProcessFileHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(this.Library, "ProcessFile"), typeof(ProcessFileHandler));
            IntPtr ptr = Windows.GetProcAddress(library, "ReadHeaderEx");
            if (ptr != IntPtr.Zero)
            {
                this.ReadHeaderEx = (ReadHeaderHandlerEx) Marshal.GetDelegateForFunctionPointer(ptr, typeof(ReadHeaderHandlerEx));
            }
            IntPtr ptr3 = Windows.GetProcAddress(library, "GetPackerCaps");
            if (ptr3 != IntPtr.Zero)
            {
                GetPackerCapsHandler handler2 = (GetPackerCapsHandler) Marshal.GetDelegateForFunctionPointer(ptr3, typeof(GetPackerCapsHandler));
                this.WcxCapabilities = handler2();
            }
            if ((this.WcxCapabilities & PK_CAPS.PK_CAPS_BY_CONTENT) > 0)
            {
                this.FCanYouHandleThisFile = (CanYouHandleThisFileHandler) Marshal.GetDelegateForFunctionPointer(Windows.GetProcAddress(library, "CanYouHandleThisFile"), typeof(CanYouHandleThisFileHandler));
            }
            IntPtr ptr4 = Windows.GetProcAddress(this.Library, "SetProcessDataProc");
            if (ptr4 != IntPtr.Zero)
            {
                this.SetProcessData = (SetProcessDataProcHandler) Marshal.GetDelegateForFunctionPointer(ptr4, typeof(SetProcessDataProcHandler));
            }
            IntPtr ptr5 = Windows.GetProcAddress(this.Library, "DeleteFiles");
            if (ptr5 != IntPtr.Zero)
            {
                this.FDeleteFiles = (DeleteFilesHandler) Marshal.GetDelegateForFunctionPointer(ptr5, typeof(DeleteFilesHandler));
            }
            IntPtr ptr6 = Windows.GetProcAddress(this.Library, "PackFiles");
            if (ptr6 != IntPtr.Zero)
            {
                this.FPackFiles = (PackFilesHandler) Marshal.GetDelegateForFunctionPointer(ptr6, typeof(PackFilesHandler));
            }
            this.FName = name;
            base.LoadComponentSettings();
        }

        public int CloseArchive(IntPtr hArcData)
        {
            if (this.FCloseArchive == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            int num = this.FCloseArchive(hArcData);
            if (num == 0)
            {
                this.LockCount--;
            }
            return num;
        }

        public int DeleteFiles(string packedFile, IEnumerable<string> files)
        {
            int num;
            if (this.Library == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            if (this.FDeleteFiles == null)
            {
                throw new NotSupportedException();
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in files)
            {
                builder.Append(str);
                builder.Append('\0');
            }
            this.LockCount++;
            try
            {
                num = this.FDeleteFiles(packedFile, builder.ToString());
            }
            finally
            {
                this.LockCount--;
            }
            return num;
        }

        public void Dispose()
        {
            if (this.Locked)
            {
                throw new InvalidOperationException("Cannot dispose archive format because it is in use.");
            }
            this.Library.Close();
            this.Library = null;
            this.FOpenArchive = null;
            this.FCloseArchive = null;
            this.ReadHeader = null;
            this.ReadHeaderEx = null;
            this.ProcessFile = null;
            this.FCanYouHandleThisFile = null;
            this.SetProcessData = null;
            this.FConfigurePacker = null;
            this.FDeleteFiles = null;
            if (FFormats != null)
            {
                FFormats.Remove(this.Name);
            }
        }

        public static IEnumerable<FindFormatResult> FindFormat(Stream stream)
        {
            return new <FindFormat>d__0(-2) { <>3__stream = stream };
        }

        public static WcxFormatInfo GetFormat(string name)
        {
            WcxFormatInfo info;
            if ((FFormats != null) && FFormats.TryGetValue(name, out info))
            {
                return info;
            }
            return null;
        }

        protected override void OnLoadComponentSettings(ArchiveFormatSettings settings)
        {
            base.OnLoadComponentSettings(settings);
            this.UsePipes = settings.UsePipes;
        }

        protected override void OnResetComponentSettings()
        {
            base.OnResetComponentSettings();
            base.HideFormat = (this.WcxCapabilities & PK_CAPS.PK_CAPS_HIDE) > 0;
            this.UsePipes = false;
            base.Extension = null;
        }

        protected override void OnSaveComponentSettings(ArchiveFormatSettings settings)
        {
            base.OnSaveComponentSettings(settings);
            settings.UsePipes = this.UsePipes;
        }

        public IntPtr OpenArchive(ref OpenArchiveData ArchiveData)
        {
            IntPtr zero;
            if (this.FOpenArchive == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            try
            {
                zero = this.FOpenArchive(ref ArchiveData);
            }
            catch
            {
                ArchiveData.CmtState = -1;
                zero = IntPtr.Zero;
            }
            if (zero != IntPtr.Zero)
            {
                this.LockCount++;
            }
            return zero;
        }

        public override IEnumerable<ISimpleItem> OpenArchiveContent(Stream archiveStream, string archiveName)
        {
            if (this.FOpenArchive == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            IEnumerable<ISimpleItem> enumerable = null;
            FileStream stream = archiveStream as FileStream;
            if (stream != null)
            {
                enumerable = this.OpenArchiveContent(stream.Name, archiveName);
            }
            if (enumerable != null)
            {
                archiveStream.Close();
            }
            return enumerable;
        }

        public IEnumerable<ISimpleItem> OpenArchiveContent(string archiveFileName, string archiveName)
        {
            if (this.FOpenArchive == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            OpenArchiveData archiveData = new OpenArchiveData {
                ArcName = archiveFileName,
                OpenMode = PK_OM.PK_OM_LIST
            };
            IntPtr hArcData = this.OpenArchive(ref archiveData);
            if (hArcData != IntPtr.Zero)
            {
                this.CloseArchive(hArcData);
                return new WcxArchiveContent(new WcxArchiveContext(archiveFileName, this));
            }
            if (archiveData.OpenResult != 0)
            {
                WcxErrors.ThrowExceptionForError(archiveData.OpenResult);
            }
            return null;
        }

        public int PackFiles(string packedFile, string subFolder, string srcPath, IEnumerable<string> files, ProcessDataProcCallback callback)
        {
            int num;
            if (this.Library == null)
            {
                throw new ObjectDisposedException("WcxFormatInfo");
            }
            if (this.FPackFiles == null)
            {
                throw new NotSupportedException();
            }
            if (this.LockPack)
            {
                throw new InvalidOperationException(string.Format("Another pack operation with '{0}' pack plug-in already in progress. Only one packing process per wcx plug-in allowed.", this.Name));
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in files)
            {
                builder.Append(str);
                builder.Append('\0');
            }
            this.LockPack = true;
            this.LockCount++;
            try
            {
                if (callback != null)
                {
                    this.SetProcessData(IntPtr.Zero, callback);
                }
                num = this.FPackFiles(packedFile, subFolder, srcPath, builder.ToString(), PK_PACK.PK_PACK_SAVE_PATHS);
            }
            finally
            {
                this.SetProcessData(IntPtr.Zero, null);
                this.LockCount--;
                this.LockPack = false;
            }
            return num;
        }

        public static WcxFormatInfo RegisterFormat(string formatPath)
        {
            return RegisterFormat(formatPath, true);
        }

        private static WcxFormatInfo RegisterFormat(string formatPath, bool checkExists)
        {
            WcxFormatInfo info;
            string fileName = Path.GetFileName(formatPath);
            if (checkExists && FFormats.TryGetValue(fileName, out info))
            {
                throw new InvalidOperationException(string.Format("Archive format '{0}' already registered", fileName));
            }
            Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule = Windows.LoadLibrary(formatPath);
            if (hModule.IsInvalid)
            {
                return null;
            }
            if (Windows.GetProcAddress(hModule, "OpenArchive") == IntPtr.Zero)
            {
                hModule.Close();
                return null;
            }
            info = new WcxFormatInfo(hModule, fileName);
            FFormats.Add(fileName, info);
            return info;
        }

        public void ShowConfigurePackerDialog(IWin32Window owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException();
            }
            if (this.FConfigurePacker == null)
            {
                if (this.Library == null)
                {
                    throw new ObjectDisposedException("WcxFormatInfo");
                }
                if ((this.WcxCapabilities & PK_CAPS.PK_CAPS_OPTIONS) == 0)
                {
                    throw new NotSupportedException();
                }
                IntPtr procAddress = Windows.GetProcAddress(this.Library, "ConfigurePacker");
                if (procAddress == IntPtr.Zero)
                {
                    throw new NotSupportedException();
                }
                this.FConfigurePacker = (ConfigurePackerHandler) Marshal.GetDelegateForFunctionPointer(procAddress, typeof(ConfigurePackerHandler));
                if (this.FConfigurePacker == null)
                {
                    throw new NotSupportedException();
                }
            }
            this.LockCount++;
            try
            {
                this.FConfigurePacker(owner.Handle, this.Library.DangerousGetHandle());
            }
            finally
            {
                this.LockCount--;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", base.ToString(), this.Name);
        }

        public bool CanDeleteFiles
        {
            get
            {
                return (this.FDeleteFiles != null);
            }
        }

        public override ArchiveFormatCapabilities Capabilities
        {
            get
            {
                return ((((((this.WcxCapabilities & PK_CAPS.PK_CAPS_NEW) > 0) ? ArchiveFormatCapabilities.CreateArchive : ((ArchiveFormatCapabilities) 0)) | (((this.WcxCapabilities & PK_CAPS.PK_CAPS_MODIFY) > 0) ? ArchiveFormatCapabilities.UpdateArchive : ((ArchiveFormatCapabilities) 0))) | (((this.WcxCapabilities & PK_CAPS.PK_CAPS_MULTIPLE) > 0) ? ArchiveFormatCapabilities.MultiFileArchive : ((ArchiveFormatCapabilities) 0))) | (((this.WcxCapabilities & PK_CAPS.PK_CAPS_BY_CONTENT) > 0) ? ArchiveFormatCapabilities.DetectFormatByContent : ((ArchiveFormatCapabilities) 0)));
            }
        }

        public override Guid ClassId
        {
            get
            {
                if (!this.FClassId.HasValue)
                {
                    using (MACTripleDES edes = new MACTripleDES())
                    {
                        byte[] d = edes.ComputeHash(Encoding.UTF8.GetBytes(this.FName.ToUpper()));
                        this.FClassId = new Guid(0x1e946125, 0x1b8f, 0x4fd6, d);
                    }
                }
                return this.FClassId.Value;
            }
        }

        private static string DefaultIniName
        {
            get
            {
                if (FDefaultIniName == null)
                {
                    string fDefaultIniName = FDefaultIniName;
                }
                return (FDefaultIniName = Path.Combine(SettingsManager.SpecialFolders.UserConfig, "wcx.ini"));
            }
        }

        public static ICollection<WcxFormatInfo> Formats
        {
            get
            {
                if (FFormats == null)
                {
                    FFormats = new Dictionary<string, WcxFormatInfo>(StringComparer.OrdinalIgnoreCase);
                    if (Directory.Exists(WcxPluginsPath))
                    {
                        foreach (string str in Directory.GetFiles(WcxPluginsPath, "*.wcx", SearchOption.AllDirectories))
                        {
                            RegisterFormat(str, false);
                        }
                    }
                }
                return FFormats.Values;
            }
        }

        public bool Locked
        {
            get
            {
                return (this.LockCount > 0);
            }
        }

        public override string Name
        {
            get
            {
                return this.FName;
            }
        }

        internal ProcessFileHandler ProcessFile { get; private set; }

        internal ReadHeaderHandler ReadHeader { get; private set; }

        internal ReadHeaderHandlerEx ReadHeaderEx { get; private set; }

        internal SetProcessDataProcHandler SetProcessData { get; private set; }

        public bool UsePipes
        {
            get
            {
                return this.FUsePipes;
            }
            set
            {
                if (this.FUsePipes != value)
                {
                    this.FUsePipes = value;
                    this.Changed();
                }
            }
        }

        public static string WcxPluginsPath
        {
            get
            {
                return Path.Combine(SettingsManager.SpecialFolders.Plugins, "WCX");
            }
        }

        [CompilerGenerated]
        private sealed class <FindFormat>d__0 : IEnumerable<FindFormatResult>, IEnumerable, IEnumerator<FindFormatResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private FindFormatResult <>2__current;
            public Stream <>3__stream;
            public IEnumerator<WcxFormatInfo> <>7__wrap4;
            private int <>l__initialThreadId;
            public string <fileName>5__2;
            public FileStream <LocalStream>5__1;
            public WcxFormatInfo <NextFormatInfo>5__3;
            public Stream stream;

            [DebuggerHidden]
            public <FindFormat>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            break;

                        case 2:
                            goto Label_00DB;

                        default:
                            goto Label_00F9;
                    }
                    this.<>1__state = -1;
                    this.<LocalStream>5__1 = this.stream as FileStream;
                    if (this.<LocalStream>5__1 != null)
                    {
                        this.<fileName>5__2 = this.<LocalStream>5__1.Name;
                        this.<>7__wrap4 = WcxFormatInfo.Formats.GetEnumerator();
                        this.<>1__state = 1;
                        while (this.<>7__wrap4.MoveNext())
                        {
                            this.<NextFormatInfo>5__3 = this.<>7__wrap4.Current;
                            if ((this.<NextFormatInfo>5__3.FCanYouHandleThisFile == null) || !this.<NextFormatInfo>5__3.FCanYouHandleThisFile(this.<fileName>5__2))
                            {
                                continue;
                            }
                            this.<>2__current = new FindFormatResult(this.<NextFormatInfo>5__3, FindFormatSource.Content);
                            this.<>1__state = 2;
                            return true;
                        Label_00DB:
                            this.<>1__state = 1;
                        }
                        this.<>m__Finally5();
                    }
                Label_00F9:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<FindFormatResult> IEnumerable<FindFormatResult>.GetEnumerator()
            {
                WcxFormatInfo.<FindFormat>d__0 d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = this;
                }
                else
                {
                    d__ = new WcxFormatInfo.<FindFormat>d__0(0);
                }
                d__.stream = this.<>3__stream;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.FileSystem.Archive.Common.FindFormatResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
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

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

