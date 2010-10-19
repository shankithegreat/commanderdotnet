namespace Nomad.FileSystem.Archive.Wcx
{
    using Microsoft;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.IO;
    using System.Reflection;

    public class WcxArchiveItem : CustomPropertyProvider, ISimpleItem, IGetVirtualProperty, ISequenceableItem, IGetStream
    {
        private FileAttributes Attributes;
        private long CompressedSize;
        private WcxArchiveContext Context;
        private int CRC;
        private string FName;
        public readonly int Index;
        private DateTime? LastWriteTime;
        private int Method;
        private long Size;

        internal WcxArchiveItem(WcxArchiveContext context, HeaderDataEx header, int index)
        {
            this.Context = context;
            this.Index = index;
            this.FName = header.FileName;
            this.Size = header.UnpSize | (header.UnpSizeHigh << 0x20);
            this.CompressedSize = header.PackSize | (header.UnpSizeHigh << 0x20);
            this.Attributes = header.FileAttr;
            this.CRC = header.FileCRC;
            this.Method = header.Method;
            this.InitializeArchiveItem(header.FileTime);
        }

        protected override VirtualPropertySet CreateAvailableSet()
        {
            int[] properties = new int[3];
            properties[1] = 6;
            properties[2] = 20;
            VirtualPropertySet set = new VirtualPropertySet(properties);
            if ((this.Size > 0L) || ((this.Size == 0L) && (this.CompressedSize == 0L)))
            {
                set[3] = true;
            }
            if ((this.CompressedSize > 0L) || ((this.Size == 0L) && (this.CompressedSize == 0L)))
            {
                set[5] = true;
            }
            if (this.CRC > 0)
            {
                set[0x18] = true;
            }
            if (this.LastWriteTime.HasValue)
            {
                set[8] = true;
            }
            return set;
        }

        public Stream GetStream()
        {
            return new ExtractSingleItemStream(this.Context, this.Index);
        }

        private void InitializeArchiveItem(int fileTime)
        {
            this.LastWriteTime = null;
            if (fileTime > 0)
            {
                try
                {
                    this.LastWriteTime = new DateTime((fileTime >> 0x19) + 0x7bc, (fileTime >> 0x15) & 15, (fileTime >> 0x10) & 0x1f, (fileTime >> 11) & 0x1f, (fileTime >> 5) & 0x3f, (fileTime & 0x1f) << 1);
                }
                catch (ArgumentException)
                {
                    this.LastWriteTime = null;
                }
            }
        }

        public object this[int property]
        {
            get
            {
                switch (property)
                {
                    case 0:
                        return this.FName;

                    case 3:
                        return this.Size;

                    case 5:
                        return this.CompressedSize;

                    case 6:
                        return this.Attributes;

                    case 8:
                        return this.LastWriteTime;

                    case 20:
                        return this.Method.ToString();

                    case 0x18:
                        return ((this.CRC > 0) ? this.CRC : 0);
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
        }

        public ISequenceContext SequenceContext
        {
            get
            {
                return this.Context;
            }
        }

        private class ExtractSingleItemStream : CustomStreamWrapper
        {
            private WcxArchiveContext Context;
            private FileStream FBaseStream;
            private int Index;

            internal ExtractSingleItemStream(WcxArchiveContext context, int index)
            {
                this.Context = context;
                this.Index = index;
            }

            public override void Close()
            {
                if (this.FBaseStream != null)
                {
                    this.FBaseStream.Close();
                }
            }

            private int ProcessData(string FileName, int Size)
            {
                return 1;
            }

            protected override Stream BaseStream
            {
                get
                {
                    if (this.FBaseStream == null)
                    {
                        OpenArchiveData archiveData = new OpenArchiveData {
                            ArcName = this.Context.ArchiveName,
                            OpenMode = PK_OM.PK_OM_EXTRACT
                        };
                        IntPtr hArcData = this.Context.FormatInfo.OpenArchive(ref archiveData);
                        int errorCode = (hArcData == IntPtr.Zero) ? 15 : archiveData.OpenResult;
                        if (errorCode == 0)
                        {
                            try
                            {
                                if (this.Context.FormatInfo.SetProcessData != null)
                                {
                                    ProcessDataProcCallback pProcessDataProc = new ProcessDataProcCallback(this.ProcessData);
                                    this.Context.FormatInfo.SetProcessData(hArcData, pProcessDataProc);
                                }
                                int num2 = 0;
                                HeaderDataEx headerEx = new HeaderDataEx();
                                while ((errorCode == 0) && ((errorCode = this.Context.ReadHeader(hArcData, ref headerEx)) == 0))
                                {
                                    if (num2 == this.Index)
                                    {
                                        string destName = Path.Combine(OS.TempDirectory, StringHelper.GuidToCompactString(Guid.NewGuid()));
                                        errorCode = this.Context.ProcessFile(hArcData, PK_OPERATION.PK_EXTRACT, null, destName);
                                        if (errorCode == 0)
                                        {
                                            this.FBaseStream = new FileStream(destName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x8000, FileOptions.DeleteOnClose);
                                        }
                                        break;
                                    }
                                    errorCode = this.Context.ProcessFile(hArcData, PK_OPERATION.PK_SKIP, null, null);
                                    num2++;
                                }
                                if (errorCode == 10)
                                {
                                    errorCode = 0x16;
                                }
                            }
                            finally
                            {
                                this.Context.FormatInfo.CloseArchive(hArcData);
                            }
                        }
                        if (errorCode != 0)
                        {
                            WcxErrors.ThrowExceptionForError(errorCode);
                        }
                    }
                    return this.FBaseStream;
                }
            }
        }
    }
}

