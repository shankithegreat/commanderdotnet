namespace Nomad.FileSystem.Virtual
{
    using System;

    public class VirtualItemEventArgs : EventArgs
    {
        public readonly IVirtualItem Item;

        public VirtualItemEventArgs(IVirtualItem item)
        {
            this.Item = item;
        }
    }
}

