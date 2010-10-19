namespace Microsoft.Win32
{
    using System;

    public enum CDDS
    {
        CDDS_ITEM = 0x10000,
        CDDS_ITEMPOSTERASE = 0x10004,
        CDDS_ITEMPOSTPAINT = 0x10002,
        CDDS_ITEMPREERASE = 0x10003,
        CDDS_ITEMPREPAINT = 0x10001,
        CDDS_POSTERASE = 4,
        CDDS_POSTPAINT = 2,
        CDDS_PREERASE = 3,
        CDDS_PREPAINT = 1,
        CDDS_SUBITEM = 0x20000
    }
}

