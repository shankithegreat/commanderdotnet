namespace Nomad.Workers
{
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;

    public class ConfirmArchiveActionArgs : CancelEventArgs
    {
        public readonly string ActionTag;
        public readonly ArchiveFormatInfo ArchiveFormat;
        public readonly IVirtualItem DestArchiveFile;

        public ConfirmArchiveActionArgs(string tag, ArchiveFormatInfo format, IVirtualItem archiveFile)
        {
            this.ActionTag = tag;
            this.ArchiveFormat = format;
            this.DestArchiveFile = archiveFile;
        }
    }
}

