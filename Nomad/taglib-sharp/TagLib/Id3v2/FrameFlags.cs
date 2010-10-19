namespace TagLib.Id3v2
{
    using System;

    [Flags]
    public enum FrameFlags : ushort
    {
        Compression = 8,
        DataLengthIndicator = 1,
        Encryption = 4,
        FileAlterPreservation = 0x2000,
        GroupingIdentity = 0x40,
        None = 0,
        ReadOnly = 0x1000,
        TagAlterPreservation = 0x4000,
        Unsynchronisation = 2
    }
}

