namespace Nomad.Commons
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ProgressEventArgs : CancelEventArgs
    {
        public ProgressEventArgs(int progressPercentage)
        {
            if ((progressPercentage < 0) || (progressPercentage > 100))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.ProgressPercentage = progressPercentage;
        }

        public ProgressEventArgs(int progressCurrent, int progressMaximum)
        {
            if (progressCurrent < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (progressMaximum < progressCurrent)
            {
                throw new ArgumentException();
            }
            this.ProgressPercentage = (progressCurrent * 100) / progressMaximum;
        }

        public int ProgressPercentage { get; private set; }
    }
}

