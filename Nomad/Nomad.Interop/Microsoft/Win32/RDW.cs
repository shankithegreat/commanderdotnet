namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum RDW : uint
    {
        RDW_ALLCHILDREN = 0x80,
        RDW_ERASE = 4,
        RDW_ERASENOW = 0x200,
        RDW_FRAME = 0x400,
        RDW_INTERNALPAINT = 2,
        RDW_INVALIDATE = 1,
        RDW_NOCHILDREN = 0x40,
        RDW_NOERASE = 0x20,
        RDW_NOFRAME = 0x800,
        RDW_NOINTERNALPAINT = 0x10,
        RDW_UPDATENOW = 0x100,
        RDW_VALIDATE = 8
    }
}

