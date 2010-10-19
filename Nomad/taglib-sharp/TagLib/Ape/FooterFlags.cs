namespace TagLib.Ape
{
    using System;

    [Flags]
    public enum FooterFlags : uint
    {
        FooterAbsent = 0x40000000,
        HeaderPresent = 0x80000000,
        IsHeader = 0x20000000
    }
}

