namespace Nomad.Workers
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Win32;
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Properties;
    using Nomad.Workers.Configuration;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class CopyWorker : EventBackgroundWorker, ISetOwnerWindow
    {
        private byte[] Buffer1;
        private byte[] Buffer2;
        private bool CanRaiseProgress;
        private IVirtualFolder FContent;
        private int FCopyBufferSize;
        private CopyMode FCopyMode;
        private CopyWorkerOptions FCopyOptions;
        private IVirtualFolder FDest;
        private IVirtualItemFilter FFilter;
        private int FProcessedCount;
        private IDictionary<IVirtualItem, int> FRenameContent;
        private IRenameFilter FRenameFilter;
        private int FSkippedCount;
        private object FSnapshotLock;
        private int FStoredProgress;
        private int FTotalCount;
        private ProcessedSize FTotalProcessed;
        private Exception SearchError;

        public event EventHandler<ProgressEventArgs> FileProgressChanged;

        public event EventHandler<CopyItemEventArgs> OnAfterCopyItem;

        public event EventHandler<BeforeCopyItemEventArgs> OnBeforeCopyItem;

        public event EventHandler<CopyItemErrorEventArgs> OnCopyItemError;

        public event EventHandler<ChangeItemErrorEventArgs> OnCreateFolderError;

        public event EventHandler<ChangeItemErrorEventArgs> OnDeleteItemError;

        public event EventHandler<ChangeItemErrorEventArgs> OnMoveItemError;

        public CopyWorker(IEnumerable<IVirtualItem> items, IVirtualFolder dest) : this(items, dest, null, 0, null, null)
        {
        }

        public CopyWorker(IEnumerable<IVirtualItem> items, IVirtualFolder dest, CopySettings settings, CopyWorkerOptions copyOptions, IVirtualItemFilter filter, IRenameFilter renameFilter)
        {
            this.FSnapshotLock = new object();
            this.FTotalProcessed = new ProcessedSize();
            this.FStoredProgress = -1;
            this.FCopyBufferSize = 0x40000;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (dest == null)
            {
                throw new ArgumentNullException("dest");
            }
            if (!VirtualItemHelper.CanCreateInFolder(dest))
            {
                throw new ArgumentException(string.Format(Resources.sCannotCopyToBasicFolder, dest.FullName));
            }
            this.FContent = new AggregatedVirtualFolder(items);
            ICloneable cloneable = dest as ICloneable;
            if (cloneable != null)
            {
                this.FDest = (IVirtualFolder) cloneable.Clone();
            }
            else
            {
                this.FDest = dest;
            }
            this.FCopyOptions = copyOptions;
            if (settings != null)
            {
                this.FCopyOptions |= settings.DefaultCopyOptions & (CopyWorkerOptions.CopyFolderTime | CopyWorkerOptions.ClearROFromCD);
                this.FCopyBufferSize = settings.CopyBufferSize;
            }
            this.FFilter = filter;
            this.FRenameFilter = renameFilter;
            if (this.FRenameFilter != null)
            {
                this.FRenameContent = new Dictionary<IVirtualItem, int>();
                foreach (IVirtualItem item in items)
                {
                    this.FRenameContent.Add(item, 0);
                }
            }
            this.Buffer1 = new byte[this.FCopyBufferSize];
        }

        private OverwriteDialogResult BeforeCopy(IVirtualItem source, IVirtualItem dest, out string newName)
        {
            newName = string.Empty;
            OverwriteDialogResult overwrite = OverwriteDialogResult.Overwrite;
            if (this.OnBeforeCopyItem != null)
            {
                BeforeCopyItemEventArgs e = new BeforeCopyItemEventArgs(source, dest);
                this.OnBeforeCopyItem(this, e);
                newName = e.NewName;
                overwrite = e.OverwriteResult;
            }
            return overwrite;
        }

        private ChangeItemAction ChangeItemError(IVirtualItem item, IVirtualItem source, IVirtualItem dest, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            if (this.OnCopyItemError != null)
            {
                CopyItemErrorEventArgs e = new CopyItemErrorEventArgs(item, source, dest, available, error);
                this.OnCopyItemError(this, e);
                none = e.Action;
            }
            return none;
        }

        private bool CheckDestFreeSpace(OverwriteDialogResult overwrite, IVirtualFile source, IVirtualFile dest, IVirtualFolder destFolder)
        {
            bool flag;
            long num = -1L;
            IVirtualFolder folderRoot = VirtualItemHelper.GetFolderRoot(destFolder);
            if ((folderRoot != null) && folderRoot.IsPropertyAvailable(0x1b))
            {
                object obj2 = folderRoot[0x1b];
                if (obj2 != null)
                {
                    num = Convert.ToInt64(obj2);
                }
            }
            if (num < 0L)
            {
                return true;
            }
            object obj3 = source[3];
            if (obj3 == null)
            {
                return true;
            }
            uint clusterSize = 0;
            IGetVirtualVolume volume = destFolder as IGetVirtualVolume;
            if (volume != null)
            {
                clusterSize = volume.ClusterSize;
                if (string.Equals(volume.FileSystem, "FAT32", StringComparison.OrdinalIgnoreCase))
                {
                    num = Math.Min(num, 0x100000000L);
                }
            }
            long num3 = Convert.ToInt64(obj3);
            if (clusterSize > 0)
            {
                num3 = (long) (((num3 / ((ulong) clusterSize)) + Math.Sign((long) (num3 % ((ulong) clusterSize)))) * clusterSize);
            }
        Label_00E7:
            flag = num3 <= num;
            if ((!flag && (dest is IPersistVirtualItem)) && ((IPersistVirtualItem) dest).Exists)
            {
                long num4 = Convert.ToInt64(dest[3]);
                if (clusterSize > 0)
                {
                    num4 = (long) (((num4 / ((ulong) clusterSize)) + Math.Sign((long) (num4 % ((ulong) clusterSize)))) * clusterSize);
                }
                switch (overwrite)
                {
                    case OverwriteDialogResult.Overwrite:
                        flag = num3 < (num + num4);
                        break;

                    case OverwriteDialogResult.Resume:
                        flag = (num3 - num4) < num;
                        break;
                }
            }
            if (flag)
            {
                return true;
            }
            Exception error = new WarningException(string.Format(Resources.sNotEnoughSpaceInDest, destFolder.FullName, source.FullName));
            switch (this.ChangeItemError(null, source, dest, AvailableItemActions.CanRetryOrIgnore, error))
            {
                case ChangeItemAction.Retry:
                    goto Label_00E7;

                case ChangeItemAction.Ignore:
                    return true;

                case ChangeItemAction.Skip:
                    return false;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return false;
            }
            throw error;
        }

        public bool CheckOption(CopyWorkerOptions option)
        {
            return ((this.FCopyOptions & option) == option);
        }

        private CopyItemAction CopyAlternateStreams(IVirtualItem source, IVirtualItem dest, bool asyncCopy)
        {
            IVirtualAlternateStreams SourceStreams = source as IVirtualAlternateStreams;
            IVirtualAlternateStreams DestStreams = dest as IVirtualAlternateStreams;
            if (!((((SourceStreams != null) && (DestStreams != null)) && SourceStreams.HasAlternateStreams) && DestStreams.IsSupported))
            {
                return CopyItemAction.Next;
            }
            CopyItemAction next = CopyItemAction.Next;
            using (IEnumerator<string> enumerator = SourceStreams.GetStreamNames().GetEnumerator())
            {
                string NextStreamName;
                while (enumerator.MoveNext())
                {
                    NextStreamName = enumerator.Current;
                    if (next != CopyItemAction.Next)
                    {
                        return next;
                    }
                    Stream InStream = null;
                    bool DoAsyncCopy = asyncCopy;
                    next = this.OpenItemStream(source, source, dest, delegate {
                        InStream = SourceStreams.Open(NextStreamName, FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan | (DoAsyncCopy ? FileOptions.Asynchronous : FileOptions.None));
                    });
                    using (InStream)
                    {
                        if ((InStream != null) && (next == CopyItemAction.Next))
                        {
                            DoAsyncCopy = DoAsyncCopy && (!InStream.CanSeek || (InStream.Length > this.Buffer1.Length));
                            Stream OutStream = null;
                            next = this.OpenItemStream(dest, source, dest, delegate {
                                OutStream = DestStreams.Open(NextStreamName, FileMode.Create, FileAccess.Write, FileShare.None, FileOptions.SequentialScan | (DoAsyncCopy ? FileOptions.Asynchronous : FileOptions.None));
                            });
                            using (OutStream)
                            {
                                if ((OutStream != null) && (next == CopyItemAction.Next))
                                {
                                    FileStream stream = InStream as FileStream;
                                    DoAsyncCopy &= ((stream == null) || stream.IsAsync) && (InStream != Stream.Null);
                                    FileStream stream2 = OutStream as FileStream;
                                    DoAsyncCopy &= ((stream2 == null) || stream2.IsAsync) && (OutStream != Stream.Null);
                                    if (InStream.CanSeek)
                                    {
                                        lock (this.FSnapshotLock)
                                        {
                                            this.FTotalProcessed.AddTotalSize(InStream.Length);
                                        }
                                    }
                                    if (DoAsyncCopy)
                                    {
                                        next = this.CopyStreamAsync(InStream, OutStream, source, dest);
                                    }
                                    else
                                    {
                                        next = this.CopyStream(InStream, OutStream, source, dest);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return next;
        }

        private void CopyItem(IVirtualItem sourceItem, IVirtualFolder destFolder, IGetStream getSourceStream, List<IVirtualItem> skippedItems)
        {
            Exception exception;
            object obj2;
            CopyItemAction next = CopyItemAction.Next;
            IChangeVirtualFile DestFile = null;
            try
            {
                if (!(sourceItem is IChangeVirtualFile))
                {
                    throw new ItemChangeNotSupportedException();
                }
                ICreateVirtualFile file = destFolder as ICreateVirtualFile;
                if (file == null)
                {
                    throw new WarningException(string.Format(Resources.sCannotCopyToBasicFolder, destFolder.FullName));
                }
                DestFile = file.CreateFile(this.CreateNewName(sourceItem));
                OverwriteDialogResult Overwrite = OverwriteDialogResult.Overwrite;
                do
                {
                    string str;
                    Overwrite = this.BeforeCopy((IVirtualFile) sourceItem, DestFile, out str);
                    switch (Overwrite)
                    {
                        case OverwriteDialogResult.Rename:
                            DestFile = file.CreateFile(str);
                            break;

                        case OverwriteDialogResult.Skip:
                            skippedItems.Add(sourceItem);
                            lock ((obj2 = this.FSnapshotLock))
                            {
                                this.FSkippedCount++;
                                if (sourceItem.IsPropertyAvailable(3))
                                {
                                    this.FTotalProcessed.AddProcessedSize(Convert.ToInt64(sourceItem[3]));
                                }
                            }
                            return;

                        case OverwriteDialogResult.Abort:
                            base.CancelAsync();
                            return;
                    }
                }
                while (((Overwrite != OverwriteDialogResult.Append) && (Overwrite != OverwriteDialogResult.Resume)) && (Overwrite != OverwriteDialogResult.Overwrite));
                if (sourceItem.Equals(DestFile))
                {
                    throw new WarningException(string.Format(Resources.sCannotCopyFileToItself, sourceItem.FullName));
                }
                IChangeVirtualFile CurrentFile = (IChangeVirtualFile) sourceItem;
                IChangeVirtualItem item = sourceItem as IChangeVirtualItem;
                IChangeVirtualItem destItem = DestFile as IChangeVirtualItem;
                if ((this.CheckOption(CopyWorkerOptions.DeleteSource) && (item != null)) && item.CanMoveTo(destFolder))
                {
                    if (this.MoveItem(item, destFolder) == null)
                    {
                        next = CopyItemAction.Skip;
                    }
                    lock ((obj2 = this.FSnapshotLock))
                    {
                        this.FTotalProcessed.AddProcessedSize(Convert.ToInt64(CurrentFile[3]));
                    }
                }
                else
                {
                    if (!(!this.CheckOption(CopyWorkerOptions.CheckFreeSpace) || this.CheckDestFreeSpace(Overwrite, CurrentFile, DestFile, destFolder)))
                    {
                        next = CopyItemAction.Skip;
                    }
                    if (next == CopyItemAction.Next)
                    {
                        long num = 0L;
                        FileAttributes attributes = 0;
                        bool flag = false;
                        long position = 0L;
                        ISetOwnerWindow window = CurrentFile as ISetOwnerWindow;
                        if (window != null)
                        {
                            window.Owner = this.Owner;
                        }
                        try
                        {
                            if ((destItem != null) && destItem.Exists)
                            {
                                num = Convert.ToInt64(DestFile[3]);
                                attributes = DestFile.Attributes;
                                VirtualItemHelper.ResetSystemAttributes(DestFile);
                            }
                            else
                            {
                                Overwrite = OverwriteDialogResult.Overwrite;
                            }
                            bool flag2 = (((this.CheckOption(CopyWorkerOptions.UseSystemCopy) && OS.IsWinNT) && ((Overwrite == OverwriteDialogResult.Overwrite) && (getSourceStream == null))) && (CurrentFile is CustomFileSystemFile)) && (DestFile is CustomFileSystemFile);
                            if (flag2)
                            {
                                try
                                {
                                    if (this.CopySystem(CurrentFile, DestFile))
                                    {
                                        VirtualItemHelper.ResetSystemAttributes(DestFile);
                                    }
                                    else
                                    {
                                        next = CopyItemAction.Skip;
                                    }
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    IElevatable elevatable = CurrentFile as IElevatable;
                                    bool flag3 = (elevatable != null) && elevatable.CanElevate;
                                    if (!flag3)
                                    {
                                        elevatable = DestFile as IElevatable;
                                        flag3 = (elevatable != null) && elevatable.CanElevate;
                                    }
                                    if (!flag3)
                                    {
                                        throw;
                                    }
                                    flag2 = false;
                                }
                            }
                            if (!flag2)
                            {
                                bool asyncCopy = this.CheckOption(CopyWorkerOptions.AsyncCopy);
                                if (this.CheckOption(CopyWorkerOptions.AutoAsyncCopy))
                                {
                                    IGetVirtualVolume parent = CurrentFile.Parent as IGetVirtualVolume;
                                    IGetVirtualVolume volume2 = destFolder as IGetVirtualVolume;
                                    asyncCopy = ((volume2 != null) && (parent != null)) && ((volume2.Location != parent.Location) || (volume2.VolumeType != parent.VolumeType));
                                }
                                Stream InStream = null;
                                bool DoAsyncCopy = asyncCopy;
                                long StartOffset = 0L;
                                if (Overwrite == OverwriteDialogResult.Resume)
                                {
                                    StartOffset = num;
                                }
                                next = this.OpenItemStream(sourceItem, sourceItem, DestFile, delegate {
                                    if (getSourceStream != null)
                                    {
                                        InStream = getSourceStream.GetStream();
                                        if (StartOffset > 0L)
                                        {
                                            InStream.Seek(StartOffset, SeekOrigin.Begin);
                                        }
                                    }
                                    else
                                    {
                                        InStream = CurrentFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.SequentialScan | (DoAsyncCopy ? FileOptions.Asynchronous : FileOptions.None), StartOffset);
                                    }
                                });
                                using (InStream)
                                {
                                    if ((InStream != null) && (next == CopyItemAction.Next))
                                    {
                                        DoAsyncCopy = DoAsyncCopy && (!InStream.CanSeek || (InStream.Length > this.Buffer1.Length));
                                        Stream OutStream = null;
                                        next = this.OpenItemStream(DestFile, sourceItem, DestFile, delegate {
                                            OutStream = DestFile.Open((Overwrite == OverwriteDialogResult.Overwrite) ? FileMode.Create : FileMode.Append, FileAccess.Write, FileShare.None, FileOptions.SequentialScan | (DoAsyncCopy ? FileOptions.Asynchronous : FileOptions.None), 0L);
                                        });
                                        using (OutStream)
                                        {
                                            if ((OutStream != null) && (next == CopyItemAction.Next))
                                            {
                                                flag = true;
                                                FileStream stream = InStream as FileStream;
                                                DoAsyncCopy &= ((stream == null) || stream.IsAsync) && (InStream != Stream.Null);
                                                FileStream stream2 = OutStream as FileStream;
                                                DoAsyncCopy &= ((stream2 == null) || stream2.IsAsync) && (OutStream != Stream.Null);
                                                if (DoAsyncCopy)
                                                {
                                                    next = this.CopyStreamAsync(InStream, OutStream, CurrentFile, DestFile);
                                                }
                                                else
                                                {
                                                    next = this.CopyStream(InStream, OutStream, CurrentFile, DestFile);
                                                }
                                                if (OutStream.CanSeek)
                                                {
                                                    if (Overwrite == OverwriteDialogResult.Append)
                                                    {
                                                        position = OutStream.Position - num;
                                                    }
                                                    else
                                                    {
                                                        position = OutStream.Position;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!((next != CopyItemAction.Next) || base.CancellationPending))
                                {
                                    this.CopyAlternateStreams(CurrentFile, DestFile, asyncCopy);
                                }
                            }
                            if ((next == CopyItemAction.Next) && !base.CancellationPending)
                            {
                                if (destItem != null)
                                {
                                    next = this.SetOutFileAttributes(sourceItem, destItem, true, this.CheckOption(CopyWorkerOptions.CopyItemTime));
                                }
                                if (((this.CheckOption(CopyWorkerOptions.DeleteSource) && (next == CopyItemAction.Next)) && (item != null)) && !this.DeleteItem(CurrentFile))
                                {
                                    next = CopyItemAction.Skip;
                                }
                                this.RaiseAfterCopy(sourceItem, DestFile);
                            }
                        }
                        catch (AbortException)
                        {
                            next = CopyItemAction.SkipUndoDest;
                            base.CancelAsync();
                        }
                        catch (Exception exception3)
                        {
                            exception = exception3;
                            next = this.CopyItemError(null, sourceItem, DestFile, flag ? AvailableItemActions.CanUndoDestination : AvailableItemActions.None, exception);
                        }
                        if ((next != CopyItemAction.Next) && CurrentFile.IsPropertyAvailable(3))
                        {
                            lock ((obj2 = this.FSnapshotLock))
                            {
                                this.FTotalProcessed.AddProcessedSize(Convert.ToInt64(CurrentFile[3]) - position);
                            }
                        }
                        if (flag && (next == CopyItemAction.SkipUndoDest))
                        {
                            if (Overwrite != OverwriteDialogResult.Overwrite)
                            {
                                using (Stream stream3 = DestFile.Open(FileMode.Open, FileAccess.Write, FileShare.None, FileOptions.None, 0L))
                                {
                                    if (stream3.CanSeek && stream3.CanWrite)
                                    {
                                        stream3.SetLength(num);
                                    }
                                }
                                if ((destItem != null) && destItem.CanSetProperty(6))
                                {
                                    destItem[6] = attributes;
                                }
                            }
                            else if (destItem != null)
                            {
                                destItem.Delete(false);
                            }
                        }
                    }
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
                next = this.CopyItemError(null, sourceItem, DestFile, AvailableItemActions.None, exception);
            }
            if (next != CopyItemAction.Next)
            {
                skippedItems.Add(sourceItem);
                lock ((obj2 = this.FSnapshotLock))
                {
                    this.FSkippedCount++;
                }
            }
            else
            {
                lock ((obj2 = this.FSnapshotLock))
                {
                    this.FProcessedCount++;
                }
            }
        }

        private CopyItemAction CopyItemError(IVirtualItem item, IVirtualItem source, IVirtualItem dest, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            bool undoDest = true;
            if (this.OnCopyItemError != null)
            {
                CopyItemErrorEventArgs e = new CopyItemErrorEventArgs(item, source, dest, available, error);
                this.OnCopyItemError(this, e);
                none = e.Action;
                undoDest = e.UndoDest;
            }
            switch (none)
            {
                case ChangeItemAction.Retry:
                    return CopyItemAction.Next;

                case ChangeItemAction.Skip:
                    return (undoDest ? CopyItemAction.SkipUndoDest : CopyItemAction.Skip);

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return (undoDest ? CopyItemAction.SkipUndoDest : CopyItemAction.Skip);
            }
            throw error;
        }

        private CopyItemAction CopyStream(Stream sourceStream, Stream destStream, IVirtualItem sourceItem, IVirtualItem destItem)
        {
            object obj2;
            lock ((obj2 = this.FSnapshotLock))
            {
                this.FCopyMode = CopyMode.Sync;
            }
            int count = 0;
            ProcessedSize processed = CreateFileProcessed(sourceStream, sourceItem);
            if (!((processed.Total <= this.Buffer1.Length) || this.RaiseFileProgress(ref processed)))
            {
                return CopyItemAction.SkipUndoDest;
            }
            do
            {
                if (base.CheckCancellationPending())
                {
                    return CopyItemAction.SkipUndoDest;
                }
                count = sourceStream.Read(this.Buffer1, 0, this.Buffer1.Length);
                try
                {
                    destStream.Write(this.Buffer1, 0, count);
                }
                catch (IOException exception)
                {
                    CopyItemAction action = this.HandleCopyIOException(destItem, sourceItem, destItem, destStream, this.Buffer1, count, exception);
                    if (action != CopyItemAction.Next)
                    {
                        return action;
                    }
                }
                lock ((obj2 = this.FSnapshotLock))
                {
                    this.FTotalProcessed.AddProcessedSize((long) count);
                }
                this.RaiseProgress();
                processed.AddProcessedSize((long) count);
                if (!this.RaiseFileProgress(ref processed))
                {
                    return CopyItemAction.SkipUndoDest;
                }
            }
            while (count > 0);
            return CopyItemAction.Next;
        }

        private CopyItemAction CopyStreamAsync(Stream sourceStream, Stream destStream, IVirtualItem sourceItem, IVirtualItem destItem)
        {
            object obj2;
            lock ((obj2 = this.FSnapshotLock))
            {
                this.FCopyMode = CopyMode.Async;
            }
            if (this.Buffer2 == null)
            {
                this.Buffer2 = new byte[this.Buffer1.Length];
            }
            byte[] buffer = this.Buffer1;
            byte[] buffer2 = this.Buffer2;
            int count = 0;
            ProcessedSize processed = CreateFileProcessed(sourceStream, sourceItem);
            if (!((processed.Total <= buffer.Length) || this.RaiseFileProgress(ref processed)))
            {
                return CopyItemAction.SkipUndoDest;
            }
            IAsyncResult asyncResult = sourceStream.BeginRead(buffer, 0, buffer.Length, null, null);
            do
            {
                count = sourceStream.EndRead(asyncResult);
                if (base.CheckCancellationPending())
                {
                    return CopyItemAction.SkipUndoDest;
                }
                IAsyncResult result2 = destStream.BeginWrite(buffer, 0, count, null, null);
                byte[] buffer3 = buffer;
                if (count > 0)
                {
                    asyncResult = sourceStream.BeginRead(buffer2, 0, buffer2.Length, null, null);
                    buffer2 = Interlocked.Exchange<byte[]>(ref buffer, buffer2);
                }
                try
                {
                    destStream.EndWrite(result2);
                }
                catch (IOException exception)
                {
                    CopyItemAction action = this.HandleCopyIOException(destItem, sourceItem, destItem, destStream, buffer3, count, exception);
                    if (action != CopyItemAction.Next)
                    {
                        return action;
                    }
                }
                lock ((obj2 = this.FSnapshotLock))
                {
                    this.FTotalProcessed.AddProcessedSize((long) count);
                }
                this.RaiseProgress();
                processed.AddProcessedSize((long) count);
                if (!this.RaiseFileProgress(ref processed))
                {
                    return CopyItemAction.SkipUndoDest;
                }
            }
            while (count > 0);
            return CopyItemAction.Next;
        }

        private bool CopySystem(IVirtualItem sourceFile, IVirtualItem destFile)
        {
            long processed;
            object obj2;
            lock ((obj2 = this.FSnapshotLock))
            {
                processed = this.FTotalProcessed.Processed;
                this.FCopyMode = CopyMode.System;
            }
            bool pbCancel = false;
            CopyProgressRoutine lpProgressRoutine = new CopyProgressRoutine(this.SystemCopyProgress);
            if (!System.IO.File.Exists(destFile.FullName))
            {
                using (System.IO.File.Create(destFile.FullName))
                {
                    LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Created, destFile.FullName);
                }
            }
            IntPtr ptr = Marshal.AllocHGlobal(8);
            try
            {
                Marshal.WriteInt64(ptr, 0L);
                if (!Windows.CopyFileEx(sourceFile.FullName, destFile.FullName, lpProgressRoutine, ptr, ref pbCancel, (COPY_FILE) 0))
                {
                    lock ((obj2 = this.FSnapshotLock))
                    {
                        this.FTotalProcessed.SetProcessedSize(processed);
                    }
                    int error = Marshal.GetLastWin32Error();
                    switch (error)
                    {
                        case 5:
                        {
                            Win32Exception inner = new Win32Exception(error);
                            throw new UnauthorizedAccessException(inner.Message, inner);
                        }
                        case 0x57:
                        {
                            IVirtualFolder parent = destFile.Parent;
                            if (parent != null)
                            {
                                throw new WarningException(string.Format(Resources.sNotEnoughSpaceInDest, parent.FullName, sourceFile.FullName));
                            }
                            throw new Win32IOException(error);
                        }
                        case 0x4d3:
                            return false;
                    }
                    throw new Win32IOException(error);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Changed, destFile.FullName);
            return true;
        }

        private static ProcessedSize CreateFileProcessed(Stream sourceStream, IVirtualItem sourceItem)
        {
            ProcessedSize size = new ProcessedSize(0L, 0L);
            if (sourceStream.CanSeek)
            {
                size.SetProcessedSize(sourceStream.Position);
                size.SetTotalSize(sourceStream.Length);
                return size;
            }
            object obj2 = sourceItem[3];
            if (obj2 != null)
            {
                size.SetTotalSize((long) obj2);
            }
            return size;
        }

        private ChangeItemAction CreateFolderError(IVirtualFolder folder, AvailableItemActions availableActions, Exception error)
        {
            return this.ItemError(this.OnCreateFolderError, folder, availableActions, error);
        }

        private IVirtualFolder CreateNewFolder(IVirtualFolder destFolder, IVirtualFolder sourceFolder)
        {
            ChangeItemAction action;
            ICreateVirtualFolder folder = destFolder as ICreateVirtualFolder;
            if (folder == null)
            {
                throw new WarningException(string.Format(Resources.sCannotCopyToBasicFolder, destFolder.FullName));
            }
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
        Label_002D:;
            try
            {
                IVirtualFolder dest = folder.CreateFolder(this.CreateNewName(sourceFolder));
                if (this.CopyAlternateStreams(sourceFolder, dest, false) != CopyItemAction.Next)
                {
                    return null;
                }
                IChangeVirtualItem destItem = dest as IChangeVirtualItem;
                if ((destItem != null) && (this.SetOutFileAttributes(sourceFolder, destItem, true, false) != CopyItemAction.Next))
                {
                    return null;
                }
                return dest;
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.CreateFolderError(destFolder, canRetryOrElevate, exception);
                canRetryOrElevate &= ~AvailableItemActions.CanElevate;
            }
            catch (Exception exception2)
            {
                action = this.CreateFolderError(destFolder, canRetryOrElevate, exception2);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_002D;

                case ChangeItemAction.Skip:
                    return null;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return null;
            }
            throw new InvalidEnumArgumentException();
        }

        private string CreateNewName(IVirtualItem item)
        {
            string name;
            if (((this.FRenameFilter != null) && (this.FRenameContent != null)) && this.FRenameContent.ContainsKey(item))
            {
                name = this.FRenameFilter.CreateNewName(item.Name);
            }
            else
            {
                name = item.Name;
            }
            return PathHelper.NormalizeInvalidFileName(name);
        }

        private bool DeleteItem(IVirtualItem item)
        {
            ChangeItemAction action;
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
        Label_0003:;
            try
            {
                IChangeVirtualItem item2 = item as IChangeVirtualItem;
                if (item2 == null)
                {
                    throw new ItemChangeNotSupportedException(string.Format(Resources.sErrorDeleteNonChangeableItem, item.FullName));
                }
                item2.Delete(false);
                return true;
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.DeleteItemError(item, canRetryOrElevate, exception);
                canRetryOrElevate &= ~AvailableItemActions.CanElevate;
            }
            catch (IOException exception2)
            {
                if (Marshal.GetHRForException(exception2) == HRESULT.HRESULT_FROM_WIN32(0x91))
                {
                    return false;
                }
                action = this.DeleteItemError(item, canRetryOrElevate, exception2);
            }
            catch (Exception exception3)
            {
                action = this.DeleteItemError(item, canRetryOrElevate, exception3);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_0003;

                case ChangeItemAction.Skip:
                    return false;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return false;
            }
            throw new InvalidEnumArgumentException();
        }

        private ChangeItemAction DeleteItemError(IVirtualItem item, AvailableItemActions availableActions, Exception error)
        {
            return this.ItemError(this.OnDeleteItemError, item, availableActions, error);
        }

        protected override void DoWork()
        {
            try
            {
                using (new ThreadExecutionStateLock(true, false))
                {
                    using (VirtualSearchFolder folder = new VirtualSearchFolder(this.FContent, this.FFilter, SearchFolderOptions.SkipUnmatchedSubfolders | SearchFolderOptions.ProcessSubfolders))
                    {
                        int num;
                        ProcessItemHandler handler = null;
                        folder.OnChanged += new EventHandler<VirtualItemChangedEventArgs>(this.SearchFolderChanged);
                        folder.Completed += new AsyncCompletedEventHandler(this.SearchFolderCompleted);
                        Dictionary<ISequenceContext, ISequenceProcessor> dictionary = new Dictionary<ISequenceContext, ISequenceProcessor>();
                        Dictionary<IVirtualFolder, IVirtualFolder> FolderMap = new Dictionary<IVirtualFolder, IVirtualFolder>();
                        List<IVirtualFolder> list = new List<IVirtualFolder>();
                        List<IVirtualFolder> list2 = new List<IVirtualFolder>();
                        List<IVirtualItem> SkippedItems = new List<IVirtualItem>();
                        Stack<Tuple<IVirtualFolder, IChangeVirtualItem>> SetTimeFolders = null;
                        if (this.CheckOption(CopyWorkerOptions.CopyFolderTime | CopyWorkerOptions.CopyItemTime))
                        {
                            SetTimeFolders = new Stack<Tuple<IVirtualFolder, IChangeVirtualItem>>();
                        }
                        foreach (IVirtualItem item in folder.GetContent())
                        {
                            if (base.CheckCancellationPending())
                            {
                                return;
                            }
                            bool flag = false;
                            foreach (IVirtualFolder folder2 in list2)
                            {
                                if (folder2.IsChild(item))
                                {
                                    flag = true;
                                    if (!(item is IVirtualFolder))
                                    {
                                        lock (this.FSnapshotLock)
                                        {
                                            this.FSkippedCount++;
                                            if (item.IsPropertyAvailable(3))
                                            {
                                                this.FTotalProcessed.AddProcessedSize(Convert.ToInt64(item[3]));
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            if (flag)
                            {
                                this.RaiseProgress();
                                continue;
                            }
                            IVirtualFolder other = this.FDest;
                            IVirtualFolder folder4 = item as IVirtualFolder;
                            if (folder4 != null)
                            {
                                num = list.Count - 1;
                                while (num >= 0)
                                {
                                    if (list[num].IsChild(folder4))
                                    {
                                        other = FolderMap[list[num]];
                                        break;
                                    }
                                    num--;
                                }
                                if (this.CheckOption(CopyWorkerOptions.SkipEmptyFolders))
                                {
                                    other = null;
                                }
                                if (other != null)
                                {
                                    if (folder4.Equals(other) || folder4.IsChild(other))
                                    {
                                        throw new WarningException(string.Format(Resources.sCannotCopyFolderToItself, folder4.FullName));
                                    }
                                    if (this.CheckOption(CopyWorkerOptions.DeleteSource) && (this.FFilter == null))
                                    {
                                        IChangeVirtualItem item2 = folder4 as IChangeVirtualItem;
                                        if ((item2 != null) && item2.CanMoveTo(other))
                                        {
                                            folder.AsyncWaitHandle.WaitOne();
                                            IVirtualFolder folder5 = (IVirtualFolder) this.MoveItem(item2, other);
                                            if (folder5 != null)
                                            {
                                                list2.Add(folder5);
                                                continue;
                                            }
                                        }
                                    }
                                    other = this.CreateNewFolder(other, folder4);
                                    if (other == null)
                                    {
                                        list2.Add(folder4);
                                        continue;
                                    }
                                    IChangeVirtualItem item3 = other as IChangeVirtualItem;
                                    if ((item3 != null) && (SetTimeFolders != null))
                                    {
                                        SetTimeFolders.Push(Tuple.Create<IVirtualFolder, IChangeVirtualItem>(folder4, item3));
                                    }
                                }
                                list.Add(folder4);
                                FolderMap.Add(folder4, other);
                            }
                            else
                            {
                                ISequenceableItem sequenceableItem = item as ISequenceableItem;
                                if ((sequenceableItem == null) && (item is ISequenceable))
                                {
                                    sequenceableItem = ((ISequenceable) item).SequenceableItem;
                                }
                                if (sequenceableItem != null)
                                {
                                    ISequenceContext sequenceContext = sequenceableItem.SequenceContext;
                                    if (sequenceContext != null)
                                    {
                                        ISequenceProcessor processor;
                                        if (!dictionary.TryGetValue(sequenceContext, out processor))
                                        {
                                            processor = sequenceContext.CreateProcessor(SequenseProcessorType.Extract);
                                            dictionary.Add(sequenceContext, processor);
                                        }
                                        processor.Add(sequenceableItem, item);
                                        continue;
                                    }
                                }
                                if ((item.Parent != null) && FolderMap.ContainsKey(item.Parent))
                                {
                                    other = this.GetDestFolder(item.Parent, FolderMap, SetTimeFolders);
                                }
                                if (other != null)
                                {
                                    this.CopyItem(item, other, null, SkippedItems);
                                    this.RaiseProgress();
                                }
                            }
                        }
                        if (dictionary.Count > 0)
                        {
                            foreach (ISequenceProcessor processor in dictionary.Values)
                            {
                                if (base.CheckCancellationPending())
                                {
                                    return;
                                }
                                try
                                {
                                    ISetOwnerWindow window = processor as ISetOwnerWindow;
                                    if (window != null)
                                    {
                                        window.Owner = this.Owner;
                                    }
                                    if (handler == null)
                                    {
                                        handler = delegate (ProcessItemEventArgs e) {
                                            if (!this.CheckCancellationPending())
                                            {
                                                IVirtualItem sourceItem = (IVirtualItem) e.UserState;
                                                IVirtualFolder destFolder = this.FDest;
                                                if (FolderMap.ContainsKey(sourceItem.Parent))
                                                {
                                                    destFolder = this.GetDestFolder(sourceItem.Parent, FolderMap, SetTimeFolders);
                                                }
                                                this.CopyItem(sourceItem, destFolder, (IGetStream) e, SkippedItems);
                                                this.RaiseProgress();
                                            }
                                            e.Cancel = this.CancellationPending;
                                        };
                                    }
                                    processor.Process(handler);
                                }
                                catch (AbortException)
                                {
                                    base.CancelAsync();
                                }
                            }
                        }
                        if (SetTimeFolders != null)
                        {
                            while (SetTimeFolders.Count > 0)
                            {
                                if (base.CheckCancellationPending())
                                {
                                    return;
                                }
                                Tuple<IVirtualFolder, IChangeVirtualItem> tuple = SetTimeFolders.Pop();
                                this.SetOutFileAttributes(tuple.Item1, tuple.Item2, false, true);
                            }
                        }
                        if (this.CheckOption(CopyWorkerOptions.DeleteSource))
                        {
                            for (num = list.Count - 1; num >= 0; num--)
                            {
                                if (base.CheckCancellationPending())
                                {
                                    return;
                                }
                                bool flag2 = this.CheckOption(CopyWorkerOptions.SkipEmptyFolders) && (FolderMap[list[num]] == null);
                                foreach (IVirtualItem item5 in SkippedItems)
                                {
                                    if (list[num].IsChild(item5))
                                    {
                                        flag2 = true;
                                        break;
                                    }
                                }
                                if (!flag2 && !this.DeleteItem(list[num]))
                                {
                                    SkippedItems.Add(list[num]);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (base.CancellationPending && (this.SearchError != null))
                {
                    throw new ApplicationException(string.Format(Resources.sErrorPopulateCopyItems, this.SearchError.Message), this.SearchError);
                }
            }
        }

        private IVirtualFolder GetDestFolder(IVirtualFolder sourceFolder, Dictionary<IVirtualFolder, IVirtualFolder> folderMap, Stack<Tuple<IVirtualFolder, IChangeVirtualItem>> setTimeFolders)
        {
            IVirtualFolder destFolder = folderMap[sourceFolder];
            if (destFolder == null)
            {
                Stack<IVirtualFolder> stack = new Stack<IVirtualFolder>();
                while (destFolder == null)
                {
                    stack.Push(sourceFolder);
                    sourceFolder = sourceFolder.Parent;
                    if (folderMap.ContainsKey(sourceFolder))
                    {
                        destFolder = folderMap[sourceFolder];
                    }
                    else
                    {
                        destFolder = this.FDest;
                    }
                }
                while (stack.Count > 0)
                {
                    sourceFolder = stack.Pop();
                    destFolder = this.CreateNewFolder(destFolder, sourceFolder);
                    if (destFolder == null)
                    {
                        return null;
                    }
                    IChangeVirtualItem item = destFolder as IChangeVirtualItem;
                    if ((item != null) && (setTimeFolders != null))
                    {
                        setTimeFolders.Push(Tuple.Create<IVirtualFolder, IChangeVirtualItem>(sourceFolder, item));
                    }
                    folderMap[sourceFolder] = destFolder;
                }
            }
            return destFolder;
        }

        public CopyProgressSnapshot GetProgressSnapshot()
        {
            CopyProgressSnapshot snapshot = new CopyProgressSnapshot();
            lock (this.FSnapshotLock)
            {
                snapshot.Processed = this.FTotalProcessed;
                snapshot.TotalCount = this.FTotalCount;
                snapshot.ProcessedCount = this.FProcessedCount;
                snapshot.SkippedCount = this.FSkippedCount;
                snapshot.CopyMode = this.FCopyMode;
            }
            snapshot.Duration = base.Duration;
            return snapshot;
        }

        private CopyItemAction HandleCopyIOException(IVirtualItem item, IVirtualItem source, IVirtualItem dest, Stream Out, byte[] buffer, int count, Exception error)
        {
            AvailableItemActions available = AvailableItemActions.CanUndoDestination | AvailableItemActions.CanRetry;
            CopyItemAction action = this.CopyItemError(item, source, dest, available, error);
            while (action == CopyItemAction.Next)
            {
                try
                {
                    Out.Write(buffer, 0, count);
                    return CopyItemAction.Next;
                }
                catch (IOException exception)
                {
                    error = exception;
                    action = this.CopyItemError(item, source, dest, available, error);
                }
            }
            return action;
        }

        private ChangeItemAction ItemError(EventHandler<ChangeItemErrorEventArgs> handler, IVirtualItem item, AvailableItemActions available, Exception error)
        {
            ChangeItemAction none = ChangeItemAction.None;
            if (handler != null)
            {
                ChangeItemErrorEventArgs e = new ChangeItemErrorEventArgs(item, available, error);
                handler(this, e);
                none = e.Action;
            }
            return none;
        }

        private IVirtualItem MoveItem(IChangeVirtualItem item, IVirtualFolder dest)
        {
            ChangeItemAction action;
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
        Label_0003:;
            try
            {
                return item.MoveTo(dest);
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.MoveItemError(item, canRetryOrElevate, exception);
                canRetryOrElevate &= ~AvailableItemActions.CanElevate;
            }
            catch (Exception exception2)
            {
                action = this.MoveItemError(item, canRetryOrElevate, exception2);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_0003;

                case ChangeItemAction.Skip:
                    return null;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return null;
            }
            throw new InvalidEnumArgumentException();
        }

        private ChangeItemAction MoveItemError(IVirtualItem item, AvailableItemActions availableActions, Exception error)
        {
            return this.ItemError(this.OnMoveItemError, item, availableActions, error);
        }

        private CopyItemAction OpenItemStream(IVirtualItem item, IVirtualItem source, IVirtualItem dest, ThreadStart getStream)
        {
            CopyItemAction next = CopyItemAction.Next;
            AvailableItemActions canRetryOrElevate = AvailableItemActions.CanRetryOrElevate;
            while (next == CopyItemAction.Next)
            {
                try
                {
                    getStream();
                    return next;
                }
                catch (UnauthorizedAccessException exception)
                {
                    next = this.CopyItemError(item, source, dest, canRetryOrElevate, exception);
                    canRetryOrElevate &= ~AvailableItemActions.CanElevate;
                }
                catch (IOException exception2)
                {
                    next = this.CopyItemError(item, source, dest, canRetryOrElevate, exception2);
                }
            }
            return next;
        }

        private void RaiseAfterCopy(IVirtualItem source, IVirtualItem dest)
        {
            if (this.OnAfterCopyItem != null)
            {
                this.OnAfterCopyItem(this, new CopyItemEventArgs(source, dest));
            }
        }

        private bool RaiseFileProgress(ref ProcessedSize processed)
        {
            if (this.FileProgressChanged != null)
            {
                int progressPercent = processed.ProgressPercent;
                if (progressPercent <= 100)
                {
                    ProgressEventArgs e = new ProgressEventArgs(progressPercent);
                    this.FileProgressChanged(this, e);
                    return !e.Cancel;
                }
            }
            return true;
        }

        private void RaiseProgress()
        {
            if (this.CanRaiseProgress)
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
        }

        private void SearchFolderChanged(object sender, VirtualItemChangedEventArgs e)
        {
            if ((e.ChangeType == WatcherChangeTypes.Created) && !(e.Item is IVirtualFolder))
            {
                lock (this.FSnapshotLock)
                {
                    this.FTotalCount++;
                    if (e.Item.IsPropertyAvailable(3))
                    {
                        this.FTotalProcessed.AddTotalSize(Convert.ToInt64(e.Item[3]));
                    }
                }
            }
        }

        private void SearchFolderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.CanRaiseProgress = true;
                this.RaiseProgress();
            }
            else
            {
                this.SearchError = e.Error;
                base.CancelAsync();
            }
        }

        private CopyItemAction SetOutFileAttributes(IVirtualItem sourceItem, IChangeVirtualItem destItem, bool setAttributes, bool setTime)
        {
            ChangeItemAction action;
            Debug.Assert(setAttributes || setTime);
        Label_000F:;
            try
            {
                IUpdateVirtualProperty property = destItem as IUpdateVirtualProperty;
                if (property != null)
                {
                    property.BeginUpdate();
                }
                if (setAttributes)
                {
                    if (destItem.CanSetProperty(6))
                    {
                        FileAttributes normal = FileAttributes.Normal;
                        if (sourceItem.IsPropertyAvailable(6))
                        {
                            normal = sourceItem.Attributes & ~(FileAttributes.Encrypted | FileAttributes.Compressed);
                            if (this.CheckOption(CopyWorkerOptions.ClearROFromCD))
                            {
                                IGetVirtualVolume parent = sourceItem.Parent as IGetVirtualVolume;
                                if ((parent != null) && (parent.VolumeType == DriveType.CDRom))
                                {
                                    normal &= ~FileAttributes.ReadOnly;
                                }
                            }
                        }
                        destItem[6] = normal | (destItem.Attributes & (FileAttributes.Encrypted | FileAttributes.Compressed));
                    }
                    if (this.CheckOption(CopyWorkerOptions.CopyACL))
                    {
                        SetProperty(sourceItem, destItem, 14);
                    }
                }
                if (setTime)
                {
                    SetProperty(sourceItem, destItem, 7);
                    SetProperty(sourceItem, destItem, 9);
                    SetProperty(sourceItem, destItem, 8);
                }
                if (property != null)
                {
                    property.EndUpdate();
                }
                return CopyItemAction.Next;
            }
            catch (UnauthorizedAccessException exception)
            {
                action = this.ChangeItemError(destItem, sourceItem, destItem, AvailableItemActions.CanRetryOrIgnore, exception);
            }
            catch (IOException exception2)
            {
                action = this.ChangeItemError(destItem, sourceItem, destItem, AvailableItemActions.CanRetryOrIgnore, exception2);
            }
            switch (action)
            {
                case ChangeItemAction.Retry:
                    goto Label_000F;

                case ChangeItemAction.Ignore:
                    return CopyItemAction.Next;

                case ChangeItemAction.Skip:
                    return CopyItemAction.Skip;

                case ChangeItemAction.Cancel:
                    base.CancelAsync();
                    return CopyItemAction.Skip;
            }
            throw new InvalidEnumArgumentException();
        }

        private static void SetProperty(IVirtualItem sourceItem, IChangeVirtualItem destItem, int property)
        {
            if (destItem.CanSetProperty(property) && sourceItem.IsPropertyAvailable(property))
            {
                object obj2 = sourceItem[property];
                if (obj2 != null)
                {
                    destItem[property] = obj2;
                }
            }
        }

        private PROGRESS_RESULT SystemCopyProgress(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CALLBACK_REASON dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (dwCallbackReason != CALLBACK_REASON.CALLBACK_STREAM_SWITCH)
            {
                long num = Marshal.ReadInt64(lpData);
                lock (this.FSnapshotLock)
                {
                    this.FTotalProcessed.AddProcessedSize(TotalBytesTransferred - num);
                }
                Marshal.WriteInt64(lpData, TotalBytesTransferred);
                this.RaiseProgress();
            }
            ProcessedSize processed = new ProcessedSize(TotalBytesTransferred, TotalFileSize);
            if (!this.RaiseFileProgress(ref processed))
            {
                return PROGRESS_RESULT.PROGRESS_CANCEL;
            }
            return (base.CheckCancellationPending() ? PROGRESS_RESULT.PROGRESS_CANCEL : PROGRESS_RESULT.PROGRESS_CONTINUE);
        }

        public override string Name
        {
            get
            {
                return "Copy";
            }
        }

        public IWin32Window Owner { get; set; }
    }
}

