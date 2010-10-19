namespace Nomad.Commons
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class CustomContentFilter : BasicFilter, IChangeProgress
    {
        public event EventHandler<ProgressEventArgs> Progress;

        protected CustomContentFilter()
        {
        }

        public abstract bool MatchStream(Stream contentStream, string fileName);
        protected void OnProgress(ProgressEventArgs e)
        {
            if (this.Progress != null)
            {
                this.Progress(this, e);
            }
        }

        protected bool OnProgress(int progressPercentage)
        {
            if (this.Progress != null)
            {
                ProgressEventArgs e = new ProgressEventArgs(progressPercentage);
                this.Progress(this, e);
                return !e.Cancel;
            }
            return true;
        }
    }
}

