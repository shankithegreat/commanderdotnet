namespace Nomad.Workers
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Archive.Wcx;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Special;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Threading;

    public class PackWorker : EventBackgroundWorker, IArchiveUpdateCallback, ICryptoGetTextPassword2
    {
        public const string ActionFileExistsCreate = "FileExistsCreate";
        public const string ActionFileExistsNotArchive = "FileExistsNotArchive";
        public const string ActionFileExistsNoTemp = "FileExistsNoTemp";
        public readonly ArchiveFormatInfo ArchiveFormat;
        private string CurrentProcessFile;
        private Stream CurrentSourceStream;
        public readonly IChangeVirtualFile DestFile;
        private IVirtualFolder FContent;
        private IVirtualItemFilter FFilter;
        private string FPassword;
        private int FProcessedCount;
        private SevenZipPropertiesBuilder FProperties;
        private object FSnapshotLock;
        private PackStage FStage;
        private int FStoredProgress;
        private string FSubFolder;
        private int FTotalCount;
        private ProcessedSize FTotalProcessed;
        private ArchiveUpdateMode FUpdateMode;
        public EventHandler<VirtualItemEventArgs> OnBeforePackItem;
        public EventHandler<ConfirmArchiveActionArgs> OnConfirmAction;
        public EventHandler<ChangeItemErrorEventArgs> OnItemError;
        public EventHandler<CancelVirtualItemEventArgs> OnRejectItem;
        private Stream OutStream;
        private List<PackItemInfo> SourceFiles;
        private Dictionary<string, IVirtualItem> SourceMap;
        private bool UseTimedWrapper;

        private PackWorker(ArchiveFormatInfo format, IChangeVirtualFile dest, string archiveSubFolder, IEnumerable<IVirtualItem> items, IVirtualItemFilter filter, ArchiveUpdateMode updateMode)
        {
            this.FSnapshotLock = new object();
            this.FTotalProcessed = new ProcessedSize();
            this.FStoredProgress = -1;
            if (format == null)
            {
                throw new ArgumentNullException("info");
            }
            if (dest == null)
            {
                throw new ArgumentNullException("dest");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            this.ArchiveFormat = format;
            this.DestFile = dest;
            this.FSubFolder = string.IsNullOrEmpty(archiveSubFolder) ? string.Empty : archiveSubFolder;
            this.FContent = new AggregatedVirtualFolder(items);
            this.FFilter = filter;
            this.FUpdateMode = updateMode;
        }

        public PackWorker(WcxFormatInfo format, IChangeVirtualFile dest, string archiveSubFolder, IEnumerable<IVirtualItem> items, IVirtualItemFilter filter, ArchiveUpdateMode updateMode) : this((ArchiveFormatInfo) format, dest, archiveSubFolder, items, filter, updateMode)
        {
        }

        public PackWorker(SevenZipFormatInfo format, IChangeVirtualFile dest, string archiveSubFolder, IEnumerable<IVirtualItem> items, IVirtualItemFilter filter, ArchiveUpdateMode updateMode, SevenZipPropertiesBuilder properties, string password) : this(format, dest, archiveSubFolder, items, filter, updateMode)
        {
            this.FProperties = properties;
            this.FPassword = password;
            this.UseTimedWrapper = ((format.KnownFormat == KnownSevenZipFormat.Zip) && (properties != null)) && properties.MultiThread;
        }

        private void BeforePackItem(IVirtualItem item)
        {
            if (this.OnBeforePackItem != null)
            {
                this.OnBeforePackItem(this, new VirtualItemEventArgs(item));
            }
        }

        private ChangeItemAction ChangeItemError(IVirtualItem item, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            if (this.OnItemError != null)
            {
                ChangeItemErrorEventArgs e = new ChangeItemErrorEventArgs(item, available, error);
                this.OnItemError(this, e);
                none = e.Action;
            }
            return none;
        }

        private bool ConfirmAction(string tag)
        {
            if (this.OnConfirmAction != null)
            {
                ConfirmArchiveActionArgs e = new ConfirmArchiveActionArgs(tag, this.ArchiveFormat, this.DestFile);
                this.OnConfirmAction(this, e);
                return !e.Cancel;
            }
            return true;
        }

        private bool CopyChangeFile(IChangeVirtualFile source, IChangeVirtualFile dest)
        {
            bool flag;
            using (Stream stream = source.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan, 0L))
            {
                using (Stream stream2 = dest.Open(FileMode.Create, FileAccess.Write, FileShare.Read, FileOptions.SequentialScan, 0L))
                {
                    flag = this.CopyStream(stream, stream2);
                }
            }
            return flag;
        }

        private bool CopyStream(Stream sourceStream, Stream destStream)
        {
            byte[] buffer = new byte[0xe000];
            int count = 0;
            do
            {
                base.CheckSuspendingPending();
                if (base.CancellationPending)
                {
                    return false;
                }
                count = sourceStream.Read(buffer, 0, buffer.Length);
                destStream.Write(buffer, 0, count);
            }
            while (count > 0);
            return true;
        }

        public void CryptoGetTextPassword2(out bool passwordIsDefined, out string password)
        {
            passwordIsDefined = !string.IsNullOrEmpty(this.FPassword);
            password = this.FPassword;
        }

        protected void DoSevenZipPack()
        {
            Func<Stream> getStream = null;
            Func<Stream> func2 = null;
            IChangeVirtualFile TempDestFile;
            this.FStage = PackStage.CalculatingSize;
            this.SourceFiles = new List<PackItemInfo>(this.GetFolderContent<IChangeVirtualFile>(this.FContent, this.FSubFolder, true));
            if (!base.CancellationPending)
            {
                SevenZipFormatInfo archiveFormat = (SevenZipFormatInfo) this.ArchiveFormat;
                Stream archiveStream = null;
                TempDestFile = this.DestFile;
                IntPtr pUnk = archiveFormat.CreateInArchive();
                try
                {
                    IPersistVirtualItem destFile = this.DestFile as IPersistVirtualItem;
                    if ((destFile != null) && destFile.Exists)
                    {
                        if (this.FUpdateMode == ArchiveUpdateMode.CreateNew)
                        {
                            if (!this.ConfirmAction("FileExistsCreate"))
                            {
                                return;
                            }
                        }
                        else
                        {
                            this.FStage = PackStage.ReadingExistingArchive;
                            if (getStream == null)
                            {
                                getStream = delegate {
                                    return this.DestFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.RandomAccess, 0L);
                                };
                            }
                            archiveStream = this.OpenItemStream(this.DestFile, getStream);
                            if (archiveStream == null)
                            {
                                return;
                            }
                            Marshal.AddRef(pUnk);
                            IEnumerable<ISimpleItem> enumerable = archiveFormat.OpenArchiveContent(pUnk, archiveStream, false, this.DestFile.FullName);
                            if (enumerable != null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                                Dictionary<string, PackItemInfo> dictionary2 = new Dictionary<string, PackItemInfo>(StringComparer.OrdinalIgnoreCase);
                                for (int i = 0; i < this.SourceFiles.Count; i++)
                                {
                                    PackItemInfo info2 = this.SourceFiles[i];
                                    dictionary.Add(info2.RelativePath, i);
                                    dictionary2.Add(info2.RelativePath, info2);
                                }
                                foreach (SevenZipArchiveItem item2 in enumerable)
                                {
                                    PackItemInfo info3;
                                    if (dictionary2.TryGetValue(item2.Name, out info3))
                                    {
                                        if (this.FUpdateMode == ArchiveUpdateMode.SkipAll)
                                        {
                                            this.SourceFiles[dictionary[item2.Name]] = new PackItemInfo(item2.Index, item2.Name);
                                        }
                                    }
                                    else
                                    {
                                        this.SourceFiles.Add(new PackItemInfo(item2.Index, item2.Name));
                                    }
                                }
                            }
                            else
                            {
                                archiveStream.Close();
                                if (!this.ConfirmAction("FileExistsNotArchive"))
                                {
                                    return;
                                }
                                this.FUpdateMode = ArchiveUpdateMode.CreateNew;
                            }
                        }
                        ICreateVirtualFile parent = this.DestFile.Parent as ICreateVirtualFile;
                        if (parent != null)
                        {
                            TempDestFile = parent.CreateFile(this.DestFile.Name + ".tmp");
                        }
                        else if (!this.ConfirmAction("FileExistsNoTemp"))
                        {
                            return;
                        }
                    }
                    this.FStage = (this.FUpdateMode == ArchiveUpdateMode.CreateNew) ? PackStage.PackingNewItems : PackStage.MovingExistingItems;
                    IOutArchive typedObjectForIUnknown = (IOutArchive) Marshal.GetTypedObjectForIUnknown(pUnk, typeof(IOutArchive));
                    try
                    {
                        if (this.FProperties != null)
                        {
                            ISetProperties setProperties = typedObjectForIUnknown as ISetProperties;
                            if (setProperties != null)
                            {
                                this.FProperties.Apply(setProperties);
                            }
                        }
                        if (func2 == null)
                        {
                            func2 = delegate {
                                return TempDestFile.Open(FileMode.Create, FileAccess.Write, FileShare.Read, FileOptions.SequentialScan, 0L);
                            };
                        }
                        this.OutStream = this.OpenItemStream(TempDestFile, func2);
                        if (this.OutStream != null)
                        {
                            using (this.OutStream)
                            {
                                typedObjectForIUnknown.UpdateItems(new OutStreamWrapper(this.OutStream), this.SourceFiles.Count, this);
                            }
                            this.FStage = PackStage.Finished;
                            if (TempDestFile != this.DestFile)
                            {
                                if (archiveStream != null)
                                {
                                    archiveStream.Close();
                                }
                                this.FStage = PackStage.Relocating;
                                ((IChangeVirtualItem) this.DestFile).Delete(false);
                                ((IChangeVirtualItem) TempDestFile).Name = this.DestFile.Name;
                            }
                        }
                    }
                    catch (COMException exception)
                    {
                        ((IChangeVirtualItem) TempDestFile).Delete(false);
                        if (this.CurrentSourceStream != null)
                        {
                            this.CurrentSourceStream.Close();
                        }
                        switch (exception.ErrorCode)
                        {
                            case -2147467260:
                            case -2147467259:
                                return;
                        }
                        throw;
                    }
                    finally
                    {
                        Marshal.FinalReleaseComObject(typedObjectForIUnknown);
                    }
                }
                finally
                {
                    while (Marshal.Release(pUnk) > 0)
                    {
                    }
                    if (archiveStream != null)
                    {
                        archiveStream.Close();
                    }
                }
            }
        }

        private void DoWcxPack()
        {
            this.FStage = PackStage.CalculatingSize;
            IVirtualFolder parent = null;
            foreach (IVirtualItem item in this.FContent.GetContent())
            {
                if (parent == null)
                {
                    parent = item.Parent;
                }
                else if (!parent.Equals(item.Parent))
                {
                    throw new ApplicationException(Resources.sErrorWcxDifferentParent);
                }
            }
            this.SourceFiles = new List<PackItemInfo>(this.GetFolderContent<CustomFileSystemFile>(this.FContent, string.Empty, false));
            if (!base.CancellationPending)
            {
                WcxFormatInfo archiveFormat = (WcxFormatInfo) this.ArchiveFormat;
                IPersistVirtualItem destFile = this.DestFile as IPersistVirtualItem;
                bool flag = (destFile != null) && destFile.Exists;
                if (flag)
                {
                    if (this.FUpdateMode == ArchiveUpdateMode.CreateNew)
                    {
                        if (!this.ConfirmAction("FileExistsCreate"))
                        {
                            return;
                        }
                    }
                    else
                    {
                        this.FStage = PackStage.ReadingExistingArchive;
                        IEnumerable<ISimpleItem> enumerable = archiveFormat.OpenArchiveContent(this.DestFile.FullName, Path.GetFileName(this.DestFile.FullName));
                        if (enumerable != null)
                        {
                            Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                            Dictionary<string, PackItemInfo> dictionary2 = new Dictionary<string, PackItemInfo>(StringComparer.OrdinalIgnoreCase);
                            for (int i = 0; i < this.SourceFiles.Count; i++)
                            {
                                PackItemInfo info2 = this.SourceFiles[i];
                                dictionary.Add(info2.RelativePath, i);
                                dictionary2.Add(info2.RelativePath, info2);
                            }
                            foreach (WcxArchiveItem item3 in enumerable)
                            {
                                PackItemInfo info3;
                                if (dictionary2.TryGetValue(item3.Name, out info3) && (this.FUpdateMode == ArchiveUpdateMode.SkipAll))
                                {
                                    this.SourceFiles[dictionary[item3.Name]] = null;
                                }
                            }
                        }
                        else
                        {
                            if (!this.ConfirmAction("FileExistsNotArchive"))
                            {
                                return;
                            }
                            this.FUpdateMode = ArchiveUpdateMode.CreateNew;
                        }
                    }
                }
                this.SourceMap = new Dictionary<string, IVirtualItem>(StringComparer.OrdinalIgnoreCase);
                List<string> files = new List<string>(this.SourceFiles.Count);
                foreach (PackItemInfo info4 in this.SourceFiles)
                {
                    if (info4 != null)
                    {
                        files.Add(info4.RelativePath);
                        this.SourceMap.Add(info4.Item.FullName, info4.Item);
                        this.FTotalProcessed.AddTotalSize(Convert.ToInt64(info4.Item[3]));
                    }
                }
                if (!base.CancellationPending)
                {
                    IChangeVirtualFile dest = null;
                    if (!(this.DestFile is CustomFileSystemFile))
                    {
                        dest = VirtualTempFolder.Default.CreateFile(StringHelper.GuidToCompactString(Guid.NewGuid()) + Path.GetExtension(this.DestFile.FullName));
                        if ((this.FUpdateMode != ArchiveUpdateMode.CreateNew) && flag)
                        {
                            this.FStage = PackStage.Relocating;
                            this.CopyChangeFile(this.DestFile, dest);
                        }
                    }
                    this.FStage = (this.FUpdateMode == ArchiveUpdateMode.CreateNew) ? PackStage.PackingNewItems : PackStage.MovingExistingItems;
                    if (dest == null)
                    {
                        WatcherChangeTypes changeType = flag ? WatcherChangeTypes.Changed : WatcherChangeTypes.Created;
                        LocalFileSystemCreator.RaiseFileChangedEvent(changeType, this.DestFile.FullName);
                    }
                    int errorCode = archiveFormat.PackFiles((dest != null) ? dest.FullName : this.DestFile.FullName, this.FSubFolder, parent.FullName, files, new ProcessDataProcCallback(this.ProcessData));
                    switch (errorCode)
                    {
                        case 0:
                        case 0x15:
                            break;

                        default:
                            WcxErrors.ThrowExceptionForError(errorCode);
                            break;
                    }
                    if (dest != null)
                    {
                        this.FStage = PackStage.Relocating;
                        this.CopyChangeFile(dest, this.DestFile);
                    }
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, this.DestFile.FullName);
                    this.FStage = PackStage.Finished;
                }
            }
        }

        protected override void DoWork()
        {
            if (this.ArchiveFormat is SevenZipFormatInfo)
            {
                this.DoSevenZipPack();
            }
            else
            {
                if (!(this.ArchiveFormat is WcxFormatInfo))
                {
                    throw new InvalidOperationException();
                }
                this.DoWcxPack();
            }
        }

        private IEnumerable<PackItemInfo> GetFolderContent<T>(IVirtualFolder folder, string relativePath, bool withFolders)
        {
            return new <GetFolderContent>d__0<T>(-2) { <>4__this = this, <>3__folder = folder, <>3__relativePath = relativePath, <>3__withFolders = withFolders };
        }

        public PackProgressSnapshot GetProgressSnapshot()
        {
            PackProgressSnapshot snapshot = new PackProgressSnapshot();
            lock (this.FSnapshotLock)
            {
                snapshot.Processed = this.FTotalProcessed;
                snapshot.TotalCount = this.FTotalCount;
                snapshot.ProcessedCount = this.FProcessedCount;
            }
            snapshot.Duration = base.Duration;
            snapshot.Stage = this.FStage;
            return snapshot;
        }

        public void GetProperty(int index, ItemPropId propID, IntPtr value)
        {
            PackItemInfo info = this.SourceFiles[index];
            switch (propID)
            {
                case ItemPropId.kpidPath:
                    Marshal.GetNativeVariantForObject(info.RelativePath, value);
                    return;

                case ItemPropId.kpidIsFolder:
                    Marshal.GetNativeVariantForObject(info.Item is IVirtualFolder, value);
                    return;

                case ItemPropId.kpidSize:
                {
                    long num = ((info.Item is IVirtualFolder) || !info.Item.IsPropertyAvailable(3)) ? 0L : Convert.ToInt64(info.Item[3]);
                    Marshal.GetNativeVariantForObject((ulong) num, value);
                    return;
                }
                case ItemPropId.kpidAttributes:
                {
                    if (!info.Item.IsPropertyAvailable(6))
                    {
                        Marshal.WriteInt16(value, (short) 0);
                        return;
                    }
                    FileAttributes attributes = (FileAttributes) info.Item[6];
                    Marshal.GetNativeVariantForObject((uint) attributes, value);
                    return;
                }
                case ItemPropId.kpidCreationTime:
                    this.GetTimeProperty(info.Item, 7, value);
                    return;

                case ItemPropId.kpidLastAccessTime:
                    this.GetTimeProperty(info.Item, 9, value);
                    return;

                case ItemPropId.kpidLastWriteTime:
                    this.GetTimeProperty(info.Item, 8, value);
                    return;

                case ItemPropId.kpidIsAnti:
                    Marshal.GetNativeVariantForObject(false, value);
                    return;
            }
            Marshal.WriteInt16(value, (short) 0);
        }

        public void GetStream(int index, out ISequentialInStream inStream)
        {
            base.CheckSuspendingPending();
            if (base.CancellationPending)
            {
                throw new AbortException();
            }
            this.FStage = PackStage.PackingNewItems;
            IChangeVirtualFile Source = (IChangeVirtualFile) this.SourceFiles[index].Item;
            this.BeforePackItem(Source);
            this.CurrentSourceStream = this.OpenItemStream(Source, delegate {
                return Source.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan, 0L);
            });
            if (this.CurrentSourceStream == null)
            {
                throw new AbortException();
            }
            if (!(!this.UseTimedWrapper || RemotingServices.IsTransparentProxy(this.CurrentSourceStream)))
            {
                inStream = new InStreamTimedWrapper(this.CurrentSourceStream);
            }
            else
            {
                inStream = new InStreamWrapper(this.CurrentSourceStream);
            }
        }

        private void GetTimeProperty(IVirtualItem item, int propertyId, IntPtr value)
        {
            if (item.IsPropertyAvailable(propertyId))
            {
                Marshal.GetNativeVariantForObject(((DateTime) item[propertyId]).ToFileTime(), value);
                Marshal.WriteInt16(value, (short) 0x40);
            }
            else
            {
                Marshal.WriteInt16(value, (short) 0);
            }
        }

        public void GetUpdateItemInfo(int index, out int newData, out int newProperties, out uint indexInArchive)
        {
            PackItemInfo info = this.SourceFiles[index];
            indexInArchive = info.UpdateIndex;
            if (indexInArchive == uint.MaxValue)
            {
                newData = 1;
                newProperties = 1;
            }
            else
            {
                newData = 0;
                newProperties = 0;
            }
        }

        private Stream OpenItemStream(IVirtualItem item, Func<Stream> getStream)
        {
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
            while (true)
            {
                ChangeItemAction action;
                try
                {
                    return getStream();
                }
                catch (UnauthorizedAccessException exception)
                {
                    action = this.ChangeItemError(item, canRetryOrElevate, exception);
                    canRetryOrElevate &= ~AvailableItemActions.CanElevate;
                }
                catch (IOException exception2)
                {
                    action = this.ChangeItemError(item, canRetryOrElevate, exception2);
                }
                ChangeItemAction action2 = action;
                if (action2 != ChangeItemAction.Retry)
                {
                    if (action2 != ChangeItemAction.Cancel)
                    {
                        throw new InvalidEnumArgumentException();
                    }
                    base.CancelAsync();
                    return null;
                }
            }
        }

        private int ProcessData(string FileName, int Size)
        {
            object obj2;
            base.CheckSuspendingPending();
            this.FStage = PackStage.PackingNewItems;
            if (!string.Equals(FileName, this.CurrentProcessFile))
            {
                IVirtualItem item;
                if (this.CurrentProcessFile != null)
                {
                    lock ((obj2 = this.FSnapshotLock))
                    {
                        this.FProcessedCount++;
                    }
                }
                if (this.SourceMap.TryGetValue(FileName, out item))
                {
                    this.BeforePackItem(item);
                }
                this.CurrentProcessFile = FileName;
            }
            lock ((obj2 = this.FSnapshotLock))
            {
                this.FTotalProcessed.AddProcessedSize((long) Size);
            }
            this.RaiseProgress();
            return (base.CancellationPending ? 0 : 1);
        }

        private void RaiseProgress()
        {
            int progressPercent;
            lock (this.FSnapshotLock)
            {
                progressPercent = this.FTotalProcessed.ProgressPercent;
            }
            if ((progressPercent <= 100) && (this.FStoredProgress != progressPercent))
            {
                base.RaiseProgressChanged(progressPercent, this.GetProgressSnapshot());
                this.FStoredProgress = progressPercent;
            }
        }

        private bool RejectItem(IVirtualItem item)
        {
            if (this.OnRejectItem != null)
            {
                CancelVirtualItemEventArgs e = new CancelVirtualItemEventArgs(item);
                this.OnRejectItem(this, e);
                return e.Cancel;
            }
            return false;
        }

        public void SetCompleted(ref ulong completeValue)
        {
            base.CheckSuspendingPending();
            if (base.CancellationPending)
            {
                throw new AbortException();
            }
            lock (this.FSnapshotLock)
            {
                this.FTotalProcessed.SetProcessedSize((long) completeValue);
            }
            this.RaiseProgress();
        }

        public void SetOperationResult(int operationResult)
        {
            this.CurrentSourceStream.Close();
            lock (this.FSnapshotLock)
            {
                this.FProcessedCount++;
            }
        }

        public void SetTotal(ulong total)
        {
            this.FTotalProcessed.SetTotalSize((long) total);
            base.RaiseProgressChanged(0, this.GetProgressSnapshot());
        }

        public override string Name
        {
            get
            {
                return "Pack";
            }
        }

        [CompilerGenerated]
        private sealed class <GetFolderContent>d__0<T> : IEnumerable<PackWorker.PackItemInfo>, IEnumerable, IEnumerator<PackWorker.PackItemInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private PackWorker.PackItemInfo <>2__current;
            public IVirtualFolder <>3__folder;
            public string <>3__relativePath;
            public bool <>3__withFolders;
            public PackWorker <>4__this;
            public IVirtualFolder <>7__wrap6;
            public IEnumerator<IVirtualItem> <>7__wrap8;
            public List<IVirtualFolder>.Enumerator <>7__wrapa;
            public IEnumerator<PackWorker.PackItemInfo> <>7__wrapc;
            private int <>l__initialThreadId;
            public List<IVirtualFolder> <Folders>5__1;
            public IVirtualFolder <NextFolder>5__3;
            public IVirtualFolder <NextFolder>5__4;
            public IVirtualItem <NextItem>5__2;
            public PackWorker.PackItemInfo <NextItemInfo>5__5;
            public IVirtualFolder folder;
            public string relativePath;
            public bool withFolders;

            [DebuggerHidden]
            public <GetFolderContent>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally7()
            {
                this.<>1__state = -1;
                Monitor.Exit(this.<>7__wrap6);
            }

            private void <>m__Finally9()
            {
                this.<>1__state = 1;
                if (this.<>7__wrap8 != null)
                {
                    this.<>7__wrap8.Dispose();
                }
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                this.<>7__wrapa.Dispose();
            }

            private void <>m__Finallyd()
            {
                this.<>1__state = 5;
                if (this.<>7__wrapc != null)
                {
                    this.<>7__wrapc.Dispose();
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

                        case 3:
                            goto Label_0158;

                        case 4:
                            goto Label_01C7;

                        case 7:
                            goto Label_02E1;

                        default:
                            goto Label_0319;
                    }
                    this.<>1__state = -1;
                    this.<Folders>5__1 = new List<IVirtualFolder>();
                    Monitor.Enter(this.<>7__wrap6 = this.folder);
                    this.<>1__state = 1;
                    this.<>7__wrap8 = this.folder.GetContent().GetEnumerator();
                    this.<>1__state = 2;
                    while (this.<>7__wrap8.MoveNext())
                    {
                        this.<NextItem>5__2 = this.<>7__wrap8.Current;
                        this.<>4__this.CheckSuspendingPending();
                        if (this.<>4__this.CancellationPending)
                        {
                            this.System.IDisposable.Dispose();
                            goto Label_0319;
                        }
                        if (!((this.<>4__this.FFilter == null) || this.<>4__this.FFilter.IsMatch(this.<NextItem>5__2)))
                        {
                            continue;
                        }
                        if (!(this.<NextItem>5__2 is T))
                        {
                            goto Label_0165;
                        }
                        this.<>4__this.FTotalCount++;
                        this.<>2__current = new PackWorker.PackItemInfo(this.<NextItem>5__2, Path.Combine(this.relativePath, this.<NextItem>5__2.Name));
                        this.<>1__state = 3;
                        return true;
                    Label_0158:
                        this.<>1__state = 2;
                        goto Label_0208;
                    Label_0165:
                        this.<NextFolder>5__3 = this.<NextItem>5__2 as IVirtualFolder;
                        if (this.<NextFolder>5__3 == null)
                        {
                            goto Label_01E3;
                        }
                        if (!this.withFolders)
                        {
                            goto Label_01CE;
                        }
                        this.<>2__current = new PackWorker.PackItemInfo(this.<NextFolder>5__3, Path.Combine(this.relativePath, this.<NextFolder>5__3.Name));
                        this.<>1__state = 4;
                        return true;
                    Label_01C7:
                        this.<>1__state = 2;
                    Label_01CE:
                        this.<Folders>5__1.Add(this.<NextFolder>5__3);
                        goto Label_0208;
                    Label_01E3:
                        if (this.<>4__this.RejectItem(this.<NextItem>5__2))
                        {
                            this.<>4__this.CancelAsync();
                        }
                    Label_0208:;
                    }
                    this.<>m__Finally9();
                    this.<>m__Finally7();
                    this.<>7__wrapa = this.<Folders>5__1.GetEnumerator();
                    this.<>1__state = 5;
                    while (this.<>7__wrapa.MoveNext())
                    {
                        this.<NextFolder>5__4 = this.<>7__wrapa.Current;
                        if (this.<>4__this.CancellationPending)
                        {
                            this.System.IDisposable.Dispose();
                            goto Label_0319;
                        }
                        this.<>7__wrapc = this.<>4__this.GetFolderContent<T>(this.<NextFolder>5__4, Path.Combine(this.relativePath, this.<NextFolder>5__4.Name), this.withFolders).GetEnumerator();
                        this.<>1__state = 6;
                        while (this.<>7__wrapc.MoveNext())
                        {
                            this.<NextItemInfo>5__5 = this.<>7__wrapc.Current;
                            this.<>2__current = this.<NextItemInfo>5__5;
                            this.<>1__state = 7;
                            return true;
                        Label_02E1:
                            this.<>1__state = 6;
                        }
                        this.<>m__Finallyd();
                    }
                    this.<>m__Finallyb();
                Label_0319:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<PackWorker.PackItemInfo> IEnumerable<PackWorker.PackItemInfo>.GetEnumerator()
            {
                PackWorker.<GetFolderContent>d__0<T> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (PackWorker.<GetFolderContent>d__0<T>) this;
                }
                else
                {
                    d__ = new PackWorker.<GetFolderContent>d__0<T>(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.folder = this.<>3__folder;
                d__.relativePath = this.<>3__relativePath;
                d__.withFolders = this.<>3__withFolders;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Workers.PackWorker.PackItemInfo>.GetEnumerator();
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
                    case 3:
                    case 4:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 2:
                                case 3:
                                case 4:
                                    break;

                                default:
                                    break;
                            }
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finally9();
                            }
                        }
                        finally
                        {
                            this.<>m__Finally7();
                        }
                        break;

                    case 5:
                    case 6:
                    case 7:
                        try
                        {
                            switch (this.<>1__state)
                            {
                                case 6:
                                case 7:
                                    break;

                                default:
                                    break;
                            }
                            try
                            {
                            }
                            finally
                            {
                                this.<>m__Finallyd();
                            }
                        }
                        finally
                        {
                            this.<>m__Finallyb();
                        }
                        break;
                }
            }

            PackWorker.PackItemInfo IEnumerator<PackWorker.PackItemInfo>.Current
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

        private class PackItemInfo
        {
            public readonly IVirtualItem Item;
            public readonly string RelativePath;
            public readonly uint UpdateIndex;

            public PackItemInfo(IVirtualItem item, string relativePath)
            {
                this.Item = item;
                this.RelativePath = relativePath;
                this.UpdateIndex = uint.MaxValue;
            }

            public PackItemInfo(uint updateIndex, string relativePath)
            {
                this.UpdateIndex = updateIndex;
                this.RelativePath = relativePath;
            }
        }
    }
}

