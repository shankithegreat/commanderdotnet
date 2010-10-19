namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.ComponentModel;

    public class CancelVirtualItemEventArgs : CancelEventArgs
    {
        public readonly IVirtualItem Item;

        public CancelVirtualItemEventArgs(IVirtualItem item)
        {
            this.Item = item;
        }
    }
}

