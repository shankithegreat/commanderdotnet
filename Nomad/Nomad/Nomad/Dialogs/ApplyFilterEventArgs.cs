namespace Nomad.Dialogs
{
    using Nomad.FileSystem.Virtual.Filter;
    using System;

    public class ApplyFilterEventArgs : EventArgs
    {
        public readonly IVirtualItemFilter Filter;

        public ApplyFilterEventArgs(IVirtualItemFilter filter)
        {
            this.Filter = filter;
        }
    }
}

