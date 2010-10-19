namespace TagLib.Id3v2
{
    using System;

    [Flags]
    public enum HeaderFlags : byte
    {
        ExperimentalIndicator = 0x20,
        ExtendedHeader = 0x40,
        FooterPresent = 0x10,
        None = 0,
        Unsynchronisation = 0x80
    }
}

