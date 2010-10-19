namespace TagLib
{
    using System;

    [Flags]
    public enum TagTypes : uint
    {
        AllTags = 0xffffffff,
        Ape = 8,
        Apple = 0x10,
        Asf = 0x20,
        DivX = 0x100,
        FlacMetadata = 0x200,
        Id3v1 = 2,
        Id3v2 = 4,
        MovieId = 0x80,
        None = 0,
        RiffInfo = 0x40,
        Xiph = 1
    }
}

