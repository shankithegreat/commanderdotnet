namespace Nomad.FileSystem.Archive.Wcx
{
    using Nomad.FileSystem.Archive.Common;
    using System;

    public class WcxArchiveContext : ISequenceContext
    {
        public readonly string ArchiveName;
        public readonly WcxFormatInfo FormatInfo;

        internal WcxArchiveContext(string archiveName, WcxFormatInfo formatInfo)
        {
            this.ArchiveName = archiveName;
            this.FormatInfo = formatInfo;
        }

        public ISequenceProcessor CreateProcessor(SequenseProcessorType type)
        {
            switch (type)
            {
                case SequenseProcessorType.Extract:
                    return new WcxArchiveProcessor(this, this.FormatInfo.UsePipes);

                case SequenseProcessorType.Delete:
                    if (!this.FormatInfo.CanDeleteFiles)
                    {
                        break;
                    }
                    return new WcxDeleteProcessor(this);
            }
            return null;
        }

        public int ProcessFile(IntPtr hArcData, PK_OPERATION Operation, string destPath, string destName)
        {
            int num;
            try
            {
                num = this.FormatInfo.ProcessFile(hArcData, Operation, destPath, destName);
            }
            catch (Exception exception)
            {
                WcxException exception2 = new WcxException(-1, exception.Message, exception);
                exception2.Data.Add("Archive Format", this.FormatInfo.Name);
                throw exception2;
            }
            return num;
        }

        public int ReadHeader(IntPtr hArcData, ref HeaderDataEx headerEx)
        {
            int num2;
            try
            {
                if (this.FormatInfo.ReadHeaderEx != null)
                {
                    return this.FormatInfo.ReadHeaderEx(hArcData, ref headerEx);
                }
                HeaderData headerData = new HeaderData();
                int num = this.FormatInfo.ReadHeader(hArcData, ref headerData);
                headerEx.ArcName = headerData.ArcName;
                headerEx.FileName = headerData.FileName;
                headerEx.Flags = headerData.Flags;
                headerEx.PackSize = headerData.PackSize;
                headerEx.UnpSize = headerData.UnpSize;
                headerEx.HostOS = headerData.HostOS;
                headerEx.FileCRC = headerData.FileCRC;
                headerEx.FileTime = headerData.FileTime;
                headerEx.UnpVer = headerData.UnpVer;
                headerEx.Method = headerData.Method;
                headerEx.FileAttr = headerData.FileAttr;
                headerEx.CmtBuf = headerData.CmtBuf;
                headerEx.CmtBufSize = headerData.CmtBufSize;
                headerEx.CmtSize = headerData.CmtSize;
                headerEx.CmtState = headerData.CmtState;
                num2 = num;
            }
            catch (Exception exception)
            {
                WcxException exception2 = new WcxException(-1, exception.Message, exception);
                exception2.Data.Add("Archive Format", this.FormatInfo.Name);
                throw exception2;
            }
            return num2;
        }
    }
}

