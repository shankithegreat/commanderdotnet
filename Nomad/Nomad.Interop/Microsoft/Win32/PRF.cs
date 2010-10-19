namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum PRF
    {
        PRF_CHECKVISIBLE = 1,
        PRF_CHILDREN = 0x10,
        PRF_CLIENT = 4,
        PRF_ERASEBKGND = 8,
        PRF_NONCLIENT = 2,
        PRF_OWNED = 0x20
    }
}

