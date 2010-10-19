namespace Microsoft.IE.MLang
{
    using System;

    public enum MLCONVCHAR : uint
    {
        MLCONVCHARF_AUTODETECT = 1,
        MLCONVCHARF_NAME_ENTITIZE = 4,
        MLCONVCHARF_NCR_ENTITIZE = 2,
        MLCONVCHARF_NOBESTFITCHARS = 0x10,
        MLCONVCHARF_NONE = 0,
        MLCONVCHARF_USEDEFCHAR = 8
    }
}

