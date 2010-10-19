namespace Nomad.FileSystem.Virtual
{
    using System;

    public class SearchErrorEventArgs : VirtualItemEventArgs
    {
        public bool Continue;
        public readonly Exception Error;

        public SearchErrorEventArgs(IVirtualItem item, Exception error) : base(item)
        {
            this.Error = error;
        }
    }
}

