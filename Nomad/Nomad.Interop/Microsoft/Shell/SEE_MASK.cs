namespace Microsoft.Shell
{
    using System;

    [Flags]
    public enum SEE_MASK : uint
    {
        SEE_MASK_ASYNCOK = 0x100000,
        SEE_MASK_CLASSKEY = 3,
        SEE_MASK_CLASSNAME = 1,
        SEE_MASK_CONNECTNETDRV = 0x80,
        SEE_MASK_DOENVSUBST = 0x200,
        SEE_MASK_FLAG_DDEWAIT = 0x100,
        SEE_MASK_FLAG_LOG_USAGE = 0x4000000,
        SEE_MASK_FLAG_NO_UI = 0x400,
        SEE_MASK_HMONITOR = 0x200000,
        SEE_MASK_HOTKEY = 0x20,
        SEE_MASK_ICON = 0x10,
        SEE_MASK_IDLIST = 4,
        SEE_MASK_INVOKEIDLIST = 12,
        SEE_MASK_NO_CONSOLE = 0x8000,
        SEE_MASK_NOASYNC = 0x100,
        SEE_MASK_NOCLOSEPROCESS = 0x40,
        SEE_MASK_NOQUERYCLASSSTORE = 0x1000000,
        SEE_MASK_NOZONECHECKS = 0x800000,
        SEE_MASK_UNICODE = 0x4000,
        SEE_MASK_WAITFORINPUTIDLE = 0x2000000
    }
}

