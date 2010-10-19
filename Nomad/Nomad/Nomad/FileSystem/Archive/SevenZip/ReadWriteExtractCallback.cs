namespace Nomad.FileSystem.Archive.SevenZip
{
    using Nomad.Commons;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;
    using System.Windows.Forms;

    internal class ReadWriteExtractCallback : SevenZipProcessor, IArchiveExtractCallback, ICryptoGetTextPassword, ISetOwnerWindow
    {
        private readonly EventWaitHandle BeforeExtractItem;
        internal readonly EventWaitHandle ConfirmExtractItem;
        internal OperationResult CurrentExtractResult;
        internal SevenZipArchiveItem CurrentItem;
        private AskMode CurrentOperation;
        internal ReadWriteStream CurrentStream;
        private int ErrorCode;
        private EventWaitHandle FinishedEvent;
        private IWin32Window FOwner;
        private EventWaitHandle TerminateEvent;

        internal ReadWriteExtractCallback(SevenZipSharedArchiveContext context) : base(context)
        {
            this.TerminateEvent = new ManualResetEvent(false);
            this.FinishedEvent = new ManualResetEvent(false);
            this.BeforeExtractItem = new AutoResetEvent(false);
            this.ConfirmExtractItem = new AutoResetEvent(false);
        }

        public int CryptoGetTextPassword(out string password)
        {
            SecureString source = base.Context.ArchivePassword[0];
            if (source == null)
            {
                source = ArchiveOpenCallback.GetPassword(this.Owner, base.Context.FileName, base.Context.ArchivePassword);
            }
            if (source != null)
            {
                password = SimpleEncrypt.DecryptString(source);
                return 0;
            }
            password = null;
            return -2147467260;
        }

        protected override void DoProcess()
        {
            WaitHandle[] waitHandles = new WaitHandle[] { this.FinishedEvent, this.BeforeExtractItem };
            ReadWriteItemEventArgs e = new ReadWriteItemEventArgs(this);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessExtract), new List<uint>(base.Items.Keys));
            while (true)
            {
                if (WaitHandle.WaitAny(waitHandles) == 0)
                {
                    this.ThrowExceptionForError();
                    return;
                }
                e.Cancel = false;
                base.ItemHandler(e);
                if (e.Cancel)
                {
                    this.TerminateEvent.Set();
                }
                else if (this.CurrentStream == null)
                {
                    this.ConfirmExtractItem.Set();
                }
            }
        }

        public Stream GetItemStream(SevenZipArchiveItem item)
        {
            base.Items = new Dictionary<uint, SevenZipArchiveItem>();
            base.Items.Add(item.Index, item);
            this.CurrentStream = new ReadWriteStream(this);
            this.ConfirmExtractItem.Set();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessExtract), new List<uint>(base.Items.Keys));
            return this.CurrentStream;
        }

        public int GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
        {
            outStream = null;
            if (askExtractMode == AskMode.kExtract)
            {
                this.CurrentItem = base.Items[index];
                this.BeforeExtractItem.Set();
                if (WaitHandle.WaitAny(new WaitHandle[] { this.ConfirmExtractItem, this.TerminateEvent }) != 0)
                {
                    return -2147467260;
                }
                outStream = this.CurrentStream;
            }
            return 0;
        }

        public void PrepareOperation(AskMode askExtractMode)
        {
            this.CurrentOperation = askExtractMode;
        }

        private void ProcessExtract(object target)
        {
            List<uint> list = (List<uint>) target;
            list.Sort(Comparer<uint>.Default);
            if ((list.Count == 1) && (Convert.ToInt64(base.Items[list[0]][3]) < 0x100000L))
            {
                base.Items[list[0]].CacheAllProperties();
                this.ErrorCode = base.Context.Extract(list.ToArray(), 0, this);
            }
            else
            {
                Stream stream;
                try
                {
                    stream = base.Context.Proxy.Open(base.Context.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch (Exception exception)
                {
                    stream = null;
                    this.ErrorCode = Marshal.GetHRForException(exception);
                }
                if (stream != null)
                {
                    using (stream)
                    {
                        IntPtr pUnk = base.Context.FormatInfo.CreateInArchive();
                        try
                        {
                            IInArchive typedObjectForIUnknown = (IInArchive) Marshal.GetTypedObjectForIUnknown(pUnk, typeof(IInArchive));
                            try
                            {
                                ulong maxCheckStartPosition = 0x20000L;
                                int num2 = typedObjectForIUnknown.Open(new InStreamWrapper(stream), ref maxCheckStartPosition, new ArchiveOpenCallback(this.Owner, base.Context));
                                if (num2 == 0)
                                {
                                    uint[] indices = list.ToArray();
                                    this.ErrorCode = typedObjectForIUnknown.Extract(indices, (uint) indices.Length, 0, this);
                                }
                                else if (num2 != 1)
                                {
                                    this.ErrorCode = num2;
                                }
                            }
                            finally
                            {
                                typedObjectForIUnknown.Close();
                                Marshal.FinalReleaseComObject(typedObjectForIUnknown);
                            }
                        }
                        finally
                        {
                            Marshal.Release(pUnk);
                        }
                    }
                }
            }
            if (this.CurrentStream != null)
            {
                this.CurrentStream.WriteCompleted();
            }
            this.FinishedEvent.Set();
        }

        public void SetCompleted(ref ulong completeValue)
        {
        }

        public void SetOperationResult(OperationResult resultEOperationResult)
        {
            this.CurrentExtractResult = resultEOperationResult;
            if ((this.CurrentOperation == AskMode.kExtract) || (this.CurrentExtractResult != OperationResult.kOK))
            {
                this.CurrentStream.WriteCompleted();
            }
        }

        public void SetTotal(ulong total)
        {
        }

        public void ThrowExceptionForError()
        {
            if (this.ErrorCode == -2147467260)
            {
                throw new AbortException();
            }
            if (this.ErrorCode != 0)
            {
                Marshal.ThrowExceptionForHR(this.ErrorCode);
            }
            string str = string.Empty;
            if ((this.CurrentExtractResult != OperationResult.kOK) && this.CurrentItem.Encrypted)
            {
                str = " (possible wrong password?)";
            }
            switch (this.CurrentExtractResult)
            {
                case OperationResult.kUnSupportedMethod:
                    throw new IOException("Unsupported method");

                case OperationResult.kDataError:
                    throw new IOException("Data error" + str);

                case OperationResult.kCRCError:
                    throw new IOException("CRC error" + str);
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

        private delegate SecureString GetPasswordDelegate(IWin32Window owner, SecureString[] archivePassword);
    }
}

