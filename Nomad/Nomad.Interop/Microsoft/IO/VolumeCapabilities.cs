namespace Microsoft.IO
{
    using System;

    [Flags]
    public enum VolumeCapabilities : uint
    {
        FileCasePreservedNames = 2,
        FileCaseSensitiveSearch = 1,
        FileCompression = 0x10,
        FileNamedStreams = 0x40000,
        FilePersistentAcls = 8,
        FileSequentalWriteOnce = 0x100000,
        FileSupportsEncryption = 0x20000,
        FileSupportsObjectIds = 0x10000,
        FileSupportTransactions = 0x200000,
        FileUnicodeOnDisk = 4,
        FileVolumeQuotas = 0x20,
        ReadOnlyVolume = 0x80000,
        RemoteStorage = 0x100,
        ReparsePoints = 0x80,
        SparseFiles = 0x40,
        VolumeIsCompressed = 0x8000
    }
}

