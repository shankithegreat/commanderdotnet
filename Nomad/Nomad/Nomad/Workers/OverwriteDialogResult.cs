namespace Nomad.Workers
{
    using System;

    public enum OverwriteDialogResult
    {
        None,
        Overwrite,
        Append,
        Resume,
        Rename,
        Skip,
        Abort
    }
}

