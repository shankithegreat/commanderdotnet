namespace Nomad.FileSystem.Archive.Wcx
{
    using Microsoft;
    using Microsoft.IO.Pipes;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.IO;
    using System.Threading;

    internal class WcxArchiveProcessor : WcxProcessor
    {
        private IntPtr ArchiveHandle;
        private readonly EventWaitHandle BeforeExtractItem;
        private readonly EventWaitHandle ConfirmExtractItem;
        private int CurrentItemIndex;
        private bool ExtractNeeded;
        private EventWaitHandle FinishedEvent;
        private string PipeName;
        private int ProcessResult;
        private string TempFileName;
        private EventWaitHandle TerminateEvent;
        private bool UsePipes;

        internal WcxArchiveProcessor(WcxArchiveContext context, bool usePipes) : base(context)
        {
            this.PipeName = StringHelper.GuidToCompactString(Guid.NewGuid());
            this.TerminateEvent = new ManualResetEvent(false);
            this.FinishedEvent = new ManualResetEvent(false);
            this.BeforeExtractItem = new AutoResetEvent(false);
            this.ConfirmExtractItem = new AutoResetEvent(false);
            this.TempFileName = Path.Combine(OS.TempDirectory, this.PipeName);
            this.UsePipes = usePipes;
        }

        protected override void DoProcess(ProcessItemHandler handler)
        {
            if (!this.UsePipes)
            {
                this.ProcessExtract(handler);
            }
            else
            {
                WaitHandle[] waitHandles = new WaitHandle[] { this.FinishedEvent, this.BeforeExtractItem };
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessExtract));
                AsyncProcessWcxItemEventArgs e = new AsyncProcessWcxItemEventArgs(this);
                while (true)
                {
                    if (WaitHandle.WaitAny(waitHandles) == 0)
                    {
                        break;
                    }
                    this.ExtractNeeded = false;
                    handler(e);
                    if (e.Cancel)
                    {
                        this.TerminateEvent.Set();
                    }
                    else if (!this.ExtractNeeded)
                    {
                        this.ConfirmExtractItem.Set();
                    }
                }
            }
            if (this.ProcessResult != 0)
            {
                WcxErrors.ThrowExceptionForError(this.ProcessResult);
            }
        }

        private int ProcessData(string FileName, int Size)
        {
            return 1;
        }

        private void ProcessExtract(object target)
        {
            try
            {
                OpenArchiveData archiveData = new OpenArchiveData {
                    ArcName = base.Context.ArchiveName,
                    OpenMode = PK_OM.PK_OM_EXTRACT
                };
                this.ArchiveHandle = base.Context.FormatInfo.OpenArchive(ref archiveData);
                this.ProcessResult = (this.ArchiveHandle == IntPtr.Zero) ? 15 : archiveData.OpenResult;
                if (this.ProcessResult == 0)
                {
                    try
                    {
                        if (base.Context.FormatInfo.SetProcessData != null)
                        {
                            ProcessDataProcCallback pProcessDataProc = new ProcessDataProcCallback(this.ProcessData);
                            base.Context.FormatInfo.SetProcessData(this.ArchiveHandle, pProcessDataProc);
                        }
                        int num = 0;
                        HeaderDataEx headerEx = new HeaderDataEx();
                        while (((num < base.Items.Count) && (this.ProcessResult == 0)) && ((this.ProcessResult = base.Context.ReadHeader(this.ArchiveHandle, ref headerEx)) == 0))
                        {
                            if (this.UsePipes)
                            {
                                if (base.Items.ContainsKey(this.CurrentItemIndex))
                                {
                                    num++;
                                    this.BeforeExtractItem.Set();
                                    if (WaitHandle.WaitAny(new WaitHandle[] { this.TerminateEvent, this.ConfirmExtractItem }) == 0)
                                    {
                                        break;
                                    }
                                }
                                if (this.ExtractNeeded)
                                {
                                    this.ProcessResult = base.Context.ProcessFile(this.ArchiveHandle, PK_OPERATION.PK_EXTRACT, null, @"\\.\pipe\" + this.PipeName);
                                }
                                else
                                {
                                    this.ProcessResult = base.Context.ProcessFile(this.ArchiveHandle, PK_OPERATION.PK_SKIP, null, null);
                                }
                            }
                            else
                            {
                                this.ExtractNeeded = false;
                                if (base.Items.ContainsKey(this.CurrentItemIndex))
                                {
                                    num++;
                                    ProcessWcxItemEventArgs e = new ProcessWcxItemEventArgs(this);
                                    ((ProcessItemHandler) target)(e);
                                    if (e.Cancel)
                                    {
                                        break;
                                    }
                                }
                                if (!this.ExtractNeeded)
                                {
                                    this.ProcessResult = base.Context.ProcessFile(this.ArchiveHandle, PK_OPERATION.PK_SKIP, null, null);
                                }
                            }
                            this.CurrentItemIndex++;
                        }
                        if (this.ProcessResult == 10)
                        {
                            this.ProcessResult = 0;
                        }
                    }
                    finally
                    {
                        base.Context.FormatInfo.CloseArchive(this.ArchiveHandle);
                    }
                }
            }
            finally
            {
                this.FinishedEvent.Set();
            }
        }

        private class AsyncProcessWcxItemEventArgs : WcxArchiveProcessor.CustomProcessWcxItemEventArgs, IGetStream
        {
            internal AsyncProcessWcxItemEventArgs(WcxArchiveProcessor processor) : base(processor)
            {
            }

            public Stream GetStream()
            {
                Stream stream2;
                NamedPipeStream stream = NamedPipeStream.Create(base.Processor.PipeName, NamedPipeStream.ServerMode.Inbound, true);
                try
                {
                    base.Processor.ExtractNeeded = true;
                    IAsyncResult asyncResult = stream.BeginListen();
                    base.Processor.ConfirmExtractItem.Set();
                    if (WaitHandle.WaitAny(new WaitHandle[] { asyncResult.AsyncWaitHandle, base.Processor.FinishedEvent }) != 0)
                    {
                        throw new AbortException();
                    }
                    if (!stream.EndListen(asyncResult))
                    {
                        throw new AbortException();
                    }
                    stream2 = stream;
                }
                catch
                {
                    stream.Close();
                    base.Processor.TerminateEvent.Set();
                    throw;
                }
                return stream2;
            }
        }

        private abstract class CustomProcessWcxItemEventArgs : ProcessItemEventArgs
        {
            protected readonly WcxArchiveProcessor Processor;

            internal CustomProcessWcxItemEventArgs(WcxArchiveProcessor processor)
            {
                this.Processor = processor;
            }

            public override ISequenceableItem Item
            {
                get
                {
                    return this.Processor.Items[this.Processor.CurrentItemIndex];
                }
            }

            public override object UserState
            {
                get
                {
                    return this.Processor.GetUserState(this.Item);
                }
            }
        }

        private class ExtractCurrentItemStream : CustomStreamWrapper
        {
            private FileStream FBaseStream;
            private WcxArchiveProcessor Processor;

            internal ExtractCurrentItemStream(WcxArchiveProcessor processor)
            {
                this.Processor = processor;
            }

            protected override Stream BaseStream
            {
                get
                {
                    if (this.FBaseStream == null)
                    {
                        this.Processor.ProcessResult = this.Processor.Context.ProcessFile(this.Processor.ArchiveHandle, PK_OPERATION.PK_EXTRACT, null, this.Processor.TempFileName);
                        if (this.Processor.ProcessResult != 0)
                        {
                            throw new AbortException();
                        }
                        this.Processor.ExtractNeeded = true;
                        this.FBaseStream = new FileStream(this.Processor.TempFileName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x8000, FileOptions.DeleteOnClose);
                    }
                    return this.FBaseStream;
                }
            }
        }

        private class ProcessWcxItemEventArgs : WcxArchiveProcessor.CustomProcessWcxItemEventArgs, IGetStream
        {
            internal ProcessWcxItemEventArgs(WcxArchiveProcessor processor) : base(processor)
            {
            }

            public Stream GetStream()
            {
                return new WcxArchiveProcessor.ExtractCurrentItemStream(base.Processor);
            }
        }
    }
}

