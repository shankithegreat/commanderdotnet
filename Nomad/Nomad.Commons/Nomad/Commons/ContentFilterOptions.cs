namespace Nomad.Commons
{
    using System;

    [Flags]
    public enum ContentFilterOptions
    {
        CaseSensitive = 2,
        DetectEncoding = 0x20,
        Regex = 1,
        SpaceCompress = 8,
        UseIFilter = 0x10,
        WholeWords = 4
    }
}

