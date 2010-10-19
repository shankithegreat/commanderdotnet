namespace Nomad.Workers
{
    using System;

    public class PackProgressSnapshot
    {
        public int CompressionRatio;
        public TimeSpan Duration;
        public Nomad.Workers.ProcessedSize Processed;
        public int ProcessedCount;
        public PackStage Stage;
        public int TotalCount;

        public long ProcessedSize
        {
            get
            {
                return this.Processed.Processed;
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

