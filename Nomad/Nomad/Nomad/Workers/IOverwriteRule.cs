namespace Nomad.Workers
{
    using Nomad.FileSystem.Virtual;

    public interface IOverwriteRule
    {
        OverwriteDialogResult GetOverwrite(IVirtualItem source, IVirtualItem dest);
    }
}

