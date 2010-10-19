namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;
    using System;

    public class CopyItemEventArgs : EventArgs
    {
        public readonly IVirtualItem Dest;
        public readonly IVirtualItem Source;

        public CopyItemEventArgs(IVirtualItem source, IVirtualItem dest)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (dest == null)
            {
                throw new ArgumentNullException("dest");
            }
            this.Source = source;
            this.Dest = dest;
        }
    }
}

