namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;

    [Flags]
    public enum SevenZipFormatCapabilities
    {
        AppendExt = 0x20,
        EncryptFileNames = 8,
        Internal = 0x10,
        MultiThread = 2,
        SFX = 4,
        Solid = 1
    }
}

