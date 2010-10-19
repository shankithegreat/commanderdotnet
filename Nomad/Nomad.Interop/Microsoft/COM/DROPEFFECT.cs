namespace Microsoft.COM
{
    using System;

    [Flags]
    public enum DROPEFFECT : uint
    {
        DROPEFFECT_COPY = 1,
        DROPEFFECT_LINK = 4,
        DROPEFFECT_MOVE = 2,
        DROPEFFECT_NONE = 0,
        DROPEFFECT_SCROLL = 0x80000000
    }
}

