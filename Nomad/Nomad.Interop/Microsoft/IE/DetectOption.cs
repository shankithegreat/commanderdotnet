namespace Microsoft.IE
{
    using System;

    [Flags]
    public enum DetectOption : uint
    {
        Default = 0,
        Source7Bit = 1,
        Source8Bit = 2,
        SourceDBCS = 4,
        SourceHtml = 8,
        TrySimpleDetectFirst = 0x20
    }
}

