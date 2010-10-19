namespace Microsoft.COM
{
    using System;

    [Flags]
    public enum DVASPECT2 : uint
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_DOCPRINT = 8,
        DVASPECT_ICON = 4,
        DVASPECT_OPAQUE = 0x10,
        DVASPECT_THUMBNAIL = 2,
        DVASPECT_TRANSPARENT = 0x20
    }
}

