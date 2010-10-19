namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum GIL_IN : uint
    {
        GIL_ASYNC = 0x20,
        GIL_DEFAULTICON = 0x40,
        GIL_FORSHELL = 2,
        GIL_FORSHORTCUT = 0x80,
        GIL_OPENICON = 1
    }
}

