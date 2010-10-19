namespace Nomad.Workers
{
    using Nomad;
    using Nomad.FileSystem.Virtual;
    using System;

    public class CopyItemErrorEventArgs : ChangeItemErrorEventArgs
    {
        public readonly IVirtualItem Dest;
        public readonly IVirtualItem Source;
        public bool UndoDest;

        public CopyItemErrorEventArgs(IVirtualItem item, IVirtualItem source, IVirtualItem dest, AvailableItemActions available, Exception error) : base(item, available, error)
        {
            this.UndoDest = true;
            this.Source = source;
            this.Dest = dest;
        }

        public bool CanUndoDestination
        {
            get
            {
                return ((base.Available & AvailableItemActions.CanUndoDestination) > AvailableItemActions.None);
            }
        }
    }
}

