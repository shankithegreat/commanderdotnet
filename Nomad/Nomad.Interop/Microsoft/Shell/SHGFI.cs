namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SHGFI : uint
    {
        SHGFI_ADDOVERLAYS = 0x20,
        SHGFI_ATTR_SPECIFIED = 0x20000,
        SHGFI_ATTRIBUTES = 0x800,
        SHGFI_DISPLAYNAME = 0x200,
        SHGFI_EXETYPE = 0x2000,
        SHGFI_ICON = 0x100,
        SHGFI_ICONLOCATION = 0x1000,
        SHGFI_LARGEICON = 0,
        SHGFI_LINKOVERLAY = 0x8000,
        SHGFI_OPENICON = 2,
        SHGFI_OVERLAYINDEX = 0x40,
        SHGFI_PIDL = 8,
        SHGFI_SELECTED = 0x10000,
        SHGFI_SHELLICONSIZE = 4,
        SHGFI_SMALLICON = 1,
        SHGFI_SYSICONINDEX = 0x4000,
        SHGFI_TYPENAME = 0x400,
        SHGFI_USEFILEATTRIBUTES = 0x10
    }
}

