namespace Nomad.Workers
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessedSize
    {
        private long FProcessedSize;
        private long FTotalSize;
        public ProcessedSize(long processedSize, long totalSize)
        {
            this.FProcessedSize = processedSize;
            this.FTotalSize = totalSize;
        }

        public ProcessedSize(ProcessedSize size)
        {
            this.FProcessedSize = size.Processed;
            this.FTotalSize = size.Total;
        }

        internal void AddProcessedSize(long processedSizeDelta)
        {
            this.FProcessedSize += processedSizeDelta;
        }

        internal void AddTotalSize(long totalSizeDelta)
        {
            this.FTotalSize += totalSizeDelta;
        }

        internal void SetProcessedSize(long processedSize)
        {
            this.FProcessedSize = processedSize;
        }

        internal void SetTotalSize(long totalSize)
        {
            this.FTotalSize = totalSize;
        }

        public long Processed
        {
            get
            {
                return this.FProcessedSize;
            }
        }
        public long Total
        {
            get
            {
                return this.FTotalSize;
            }
        }
        public int ProgressPercent
        {
            get
            {
                return ((this.FTotalSize != 0L) ? ((int) ((this.FProcessedSize * 100L) / this.FTotalSize)) : ((int) 0L));
            }
        }
    }
}

