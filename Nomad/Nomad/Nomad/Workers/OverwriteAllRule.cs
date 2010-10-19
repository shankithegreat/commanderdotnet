namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("OverwriteAll, {OverwriteResult}")]
    internal class OverwriteAllRule : IOverwriteRule
    {
        private OverwriteDialogResult OverwriteResult;

        public OverwriteAllRule(OverwriteDialogResult result)
        {
            if (((result == OverwriteDialogResult.None) || (result == OverwriteDialogResult.Abort)) || (result == OverwriteDialogResult.Rename))
            {
                throw new ArgumentException();
            }
            this.OverwriteResult = result;
        }

        public OverwriteDialogResult GetOverwrite(IVirtualItem source, IVirtualItem dest)
        {
            return this.OverwriteResult;
        }
    }
}

