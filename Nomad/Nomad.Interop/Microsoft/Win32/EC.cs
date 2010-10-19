namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum EC : ushort
    {
        EC_LEFTMARGIN = 1,
        EC_RIGHTMARGIN = 2,
        EC_USEFONTINFO = 0xffff
    }
}

