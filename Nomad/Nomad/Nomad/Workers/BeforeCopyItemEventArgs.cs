namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;
    using System;

    public class BeforeCopyItemEventArgs : CopyItemEventArgs
    {
        public string NewName;
        public OverwriteDialogResult OverwriteResult;

        public BeforeCopyItemEventArgs(IVirtualItem source, IVirtualItem dest) : base(source, dest)
        {
            this.NewName = string.Empty;
            this.OverwriteResult = OverwriteDialogResult.Overwrite;
        }
    }
}

