namespace Nomad.FileSystem.Archive.Common
{
    using System;

    [Flags]
    public enum ArchiveFormatCapabilities
    {
        CreateArchive = 2,
        DetectFormatByContent = 1,
        EncryptContent = 0x10,
        MultiFileArchive = 8,
        UpdateArchive = 4
    }
}

