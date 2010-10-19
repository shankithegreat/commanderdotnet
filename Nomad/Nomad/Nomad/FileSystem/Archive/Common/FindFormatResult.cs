namespace Nomad.FileSystem.Archive.Common
{
    using System;

    public class FindFormatResult
    {
        public FindFormatSource FindSource;
        public readonly ArchiveFormatInfo Format;
        public readonly int Offset;

        public FindFormatResult(ArchiveFormatInfo format, FindFormatSource source)
        {
            this.Format = format;
            this.Offset = -1;
            this.FindSource = source;
        }

        public FindFormatResult(ArchiveFormatInfo format, int offset)
        {
            this.Format = format;
            this.Offset = offset;
            this.FindSource = FindFormatSource.Content;
        }
    }
}

