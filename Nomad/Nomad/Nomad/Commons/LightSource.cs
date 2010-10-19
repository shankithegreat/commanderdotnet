namespace Nomad.Commons
{
    using System;

    public enum LightSource : ushort
    {
        D55 = 20,
        D65 = 0x15,
        D75 = 0x16,
        Daylight = 1,
        Fluorescent = 2,
        Other = 0xff,
        Reserved = 0xfe,
        StandardLightA = 0x11,
        StandardLightB = 0x12,
        StandardLightC = 0x13,
        Tungsten = 3,
        Unknown = 0
    }
}

