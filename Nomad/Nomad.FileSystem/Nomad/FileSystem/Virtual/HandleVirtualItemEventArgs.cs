namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.ComponentModel;

    public class HandleVirtualItemEventArgs : HandledEventArgs
    {
        public readonly IVirtualItem Item;

        public HandleVirtualItemEventArgs(IVirtualItem item)
        {
            this.Item = item;
        }
    }
}

