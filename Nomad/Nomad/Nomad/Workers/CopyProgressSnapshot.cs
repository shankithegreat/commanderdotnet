namespace Nomad.Workers
{
    using System;

    public class CopyProgressSnapshot
    {
        public Nomad.Workers.CopyMode CopyMode;
        public TimeSpan Duration;
        public Nomad.Workers.ProcessedSize Processed;
        public int ProcessedCount;
        public int SkippedCount;
        public int TotalCount;

        public long ProcessedSize
        {
            get
            {
                return this.Processed.Processed;
            }
        }

        public int TotalProcessedCount
        {
            get
            {
                return (this.ProcessedCount + this.SkippedCount);
            }
        }

        public long TotalSize
        {
            get
            {
                return this.Processed.Total;
            }
        }
    }
}

