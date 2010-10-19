namespace Nomad.Workers
{
    using System;

    [Flags]
    public enum CompareFoldersOptions
    {
        AutoCompareContentAsync = 0x20,
        CompareAttributes = 1,
        CompareContent = 8,
        CompareContentAsync = 0x10,
        CompareLastWriteTime = 2,
        CompareSize = 4
    }
}

