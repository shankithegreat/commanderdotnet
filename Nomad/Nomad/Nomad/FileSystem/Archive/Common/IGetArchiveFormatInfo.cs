namespace Nomad.FileSystem.Archive.Common
{
    using System;

    public interface IGetArchiveFormatInfo
    {
        bool RefreshContent();

        ArchiveFormatInfo FormatInfo { get; }
    }
}

