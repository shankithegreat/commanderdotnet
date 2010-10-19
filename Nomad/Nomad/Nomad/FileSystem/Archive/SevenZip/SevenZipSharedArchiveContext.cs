namespace Nomad.FileSystem.Archive.SevenZip
{
    using Microsoft;
    using Microsoft.Win32;
    using Nomad.Commons.IO;
    using Nomad.Commons.Plugin;
    using Nomad.FileSystem.Archive.Common;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.Remoting;
    using System.Security;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;

    public class SevenZipSharedArchiveContext : IDisposable, ISequenceContext, IElevatable
    {
        public readonly object ArchiveLock;
        public readonly SecureString[] ArchivePassword;
        private Nomad.FileSystem.Archive.SevenZip.StreamWrapper ArchiveStreamWrapper;
        private IInArchive FArchive;
        private IntPtr FArchivePtr;
        private int FArchiveThreadId;
        public readonly SevenZipFormatInfo FormatInfo;
        private IFileProxy FProxy;

        public SevenZipSharedArchiveContext(IntPtr archivePtr, SecureString[] archivePassword)
        {
            this.FArchivePtr = archivePtr;
            this.ArchiveLock = new object();
            this.ArchivePassword = archivePassword;
        }

        public SevenZipSharedArchiveContext(IntPtr archivePtr, SevenZipFormatInfo formatInfo, Nomad.FileSystem.Archive.SevenZip.StreamWrapper wrapper)
        {
            this.FArchivePtr = archivePtr;
            this.ArchiveLock = new object();
            this.ArchivePassword = new SecureString[1];
            this.FormatInfo = formatInfo;
            this.ArchiveStreamWrapper = wrapper;
        }

        public ISequenceProcessor CreateProcessor(SequenseProcessorType type)
        {
            switch (type)
            {
                case SequenseProcessorType.Extract:
                    return new ReadWriteExtractCallback(this);

                case SequenseProcessorType.Delete:
                    if ((this.FormatInfo.Capabilities & ArchiveFormatCapabilities.UpdateArchive) <= 0)
                    {
                        break;
                    }
                    return new SevenZipDeleteProcessor(this);
            }
            return null;
        }

        public void Dispose()
        {
            if (this.FArchivePtr != IntPtr.Zero)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected void Dispose(bool disposing)
        {
            if (this.FArchive != null)
            {
                if (disposing)
                {
                    this.Archive.Close();
                }
                Marshal.FinalReleaseComObject(this.FArchive);
                this.FArchive = null;
            }
            if (this.FArchivePtr != IntPtr.Zero)
            {
                Marshal.Release(this.FArchivePtr);
                this.FArchivePtr = IntPtr.Zero;
            }
            if (this.ArchiveStreamWrapper != null)
            {
                this.ArchiveStreamWrapper.Dispose();
                this.ArchiveStreamWrapper = null;
            }
        }

        public bool Elevate(IPluginProcess process)
        {
            if (process == null)
            {
                throw new ArgumentNullException();
            }
            if (RemotingServices.IsTransparentProxy(this.Proxy))
            {
                return false;
            }
            IPluginActivator activator = process as IPluginActivator;
            if (activator == null)
            {
                return false;
            }
            if (!(process.IsAlive || process.Start()))
            {
                return false;
            }
            this.FProxy = activator.Create<IFileProxy>("filesystemproxy");
            return true;
        }

        public void EnterArchiveLock()
        {
            if (!Monitor.TryEnter(this.ArchiveLock, 0x7d0))
            {
                throw new TimeoutException("Attempt to lock thread for archive access has timed out.");
            }
        }

        public int Extract(uint[] indices, int testMode, IArchiveExtractCallback extractCallback)
        {
            int num;
            this.EnterArchiveLock();
            try
            {
                num = this.Archive.Extract(indices, (uint) indices.Length, testMode, extractCallback);
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
            return num;
        }

        ~SevenZipSharedArchiveContext()
        {
            this.Dispose(false);
        }

        public void Flush()
        {
            InStreamTimedWrapper archiveStreamWrapper = this.ArchiveStreamWrapper as InStreamTimedWrapper;
            if (archiveStreamWrapper != null)
            {
                archiveStreamWrapper.Flush();
            }
        }

        public uint GetNumberOfItems()
        {
            uint numberOfItems;
            this.EnterArchiveLock();
            try
            {
                numberOfItems = this.Archive.GetNumberOfItems();
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
            return numberOfItems;
        }

        public uint GetNumberOfProperties()
        {
            uint numberOfProperties;
            this.EnterArchiveLock();
            try
            {
                numberOfProperties = this.Archive.GetNumberOfProperties();
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
            return numberOfProperties;
        }

        public void GetProperty(uint index, ItemPropId propID, ref PropVariant value)
        {
            this.EnterArchiveLock();
            try
            {
                this.Archive.GetProperty(index, propID, ref value);
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
        }

        public void GetPropertyInfo(uint index, out string name, out ItemPropId propID, out ushort varType)
        {
            this.EnterArchiveLock();
            try
            {
                this.Archive.GetPropertyInfo(index, out name, out propID, out varType);
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
        }

        public int Open(IWin32Window owner)
        {
            IInStream archiveStreamWrapper = this.ArchiveStreamWrapper as IInStream;
            if (archiveStreamWrapper == null)
            {
                throw new InvalidOperationException();
            }
            return this.Open(owner, archiveStreamWrapper);
        }

        public int Open(IWin32Window owner, IInStream stream)
        {
            return this.Open(stream, 0x20000L, new ArchiveOpenCallback(owner, this));
        }

        public int Open(IInStream stream, ulong maxCheckStartPosition, IArchiveOpenCallback openArchiveCallback)
        {
            int num2;
            this.EnterArchiveLock();
            try
            {
                int num = this.Archive.Open(stream, ref maxCheckStartPosition, openArchiveCallback);
                this.Flush();
                num2 = num;
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
            return num2;
        }

        public bool Reopen()
        {
            bool flag;
            IInStream archiveStreamWrapper = this.ArchiveStreamWrapper as IInStream;
            if (archiveStreamWrapper == null)
            {
                throw new InvalidOperationException();
            }
            this.EnterArchiveLock();
            try
            {
                this.Archive.Close();
                archiveStreamWrapper.Seek(0L, 0, IntPtr.Zero);
                flag = HRESULT.SUCCEEDED(this.Open(null, archiveStreamWrapper));
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
            return flag;
        }

        public void UpdateItems(ISequentialOutStream outStream, int numItems, IArchiveUpdateCallback updateCallback)
        {
            this.EnterArchiveLock();
            try
            {
                if (this.FArchive != null)
                {
                    Marshal.FinalReleaseComObject(this.FArchive);
                }
                IOutArchive typedObjectForIUnknown = (IOutArchive) Marshal.GetTypedObjectForIUnknown(this.FArchivePtr, typeof(IOutArchive));
                if (typedObjectForIUnknown == null)
                {
                    throw new NotSupportedException();
                }
                try
                {
                    typedObjectForIUnknown.UpdateItems(outStream, numItems, updateCallback);
                }
                finally
                {
                    Marshal.FinalReleaseComObject(typedObjectForIUnknown);
                }
            }
            finally
            {
                Monitor.Exit(this.ArchiveLock);
            }
        }

        private IInArchive Archive
        {
            get
            {
                if (this.FArchivePtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("SharedArchiveContext");
                }
                if (this.FArchive == null)
                {
                    this.FArchive = (IInArchive) Marshal.GetTypedObjectForIUnknown(this.FArchivePtr, typeof(IInArchive));
                    this.FArchiveThreadId = Thread.CurrentThread.ManagedThreadId;
                }
                else if (this.FArchiveThreadId != Thread.CurrentThread.ManagedThreadId)
                {
                    Marshal.FinalReleaseComObject(this.FArchive);
                    this.FArchive = (IInArchive) Marshal.GetTypedObjectForIUnknown(this.FArchivePtr, typeof(IInArchive));
                    this.FArchiveThreadId = Thread.CurrentThread.ManagedThreadId;
                }
                return this.FArchive;
            }
        }

        public bool CanElevate
        {
            get
            {
                if (!(OS.IsWinVista && !RemotingServices.IsTransparentProxy(this.Proxy)))
                {
                    return false;
                }
                switch (OS.ElevationType)
                {
                    case ElevationType.Full:
                        return false;

                    case ElevationType.Limited:
                        return true;
                }
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return !principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public string FileName
        {
            get
            {
                InStreamTimedWrapper archiveStreamWrapper = this.ArchiveStreamWrapper as InStreamTimedWrapper;
                if (!((archiveStreamWrapper == null) || string.IsNullOrEmpty(archiveStreamWrapper.BaseStreamFileName)))
                {
                    return archiveStreamWrapper.BaseStreamFileName;
                }
                FileStream baseStream = this.ArchiveStreamWrapper.BaseStream as FileStream;
                if (baseStream == null)
                {
                    throw new NotSupportedException();
                }
                return baseStream.Name;
            }
        }

        public IFileProxy Proxy
        {
            get
            {
                return (this.FProxy ?? FileSystemProxy.Default);
            }
        }
    }
}

