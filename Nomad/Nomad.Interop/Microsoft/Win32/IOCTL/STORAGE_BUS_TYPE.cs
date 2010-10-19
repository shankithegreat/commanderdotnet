namespace Microsoft.Win32.IOCTL
{
    using System;

    public enum STORAGE_BUS_TYPE
    {
        BusType1394 = 4,
        BusTypeAta = 3,
        BusTypeAtapi = 2,
        BusTypeFibre = 6,
        BusTypeiSCSI = 9,
        BusTypeMaxReserved = 0x7f,
        BusTypeMmc = 13,
        BusTypeRAID = 8,
        BusTypeSas = 10,
        BusTypeSata = 11,
        BusTypeScsi = 1,
        BusTypeSd = 12,
        BusTypeSsa = 5,
        BusTypeUnknown = 0,
        BusTypeUsb = 7
    }
}

